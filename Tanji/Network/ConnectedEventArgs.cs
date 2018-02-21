using System;

using Sulakore.Communication;

namespace Tanji.Network
{
    public class ConnectedEventArgs : EventArgs
    {
        public HotelEndPoint HotelServer { get; set; }
        public bool IsFakingPolicyRequest { get; set; }

        public ConnectedEventArgs(HotelEndPoint hotelServer)
        {
            HotelServer = hotelServer;
        }
    }
}