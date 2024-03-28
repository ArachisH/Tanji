using System.Net;
using System.Text;
using System.Net.Sockets;

using Tanji.Core.Habbo.Network;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Network;

/// <summary>
/// Represents a reusable 'bridge' that transfers data to/from two separate <see cref="HNode"/> instances.
/// </summary>
public sealed class HConnection : IHConnection
{
    private static ReadOnlySpan<byte> XDPRequestBytes => "<policy-file-request/>\0"u8;
    private static readonly ReadOnlyMemory<byte> XDPResponseBytes = Encoding.UTF8.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0");

    private Task? _weldTask;
    private CancellationTokenSource? _interceptCancellationSource;

    public HNode? Local { get; private set; }
    public HNode? Remote { get; private set; }

    public int TotalInboundPackets { get; }
    public int TotalOutboundPackets { get; }
    public bool IsConnected => Local != null && Remote != null && Local.IsConnected & Remote.IsConnected;

    public HConnectionContext Context { get; }

    public HConnection(HConnectionContext context)
    {
        Context = context;
    }

    public Task AttachNodesAsync(CancellationToken cancellationToken = default)
    {
        if (_weldTask != null && !_weldTask.IsCompleted)
        {
            return _weldTask;
        }

        Task localToRemote = AttachNodesAsync(Local!, Remote!, true, cancellationToken);
        Task remoteToLocal = AttachNodesAsync(Remote!, Local!, false, cancellationToken);
        return _weldTask = Task.WhenAll(localToRemote, remoteToLocal);
    }
    public async Task InterceptLocalConnectionAsync(CancellationToken cancellationToken = default)
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
            int listenSkipAmount = Context.MinimumConnectionAttempts;
            while ((Local == null || !Local.IsConnected) && !cancellationToken.IsCancellationRequested)
            {
                Socket localSocket = await AcceptAsync(Context.AppliedPatchingOptions.InjectedAddress!.Port, cancellationToken).ConfigureAwait(false);
                Local = new HNode(localSocket, Context.ReceivePacketFormat);

                if (--listenSkipAmount > 0)
                {
                    Local.Dispose();
                    continue;
                }

                if (Context.IsWebSocketConnection)
                {
                    if (Context.WebSocketServerCertificate == null)
                    {
                        ThrowHelper.ThrowNullReferenceException("No certificate was provided for local authentication using the WebSocket Secure protocol.");
                    }

                    if (cancellationToken.IsCancellationRequested) return;
                    await Local.UpgradeToWebSocketServerAsync(Context.WebSocketServerCertificate, cancellationToken).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) return;
                if (Context.IsFakingPolicyRequest)
                {
                    using MemoryOwner<byte> buffer = MemoryOwner<byte>.Allocate(512);

                    int received = await Local.ReceiveAsync(buffer.Memory, cancellationToken).ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    if (!buffer.Span.Slice(0, received).SequenceEqual(XDPRequestBytes))
                    {
                        ThrowHelper.ThrowNotSupportedException("Expected cross-domain policy request.");
                    }

                    await Local.SendAsync(XDPResponseBytes, cancellationToken).ConfigureAwait(false);
                    Local.Dispose();
                }
            }
        }
        finally
        {
            if (Local != null && (!Local.IsConnected || cancellationToken.IsCancellationRequested))
            {
                Local.Dispose();
            }
            CancelAndNullifySource(ref _interceptCancellationSource);
            CancelAndNullifySource(ref linkedInterceptCancellationSource);
        }
    }
    public async Task EstablishRemoteConnectionAsync(IPEndPoint remoteEndPoint, CancellationToken cancellationToken = default)
    {
        Socket remoteSocket = await ConnectAsync(remoteEndPoint, cancellationToken).ConfigureAwait(false);
        Remote = new HNode(remoteSocket, Context.ReceivePacketFormat);

        if (Context.WebSocketServerCertificate != null)
        {
            await Remote.UpgradeToWebSocketClientAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
    public void Disconnect()
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
    }

    private async Task AttachNodesAsync(HNode source, HNode destination, bool isOutbound, CancellationToken cancellationToken = default)
    {
        int received;
        while (source.IsConnected && destination.IsConnected && !cancellationToken.IsCancellationRequested)
        {
            // Do not dispose 'bufferWriter' here, instead, dispose of it within the 'TransferPacketAsync' method
            var writer = new ArrayPoolBufferWriter<byte>(source.ReceivePacketFormat.MinBufferSize);
            received = await source.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

            if (received > 0)
            {
                // Continuously attempt to receive packets from the node
                _ = HandleInterceptedPacketAsync(received, source, destination, isOutbound, writer, cancellationToken);
            }
            else writer.Dispose();
        }
    }
    private async Task HandleInterceptedPacketAsync(int received, HNode source, HNode destination, bool isOutbound, ArrayPoolBufferWriter<byte> writer, CancellationToken cancellationToken = default)
    {
        try
        {
            Memory<byte> buffer = writer.DangerousGetArray();
            if (buffer.Length != writer.WrittenCount || buffer.Length != received)
            { }
            //if (Spooler != null)
            //{
            //    ValueTask<bool> packetProcessTask = isOutbound
            //        ? Spooler.PacketOutboundAsync(buffer, source, destination)
            //        : Spooler.PacketInboundAsync(buffer, source, destination);

            //    // If true, the packet is to be ignored/blocked
            //    if (await packetProcessTask.ConfigureAwait(false)) return;
            //}
            await destination.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        finally { writer.Dispose(); }
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
    private static async ValueTask<Socket> AcceptAsync(int port, CancellationToken cancellationToken = default)
    {
        using var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listenSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        listenSocket.LingerState = new LingerOption(false, 0);
        listenSocket.Listen(1);

        return await listenSocket.AcceptAsync(cancellationToken).ConfigureAwait(false);
    }
    private static async ValueTask<Socket> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken = default)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await socket.ConnectAsync(remoteEndPoint, cancellationToken).ConfigureAwait(false);
        }
        catch { /* Ignore all exceptions. */ }
        if (!socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        return socket;
    }
}