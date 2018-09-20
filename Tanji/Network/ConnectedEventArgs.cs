using System;
using System.Threading.Tasks;

using Sulakore.Communication;

namespace Tanji.Network
{
    public class ConnectedEventArgs : EventArgs
    {
        public HotelEndPoint HotelServer { get; set; }
        public bool IsFakingPolicyRequest { get; set; }
        public TaskCompletionSource<HotelEndPoint> HotelServerSource { get; }

        public ConnectedEventArgs(HotelEndPoint hotelServer)
        {
            HotelServer = hotelServer;
            HotelServerSource = new TaskCompletionSource<HotelEndPoint>();
        }
    }
}