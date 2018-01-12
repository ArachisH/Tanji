using System.ComponentModel;

using Tanji.Network;
using Tanji.Controls;
using Tanji.Services;

using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class MainFrm : ObservableForm, IHaltable
    {
        private readonly PacketLoggerFrm _packetLogger;

        public MainFrm()
        {
            _packetLogger = new PacketLoggerFrm();

            InitializeComponent();

            Bind(_packetLogger, "TopMost", "TopMost");
        }

        #region IHaltable Implementation
        public void Halt()
        {
            Text = "Tanji - Disconnected";
            TopMost = true;

            _packetLogger.Hide();
        }
        public void Restore(ConnectedEventArgs e)
        {
            HPacket endPointPkt = Master.Connection.Local.ReceivePacketAsync().Result;
            if (ConnectionPg.IsExtractingHotelServer)
            {
                string host = endPointPkt.ReadUTF8();
                int port = int.Parse(endPointPkt.ReadUTF8().Split(',')[0]);
                e.HotelServer = ConnectionPg.HotelServer = HotelEndPoint.Parse(host, port);
            }

            Text = $"Tanji - Connected[{e.HotelServer}]";
            TopMost = _packetLogger.TopMost;

            _packetLogger.Show();
        }
        #endregion
    }
}