using System.Text;
using System.Buffers;
using System.IO.Pipelines;

using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Network;
using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Network;

/// <summary>
/// Represents a reusable 'bridge' that transfers data to/from two separate <see cref="HNode"/> instances.
/// </summary>
public sealed class HConnection : IHConnection
{
    private static readonly byte[] _crossDomainPolicyRequestBytes, _crossDomainPolicyResponseBytes;

    private readonly Pipe _pipe;

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

    public bool IsConnected => (Local?.IsConnected ?? false) && (Remote?.IsConnected ?? false);

    public IHFormat? SendFormat { get; }
    public IHFormat? ReceiveFormat { get; }

    public Incoming? In { get; set; }
    public Outgoing? Out { get; set; }
    public HotelEndPoint? RemoteEndPoint { get; private set; }

    public HNode? Local { get; private set; }
    public HNode? Remote { get; private set; }

    static HConnection()
    {
        _crossDomainPolicyRequestBytes = Encoding.UTF8.GetBytes("<policy-file-request/>\0");
        _crossDomainPolicyResponseBytes = Encoding.UTF8.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0");
    }
    public HConnection()
    {
        _pipe = new Pipe();
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
                Local = await HNode.AcceptAsync(options.ListenPort, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) return;

                if (--listenSkipAmount > 0)
                {
                    Local.Dispose();
                    continue;
                }

                /* If not null, assume we're dealing with the WebSocket protocol and attempt to upgrade this node as a WebSocket server. */
                if (options.IsWebSocketConnection)
                {
                    if (options.WebSocketServerCertificate == null)
                    {
                        ThrowHelper.ThrowNullReferenceException("No certificate was provided for local authentication using the WebSocket Secure protocol.");
                    }
                    await Local.UpgradeToWebSocketServerAsync(options.WebSocketServerCertificate, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;
                }

                /* Provide an opportunity for users to specify a different endpoint for the remote connection. */
                var args = new ConnectedEventArgs(options);
                Connected?.Invoke(this, args);

                // Replace the options initially provided, by the ones returned from the Connected event invocation.
                options = args.Options;

                if (options.RemoteEndPoint == null)
                {
                    ThrowHelper.ThrowNullReferenceException("Unable to establish a connection to the remote server without an endpoint.");
                }

                if (args.Cancel || cancellationToken.IsCancellationRequested) return;
                if (options.IsFakingPolicyRequest)
                {
                    using MemoryOwner<byte> buffer = MemoryOwner<byte>.Allocate(512);

                    int received = await Local.ReceiveAsync(buffer.Memory, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    if (!buffer.Span.Slice(0, received).SequenceEqual(_crossDomainPolicyRequestBytes))
                    {
                        ThrowHelper.ThrowNotSupportedException("Expected cross-domain policy request.");
                    }

                    await Local.SendAsync(_crossDomainPolicyResponseBytes, cancellationToken).ConfigureAwait(false);

                    // TODO: Check if we need to establish a remote connection here, do they keep track of policy request per IP? (Assume yes for retros perhaps?)
                    using var tempRemote = await HNode.ConnectAsync(options.RemoteEndPoint, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    await tempRemote.SendAsync(_crossDomainPolicyRequestBytes, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    Local.Dispose();
                    continue;
                }

                Remote = await HNode.ConnectAsync(options.RemoteEndPoint, cancellationToken).ConfigureAwait(false);
                if (cancellationToken.IsCancellationRequested) return;

                if (options.WebSocketServerCertificate != null)
                {
                    await Remote.UpgradeToWebSocketClientAsync(cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;
                }

                // Intercept the packets being sent to the remote server by the local client using the format the client uses to send data.
                _ = InterceptPacketsAsync(Local, options.ClientSendPacketFormat, true);

                // Intercept the packets being sent to the local client by the remote server using the format the client uses to receive data.
                _ = InterceptPacketsAsync(Remote, options.ClientReceivePacketFormat, false);
            }
        }
        finally
        {
            if (!IsConnected || cancellationToken.IsCancellationRequested)
            {
                Local?.Dispose();
                Remote?.Dispose();
            }
            CancelAndNullifySource(ref _interceptCancellationSource);
            CancelAndNullifySource(ref linkedInterceptCancellationSource);
        }
    }

    private async Task InterceptPacketsAsync(HNode node, IHFormat format, bool isOutgoing)
    {
        while (IsConnected)
        {
            await node.ReceivePacketAsync(_pipe.Writer).ConfigureAwait(false);
        }
    }

    public async ValueTask SendToServerAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (Remote == null)
        {
            ThrowHelper.ThrowNullReferenceException();
        }
        await Remote.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
    }
    public async ValueTask SendToClientAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (Local == null)
        {
            ThrowHelper.ThrowNullReferenceException();
        }
        await Local.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
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
        if (!Monitor.TryEnter(_pipe)) return;
        try
        {
            CancelAndNullifySource(ref _interceptCancellationSource);
            if (Local != null)
            {
                Local.Dispose();
                Local = null;
            }
            if (Remote != null)
            {
                Remote.Dispose();
                Remote = null;
            }
            if (IsConnected)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }
        finally { Monitor.Exit(_pipe); }
    }
}