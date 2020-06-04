using System;
using System.Globalization;

using Sulakore.Network;

namespace Tangine.Helpers.Converters
{
    public class HotelEndPointConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as HotelEndPoint)?.ToString() ?? "*:*";
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string address = value.ToString();
            string[] points = address.Split(':');

            HotelEndPoint endpoint = null;
            if (points.Length == 2)
            {
                string[] ports = points[1].Split(',');
                if (ports.Length > 0 && ushort.TryParse(ports[0], out ushort port))
                {
                    HotelEndPoint.TryParse(points[0], port, out endpoint);
                }
            }
            return endpoint;
        }
    }
}