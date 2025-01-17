using System.Net;

using Tanji.Core.Net;
using Tanji.Core.Canvas;
using Tanji.Core.Net.Buffers;
using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Interception;

using CommunityToolkit.HighPerformance.Buffers;

using Microsoft.Extensions.Logging;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class RemoteHotelEndPointResolverService : IRemoteEndPointResolverService<HotelEndPoint>
{
    private readonly ILogger<RemoteHotelEndPointResolverService> _logger;

    public RemoteHotelEndPointResolverService(ILogger<RemoteHotelEndPointResolverService> logger)
    {
        _logger = logger;
    }

    public async Task<HotelEndPoint> ResolveAsync(string ticket, CancellationToken cancellationToken = default)
    {
        HHotel hotel = HExtensions.ToHotel(ticket);
        string host = $"game-{hotel.ToRegion()}.habbo.com";

        IPAddress[] addresses = await Dns.GetHostAddressesAsync(host, cancellationToken).ConfigureAwait(false);
        if (addresses.Length < 1)
        {
            _logger.LogCritical("Failed to resolve the external IP address of the host '{host}'.", host);
            throw new Exception($"Failed to resolve the external IP address of the host '{host}'.");
        }

        // TODO: Adjust port based on platform
        return new HotelEndPoint(addresses[0], 30000, host)
        {
            Hotel = hotel
        };
    }
    public async Task<HotelEndPoint> ResolveAsync(HNode local, HConnectionContext context, CancellationToken cancellationToken = default)
    {
        using var writer = new ArrayPoolBufferWriter<byte>(128);
        _ = await local.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

        HotelEndPoint? remoteEndPoint = await ParseRemoteEndPointAsync(context.SendPacketFormat, writer.WrittenSpan).ConfigureAwait(false);
        if (remoteEndPoint == null)
        {
            _logger.LogError("Failed to parse the remote endpoint from the intercepted packet.");
            throw new Exception("Failed to parse the remote endpoint from the intercepted packet.");
        }
        return remoteEndPoint;
    }

    private static Task<HotelEndPoint> ParseRemoteEndPointAsync(IHFormat packetFormat, ReadOnlySpan<byte> packetSpan)
    {
        var pktReader = new HPacketReader(packetFormat, packetSpan);

        string hostNameOrAddress = pktReader.ReadUTF8().Split('\0')[0];
        int port = pktReader.Read<int>();

        return HotelEndPoint.ParseAsync(hostNameOrAddress, port);
    }
}