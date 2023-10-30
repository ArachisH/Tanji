using System.Text;
using System.Buffers;
using System.Threading.Channels;

using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Network;
using Tanji.Core.Habbo.Network.Formats;
using Tanji.Core.Habbo.Network.Buffers;

namespace Tanji.Core.Network;

/// <summary>
/// Represents a reusable 'bridge' that transfers data to/from two separate <see cref="HNode"/> instances.
/// </summary>
public sealed class HConnection : IHConnection
{
    private static readonly byte[] _crossDomainPolicyRequestBytes, _crossDomainPolicyResponseBytes;

    private readonly object _disconnectLock;
    private readonly Channel<HPacket> _interceptedPackets;

    private HNode? _local, _remote;
    private CancellationTokenSource? _interceptCancellationSource;

    /// <summary>
    /// Occurs when the connection between the client, and server have been intercepted.
    /// </summary>
    public event EventHandler<ConnectedEventArgs>? Connected;

    /// <summary>
    /// Occurs when either the game client, or server have disconnected.
    /// </summary>
    public event EventHandler? Disconnected;

    /// <summary>
    /// Occurs when the client's outgoing data has been intercepted.
    /// </summary>
    public event EventHandler<DataInterceptedEventArgs>? DataOutgoing;

    /// <summary>
    /// Occrus when the server's incoming data has been intercepted.
    /// </summary>
    public event EventHandler<DataInterceptedEventArgs>? DataIncoming;

    public bool IsConnected => (_local?.IsConnected ?? false) && (_remote?.IsConnected ?? false);

    public IHFormat SendFormat { get; }
    public IHFormat ReceiveFormat { get; }

    public Incoming? In { get; set; }
    public Outgoing? Out { get; set; }
    public HotelEndPoint RemoteEndPoint { get; private set; }

    public HNode Local => _local;
    public HNode Remote => _remote;

    static HConnection()
    {
        _crossDomainPolicyRequestBytes = Encoding.UTF8.GetBytes("<policy-file-request/>\0");
        _crossDomainPolicyResponseBytes = Encoding.UTF8.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0");
    }
    public HConnection()
    {
        _disconnectLock = new object();
        _interceptedPackets = Channel.CreateUnbounded<HPacket>();
    }

    public async Task InterceptAsync(HConnectionOptions options, CancellationToken cancellationToken = default)
    {
        /* Reset the cancellation token. */
        CancelAndNullifySource(ref _interceptCancellationSource);
        _interceptCancellationSource = new CancellationTokenSource();

        /* Link both the internal, and user provided cancellation tokens with one another. */
        CancellationTokenSource? linkedInterceptCancellationSource = null;
        if (cancellationToken != default)
        {
            linkedInterceptCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_interceptCancellationSource.Token, cancellationToken);
            cancellationToken = linkedInterceptCancellationSource.Token; // This token will also 'cancel' when the Dispose or Disconnect method of this type is called.
        }

