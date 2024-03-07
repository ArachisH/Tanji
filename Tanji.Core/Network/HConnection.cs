using System.Net;
using System.Text;
using System.Net.Sockets;

using Tanji.Core.Habbo;
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

    private CancellationTokenSource? _interceptCancellationSource;

    public HNode? Local { get; private set; }
    public HNode? Remote { get; private set; }

    public Incoming? In { get; set; }
    public Outgoing? Out { get; set; }

    public bool IsConnected => Local != null && Remote != null && Local.IsConnected & Remote.IsConnected;

    public async ValueTask InterceptLocalConnectionAsync(HConnectionContext context, CancellationToken cancellationToken = default)
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
            int listenSkipAmount = context.MinimumConnectionAttempts;
            while ((Local == null || !Local.IsConnected) && !cancellationToken.IsCancellationRequested)
            {
                Socket localSocket = await AcceptAsync(context.AppliedPatchingOptions.InjectedAddress!.Port, cancellationToken).ConfigureAwait(false);
                Local = new HNode(localSocket, context.ReceivePacketFormat);

                if (--listenSkipAmount > 0)
                {
                    Local.Dispose();
                    continue;
                }

                if (context.IsWebSocketConnection)
                {
                    if (context.WebSocketServerCertificate == null)
                    {
                        ThrowHelper.ThrowNullReferenceException("No certificate was provided for local authentication using the WebSocket Secure protocol.");
                    }

                    if (cancellationToken.IsCancellationRequested) return;
                    await Local.UpgradeToWebSocketServerAsync(context.WebSocketServerCertificate, cancellationToken).ConfigureAwait(false);
                }

                if (cancellationToken.IsCancellationRequested) return;
                if (context.IsFakingPolicyRequest)
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
    public async ValueTask EstablishRemoteConnection(HConnectionContext context, IPEndPoint remoteEndPoint, CancellationToken cancellationToken = default)
    {
        Socket remoteSocket = await ConnectAsync(remoteEndPoint, cancellationToken).ConfigureAwait(false);
        Remote = new HNode(remoteSocket, context.ReceivePacketFormat);

        if (context.WebSocketServerCertificate != null)
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