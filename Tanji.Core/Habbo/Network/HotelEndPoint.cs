using System.Net;

namespace Tanji.Core.Habbo.Network;

public sealed class HotelEndPoint : IPEndPoint
{
    public string? Host { get; init; }
    public HHotel Hotel { get; init; }

    public HotelEndPoint(IPEndPoint endPoint)
        : base(endPoint.Address, endPoint.Port)
    { }
    public HotelEndPoint(long address, int port)
        : base(address, port)
    { }
    public HotelEndPoint(IPAddress address, int port)
        : base(address, port)
    { }
    public HotelEndPoint(IPAddress address, int port, string host)
        : base(address, port)
    {
        Host = host;
    }

    public static HotelEndPoint Create(HHotel hotel)
    {
        if (hotel == HHotel.Unknown)
        {
            ThrowHelper.ThrowArgumentException($"Unable to create a {nameof(HotelEndPoint)} object from {nameof(HHotel.Unknown)}.", nameof(hotel));
        }
        return Parse($"game-{hotel.ToRegion()}.habbo.com", 30001);
    }
    public static HotelEndPoint Create(ReadOnlySpan<char> host) => Create(host.ToHotel());

    public static HotelEndPoint Parse(string host, int port)
    {
        IPAddress[] ips = Dns.GetHostAddresses(host);
        return new HotelEndPoint(ips[0], port, host) { Host = host, Hotel = host.AsSpan().ToHotel() };
    }
    public static async Task<HotelEndPoint> ParseAsync(string host, int port, CancellationToken cancellationToken = default)
    {
        IPAddress[] ips = await Dns.GetHostAddressesAsync(host, cancellationToken).ConfigureAwait(false);
        return new HotelEndPoint(ips[0], port, host) { Host = host, Hotel = host.AsSpan().ToHotel() };
    }

    public override string ToString() => $"{(string.IsNullOrWhiteSpace(Host) ? Address.ToString() : Host)}:{Port}";
}