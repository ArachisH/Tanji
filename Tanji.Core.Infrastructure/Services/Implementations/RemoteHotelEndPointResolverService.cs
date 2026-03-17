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

    public Task<HotelEndPoint> ResolveAsync(string ticket, CancellationToken cancellationToken = default)
    {
        HHotel hotel = HExtensions.ToHotel(ticket);
        string host = $"game-{hotel.ToRegion()}.habbo.com";

        return HotelEndPoint.ParseAsync(host, 30000, cancellationToken);
    }
    public async Task<HotelEndPoint> ResolveAsync(HNode local, HConnectionContext context, CancellationToken cancellationToken = default)
    {
        using var packetBufferWriter = new ArrayPoolBufferWriter<byte>(128);
        int received = await local.ReceivePacketAsync(packetBufferWriter, cancellationToken).ConfigureAwait(false);

        HotelEndPoint? remoteEndPoint = await ParseRemoteEndPointAsync(context.OutboundPacketFormat, packetBufferWriter.WrittenSpan).ConfigureAwait(false)
            ?? throw new Exception("Failed to parse the remote end point from the intercepted packet.");

        _logger.LogDebug("Resolved Remote end point: {endpoint}, {received:n0} Packet Bytes", remoteEndPoint, received);
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