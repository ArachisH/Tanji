using System.ComponentModel;

using Tanji.Controls;
using Tanji.Services;

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
        public void Restore()
        {
            Text = $"Tanji - Connected[{Program.Master.Connection.Remote.EndPoint}]";
            TopMost = _packetLogger.TopMost;

            _packetLogger.Show();
        }
        #endregion
    }
}