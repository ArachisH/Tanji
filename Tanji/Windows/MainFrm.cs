using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Network;
using Tanji.Controls;
using Tanji.Services;

using Sulakore.Habbo;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class MainFrm : ObservableForm, IHaltable
    {
        private readonly PacketLoggerFrm _packetLogger;

        public new bool TopMost
        {
            get => base.TopMost;
            set
            {
                base.TopMost = value;
                RaiseOnPropertyChanged();

                _packetLogger.BringToFront();
            }
        }

        public MainFrm()
        {
            _packetLogger = new PacketLoggerFrm();

            InitializeComponent();

            Bind(_packetLogger, "TopMost", "TopMost");
        }

        private void PacketTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
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
            e.HotelServer = ConnectionPg.HotelServer = HotelEndPoint.Parse(endPointPkt.ReadUTF8().Split('\0')[0], endPointPkt.ReadInt32());
            e.IsFakingPolicyRequest = e.HotelServer.Hotel == HHotel.Unknown;
            e.HotelServerSource.SetResult(e.HotelServer);

            Text = $"Tanji - Connected[{e.HotelServer}]";
            TopMost = _packetLogger.TopMost;

            _packetLogger.Show();
        }
        #endregion
    }
}