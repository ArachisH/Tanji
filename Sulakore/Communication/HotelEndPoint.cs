using System;
using System.Net;

using Sulakore.Habbo;

namespace Sulakore.Communication
{
    public class HotelEndPoint : IPEndPoint
    {
        private string _host;
        public string Host
        {
            get { return _host; }
            set
            {
                _host = value;
                Hotel = GetHotel(value);
            }
        }

        public HHotel Hotel { get; private set; }

        public HotelEndPoint(IPEndPoint endpoint)
            : base(endpoint.Address, endpoint.Port)
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

        public static HHotel GetHotel(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                return HHotel.Unknown;
            }

            var hotel = HHotel.Unknown;
            if (!Enum.TryParse(host, true, out hotel))
            {
                int regionStart = host.IndexOf("game-");
                if (regionStart != -1)
                {
                    string region = host.Substring(regionStart + 5, 2);
                    switch (region)
                    {
                        case "us": return HHotel.Com;
                        case "br": return HHotel.ComBr;
                        case "tr": return HHotel.ComTr;

                        default:
                        Enum.TryParse(region, true, out hotel);
                        break;
                    }
                }
                else
                {
                    if (host[0] == '.')
                    {
                        host = host.Substring(1);
                    }
                    int domainStart = host.IndexOf("habbo.");
                    if (domainStart != -1)
                    {
                        domainStart += 6;
                        int domainEnd = host.IndexOf('/', domainStart);
                        if (domainEnd != -1)
                        {
                            string region = host.Substring(domainStart, ((domainEnd - domainStart) - 1));
                            region = region.Replace(".", string.Empty);
                            return GetHotel(region);
                        }
                    }
                }
            }
            return hotel;
        }
        public static HotelEndPoint Parse(string host, int port)
        {
            IPAddress[] ips = Dns.GetHostAddresses(host);
            return new HotelEndPoint(ips[0], port, host);
        }
        public static bool TryParse(string host, int port, out HotelEndPoint endpoint)
        {
            try
            {
                endpoint = Parse(host, port);
                return true;
            }
            catch
            {
                endpoint = null;
                return false;
            }
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Host))
            {
                return (Host + ":" + Port);
            }
            return base.ToString();
        }
    }
}