        try
        {
            int listenSkipAmount = options.MinimumConnectionAttempts;
            while (!IsConnected && !cancellationToken.IsCancellationRequested)
            {
                _local = await HNode.AcceptAsync(options.ListenPort, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) return;

                if (--listenSkipAmount > 0)
                {
                    _local.Dispose();
                    continue;
                }

                /* If not null, assume we're dealing with the WebSocket protocol and attempt to upgrade this node as a WebSocket server. */
                if (options.IsWebSocketConnection)
                {
                    if (options.WebSocketServerCertificate == null)
                    {
                        ThrowHelper.ThrowNullReferenceException("No certificate was provided for local authentication using the WebSocket Secure protocol.");
                    }
                    await _local.UpgradeToWebSocketServerAsync(options.WebSocketServerCertificate, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;
                }

                /* Provide an opportunity for users to specify a different endpoint for the remote connection. */
                var args = new ConnectedEventArgs(options);
                Connected?.Invoke(this, args);

                // Replace the options initially provided, by the ones returned from the Connected event invokation.
                options = args.Options;

                if (options.RemoteEndPoint == null)
                {
                    ThrowHelper.ThrowNullReferenceException("Unable to establish a connection to the remote server without an endpoint.");
                }

                if (args.Cancel || cancellationToken.IsCancellationRequested) return;
                if (options.IsFakingPolicyRequest)
                {
                    using IMemoryOwner<byte> bufferOwner = MemoryPool<byte>.Shared.Rent(512);
                    Memory<byte> buffer = bufferOwner.Memory;

                    int received = await _local.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    if (!buffer.Slice(0, received).Span.SequenceEqual(_crossDomainPolicyRequestBytes))
                    {
                        ThrowHelper.ThrowNotSupportedException("Expected cross-domain policy request.");
                    }

                    await _local.SendAsync(_crossDomainPolicyResponseBytes, cancellationToken).ConfigureAwait(false);

                    // TODO: Check if we need to establish a remote connection here, do they keep track of policy request per IP? (Assume yes for retros perhaps?)
                    using var tempRemote = await HNode.ConnectAsync(options.RemoteEndPoint, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    await tempRemote.SendAsync(_crossDomainPolicyRequestBytes, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    _local.Dispose();
                    continue;
                }

                _remote = await HNode.ConnectAsync(options.RemoteEndPoint, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) return;

                if (options.WebSocketServerCertificate != null)
                {
                    await _remote.UpgradeToWebSocketClientAsync(cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;
                }

                // Intercept the packets being sent to the remote server by the local client using the format the client uses to send data.
                _ = InterceptPacketsAsync(_local, options.ClientSendPacketFormat, true);

                // Intercept the packets being send to the local client by the remote server using the format the client uses to receive data.
                _ = InterceptPacketsAsync(_remote, options.ClientReceivePacketFormat, false);

                // Process intercepted packets as they come in.
                _ = HandleInterceptedPacketsAsync();
            }
        }
        finally
        {
            if (!IsConnected || cancellationToken.IsCancellationRequested)
            {
                _local?.Dispose();
                _remote?.Dispose();
            }
            CancelAndNullifySource(ref _interceptCancellationSource);
            CancelAndNullifySource(ref linkedInterceptCancellationSource);
        }
    }

    private async Task HandleInterceptedPacketsAsync()
    {
        await foreach (HPacket packet in _interceptedPackets.Reader.ReadAllAsync().ConfigureAwait(false))
        {
            try
            {
                // TODO: 
            }
            finally { packet.Writer.Dispose(); }
        }
    }
    private async Task InterceptPacketsAsync(HNode node, IHFormat format, bool isOutgoing)
    {
        while (IsConnected)
        {
            var writer = new HPacketWriter(format);
            short id = await node.ReceivePacketAsync(writer).ConfigureAwait(false);

            var packet = new HPacket(writer, id, isOutgoing, node.PacketsReceived);
            await _interceptedPackets.Writer.WriteAsync(packet).ConfigureAwait(false);

        }
    }

    public async ValueTask SendToServerAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (_remote == null)
        {
            ThrowHelper.ThrowNullReferenceException();
        }
        await _remote.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
    }
    public async ValueTask SendToClientAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (_local == null)
        {
            ThrowHelper.ThrowNullReferenceException();
        }
        await _local.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    private static void CancelAndNullifySource(ref CancellationTokenSource? cancellationTokenSource)
    {
        if (cancellationTokenSource == null) return;
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            cancellationTokenSource.Cancel();
        }
        cancellationTokenSource.Dispose();
        cancellationTokenSource = null;
    }

    public void Dispose()
    {
        Disconnect();
    }
    public void Disconnect()
    {
        if (!Monitor.TryEnter(_disconnectLock)) return;
        try
        {
            CancelAndNullifySource(ref _interceptCancellationSource);
            if (_local != null)
            {
                _local.Dispose();
                _local = null;
            }
            if (_remote != null)
            {
                _remote.Dispose();
                _remote = null;
            }
            if (IsConnected)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }
        finally { Monitor.Exit(_disconnectLock); }
    }
}