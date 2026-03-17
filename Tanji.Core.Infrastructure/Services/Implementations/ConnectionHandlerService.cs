using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.ObjectModel;

using Tanji.Core;
using Tanji.Core.Net;
using Tanji.Core.Canvas;
using Tanji.Core.Net.Interception;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using CommunityToolkit.HighPerformance.Buffers;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.Configuration;
using Tanji.Core.Infrastructure.Factories;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class ConnectionHandlerService : IConnectionHandlerService
{
    private static ReadOnlySpan<byte> XDPRequestBytes => "<policy-file-request/>\0"u8;
    private static readonly ReadOnlyMemory<byte> XDPResponseBytes = Encoding.UTF8.GetBytes("<cross-domain-policy><allow-access-from domain=\"*\" to-ports=\"*\"/></cross-domain-policy>\0");

    private readonly TanjiOptions _options;
    private readonly IClientHandlerService _clientHandler;
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ConnectionHandlerService> _logger;
    private readonly IRemoteEndPointResolverService<HotelEndPoint> _endPointResolver;

    public ObservableCollection<HConnection> Connections { get; } = [];

    public string? Username { get; } = null;
    public string? Password { get; } = null;
    public IPEndPoint? Socks5EndPoint { get; }

    public ConnectionHandlerService(ILogger<ConnectionHandlerService> logger,
        IOptions<TanjiOptions> options,
        IConnectionFactory connectionFactory,
        IClientHandlerService clientHandler,
        IRemoteEndPointResolverService<HotelEndPoint> endPointResolver)
    {
        _logger = logger;
        _options = options.Value;
        _clientHandler = clientHandler;
        _endPointResolver = endPointResolver;
        _connectionFactory = connectionFactory;
    }

    public async Task<HConnection> InterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            _logger.LogError("Ticket should be provided when attempting to launch the client.");
            ThrowHelper.ThrowArgumentNullException(nameof(ticket));
        }

        HNode? local = await AcceptLocalNodeAsync(context, _options.GameListenPort, cancellationToken).ConfigureAwait(false);
        if (local == null || !local.IsConnected)
        {
            _logger.LogError("Failed to intercept the local connection attempt from the client.");
            throw new Exception("Failed to intercept the local connection attempt from the client.");
        }

        HotelEndPoint remoteEndPoint = context.PatchingOptions.Patches.HasFlag(HPatches.InjectAddressShouter)
            ? await _endPointResolver.ResolveAsync(local, context, cancellationToken).ConfigureAwait(false)
            : await _endPointResolver.ResolveAsync(ticket, cancellationToken).ConfigureAwait(false);

        if (remoteEndPoint == null)
        {
            _logger.LogError("Failed to resolve the remote address from the address shouting mechanism.");
            throw new Exception("Failed to resolve the remote address from the address shouting mechanism.");
        }

        // TODO: Use ProxyFactory to acquire proxy instances to apply to the remote connection.

        HNode remote = await EstablishRemoteConnectionAsync(context, remoteEndPoint, cancellationToken).ConfigureAwait(false);
        HConnection connection = _connectionFactory.Create(local, remote, context);

        // Allow services to bind to specific packets before bridging the nodes.
        Connections.Add(connection);

        _ = connection.BridgeNodesAsync(cancellationToken);
        return connection;
    }

    public static async Task<HNode?> AcceptLocalNodeAsync(HConnectionContext context, int port, CancellationToken cancellationToken = default)
    {
        HNode? local = null;
        int listenSkipAmount = context.MinimumConnectionAttempts;
        while (!cancellationToken.IsCancellationRequested && (local == null || local.IsDisposed))
        {
            Socket localSocket = await AcceptAsync(port, cancellationToken).ConfigureAwait(false);
            local = new HNode(localSocket, context.InboundPacketFormat);

            if (--listenSkipAmount > 0)
            {
                local.Dispose();
                continue;
            }

            if (context.IsWebSocketConnection)
            {
                if (context.WebSocketServerCertificate == null)
                {
                    ThrowHelper.ThrowNullReferenceException("No certificate was provided for local authentication using the WebSocket Secure protocol.");
                }

                cancellationToken.ThrowIfCancellationRequested();
                await local.UpgradeToWebSocketServerAsync(context.WebSocketServerCertificate, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (context.IsFakingPolicyRequest)
            {
                using var buffer = MemoryOwner<byte>.Allocate(512);

                int received = await local.ReceiveAsync(buffer.Memory, cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                if (!buffer.Span.Slice(0, received).SequenceEqual(XDPRequestBytes))
                {
                    ThrowHelper.ThrowNotSupportedException("Expected cross-domain policy request.");
                }

                await local.SendAsync(XDPResponseBytes, cancellationToken).ConfigureAwait(false);
                local.Dispose();
            }
        }
        return local;
    }
    public static async Task<HNode> EstablishRemoteConnectionAsync(HConnectionContext context, IPEndPoint remoteEndPoint, CancellationToken cancellationToken = default)
    {
        Socket remoteSocket = await ConnectAsync(remoteEndPoint, cancellationToken).ConfigureAwait(false);
        var remote = new HNode(remoteSocket, context.InboundPacketFormat);

        if (context.IsWebSocketConnection)
        {
            await remote.UpgradeToWebSocketClientAsync(context.WebSocketClientCertificate, cancellationToken).ConfigureAwait(false);
        }
        return remote;
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
    private static async ValueTask<bool> TryApplyProxyAsync(HNode proxiedNode, IPEndPoint targetEndPoint, string? username, string? password, CancellationToken cancellationToken = default)
    {
        await proxiedNode.SendAsync(new byte[]
        {
            0x05, // Version 5
            0x02, // 2 Authentication Methods Present
            0x00, // No Authentication
            0x02  // Username + Password
        }, cancellationToken).ConfigureAwait(false);

        var response = new byte[2];
        int received = await proxiedNode.ReceiveAsync(response, cancellationToken).ConfigureAwait(false);
        if (received != 2 || response[1] == 0xFF) return false;

        int index;
        byte[]? payload;
        if (response[1] == 0x02) // Username + Password Required
        {
            index = 0;
            payload = new byte[byte.MaxValue];
            payload[index++] = 0x01;

            // Username
            payload[index++] = (byte)username.Length;
            byte[] usernameData = Encoding.Default.GetBytes(username);
            Buffer.BlockCopy(usernameData, 0, payload, index, usernameData.Length);
            index += usernameData.Length;

            // Password
            payload[index++] = (byte)password.Length;
            byte[] passwordData = Encoding.Default.GetBytes(password);
            Buffer.BlockCopy(passwordData, 0, payload, index, passwordData.Length);
            index += passwordData.Length;

            await proxiedNode.SendAsync(payload.AsMemory().Slice(0, index), cancellationToken).ConfigureAwait(false);
            received = await proxiedNode.ReceiveAsync(response, cancellationToken).ConfigureAwait(false);

            if (received != 2 || response[1] != 0x00) return false;
        }

        index = 0;
        payload = new byte[255];
        payload[index++] = 0x05;
        payload[index++] = 0x01;
        payload[index++] = 0x00;
        payload[index++] = (byte)(targetEndPoint.AddressFamily == AddressFamily.InterNetwork ? 0x01 : 0x04);

        // Destination Address
        byte[] addressBytes = targetEndPoint.Address.GetAddressBytes();
        Buffer.BlockCopy(addressBytes, 0, payload, index, addressBytes.Length);
        index += (ushort)addressBytes.Length;

        byte[] portData = BitConverter.GetBytes((ushort)targetEndPoint.Port);
        if (BitConverter.IsLittleEndian)
        {
            // Big-Endian Byte Order
            Array.Reverse(portData);
        }
        Buffer.BlockCopy(portData, 0, payload, index, portData.Length);
        index += portData.Length;

        await proxiedNode.SendAsync(payload.AsMemory().Slice(0, index), cancellationToken).ConfigureAwait(false);

        byte[] finalResponseBuffer = new byte[byte.MaxValue];
        received = await proxiedNode.ReceiveAsync(finalResponseBuffer, cancellationToken);

        return received >= 2 && response[1] == 0x00;
    }
}