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
    public partial class MainFrm : NotifiableForm, IHaltable
    {
        private readonly PacketLoggerFrm _loggerUI;

        public new bool TopMost
        {
            get => base.TopMost;
            set
            {
                base.TopMost = value;
                RaiseOnPropertyChanged();

                _loggerUI.BringToFront();
            }
        }

        public MainFrm()
        {
            _loggerUI = new PacketLoggerFrm();

            InitializeComponent();

            Bind(_loggerUI, "TopMost", "TopMost");
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

            _loggerUI.Hide();
        }
        public void Restore(ConnectedEventArgs e)
        {
            if (e.HotelServer == null)
            {
                HPacket endPointPkt = Master.Connection.Local.ReceiveAsync().Result;
                e.HotelServer = ConnectionPg.HotelServer = HotelEndPoint.Parse(endPointPkt.ReadUTF8().Split('\0')[0], endPointPkt.ReadInt32());

                e.IsFakingPolicyRequest = e.HotelServer.Hotel == HHotel.Unknown;
                e.HotelServerSource.SetResult(e.HotelServer);
            }

            Text = $"Tanji - Connected[{e.HotelServer}]";
            TopMost = _loggerUI.TopMost;

            _loggerUI.Show();
        }
        #endregion
    }
}