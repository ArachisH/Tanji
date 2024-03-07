using System.Net;
using System.Collections.ObjectModel;

using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Habbo.Network.Buffers;
using Tanji.Core.Habbo.Network.Formats;

using Microsoft.Extensions.Logging;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Services;

public sealed class ConnectionHandlerService : IConnectionHandlerService
{
    private readonly ILogger<ConnectionHandlerService> _logger;
    private readonly IClientHandlerService<CachedGame> _clientHandler;

    public ObservableCollection<HConnection> Connections { get; } = [];

    public ConnectionHandlerService(ILogger<ConnectionHandlerService> logger, IClientHandlerService<CachedGame> clientHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
    }

    public async Task<HConnection> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ticket))
        {
            _logger.LogError("Ticket should be provided when attempting to launch the client.");
            ThrowHelper.ThrowArgumentNullException(nameof(ticket));
        }

        var connection = new HConnection();
        ValueTask interceptLocalConnectionTask = connection.InterceptLocalConnectionAsync(context, cancellationToken);

        _ = _clientHandler.LaunchClient(context.Platform, ticket, context.ClientPath);
        await interceptLocalConnectionTask.ConfigureAwait(false);

        if (connection.Local == null || !connection.Local.IsConnected)
        {
            _logger.LogError("Failed to intercept local client connection attempt.");
            throw new Exception("Local connection to the client has not been established.");
        }

        if (context.AppliedPatchingOptions.Patches.HasFlag(HPatches.InjectAddressShouter))
        {
            using var writer = new ArrayPoolBufferWriter<byte>(32);
            int written = await connection.Local.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

            IPEndPoint? remoteEndPoint = ParseRemoteEndPoint(context.SendPacketFormat, writer.WrittenSpan);
            await connection.EstablishRemoteConnectionAsync(context, remoteEndPoint!, cancellationToken).ConfigureAwait(false);
        }

        Connections.Add(connection);
        return connection;
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