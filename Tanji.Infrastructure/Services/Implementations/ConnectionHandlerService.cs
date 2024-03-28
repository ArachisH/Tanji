using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.ObjectModel;

using Tanji.Core;
using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Habbo.Network;
using Tanji.Infrastructure.Factories;
using Tanji.Core.Habbo.Network.Buffers;
using Tanji.Core.Habbo.Network.Formats;

using Microsoft.Extensions.Logging;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Infrastructure.Services.Implementations;

public sealed class ConnectionHandlerService : IConnectionHandlerService
{
    private readonly IClientHandlerService _clientHandler;
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ConnectionHandlerService> _logger;

    public ObservableCollection<IHConnection> Connections { get; } = [];

    public string? Username { get; } = null;
    public string? Password { get; } = null;
    public IPEndPoint? Socks5EndPoint { get; }

    public ConnectionHandlerService(ILogger<ConnectionHandlerService> logger,
        IConnectionFactory connectionFactory,
        IClientHandlerService clientHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
        _connectionFactory = connectionFactory;
    }

    public async Task<IHConnection> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            _logger.LogError("Ticket should be provided when attempting to launch the client.");
            ThrowHelper.ThrowArgumentNullException(nameof(ticket));
        }

        IHConnection connection = _connectionFactory.Create(context);
        // Begin intercepting for connection attempts from the client before launching the client.
        Task interceptLocalConnectionTask = connection.InterceptLocalConnectionAsync(cancellationToken);
        _ = await _clientHandler.LaunchClientAsync(context.Platform, ticket, context.ClientPath).ConfigureAwait(false);

        // Wait for the intercepted connection
        await interceptLocalConnectionTask.ConfigureAwait(false);
        if (connection.Local == null || !connection.Local.IsConnected)
        {
            _logger.LogError("Local connection to the client has not been established.");
            throw new Exception("Local connection to the client has not been established.");
        }

        if (context.AppliedPatchingOptions.Patches.HasFlag(HPatches.InjectAddressShouter))
        {
            using var writer = new ArrayPoolBufferWriter<byte>(32);
            int written = await connection.Local.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

            IPEndPoint? remoteEndPoint = ParseRemoteEndPoint(context.SendPacketFormat, writer.WrittenSpan);
            if (remoteEndPoint == null)
            {
                _logger.LogError("Failed to parse the remote endpoint from the intercepted packet.");
                throw new Exception("Failed to parse the remote endpoint from the intercepted packet.");
            }

            await connection.EstablishRemoteConnectionAsync(Socks5EndPoint ?? remoteEndPoint, cancellationToken).ConfigureAwait(false);
            if (Socks5EndPoint != null)
            {
                bool wasProxiedSuccessfully = await TryApplyProxyAsync(connection.Remote!, remoteEndPoint!, Username!, Password!, cancellationToken).ConfigureAwait(false);
            }
            _ = connection.AttachNodesAsync(cancellationToken);
        }

        Connections.Add(connection);
        return connection;
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

        if (received < 2 || response[1] != 0x00) return false;

        return true;
    }

    private static IPEndPoint? ParseRemoteEndPoint(IHFormat packetFormat, ReadOnlySpan<byte> packetSpan)
    {
        var pktReader = new HPacketReader(packetFormat, packetSpan);
        string hostNameOrAddress = pktReader.ReadUTF8().Split('\0')[0];
        int port = pktReader.Read<int>();

        if (!IPAddress.TryParse(hostNameOrAddress, out IPAddress? address))
        {
            IPAddress[] addresses = Dns.GetHostAddresses(hostNameOrAddress);
            if (addresses.Length > 0) address = addresses[0];
        }

        return address != null ? new IPEndPoint(address, port) : null;
    }
}