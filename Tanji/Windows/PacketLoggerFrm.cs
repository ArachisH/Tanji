using System.ComponentModel;

using Tanji.Controls;
using Tanji.Services;

using Sulakore.Network;
using System.Windows.Forms;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class PacketLoggerFrm : ObservableForm, IHaltable, IReceiver
    {
        public new bool TopMost
        {
            get => base.TopMost;
            set
            {
                base.TopMost = value;
                RaiseOnPropertyChanged();
            }
        }

        public PacketLoggerFrm()
        {
            InitializeComponent();
        }

        private void PacketLoggerFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }

        #region IHaltable Implementation
        public void Halt()
        {
            Close();
        }
        public void Restore()
        {
            WindowState = FormWindowState.Normal;
        }
        #endregion
        #region IReceiver Implementation
        public bool IsReceiving { get; }
        public void HandleOutgoing(DataInterceptedEventArgs e)
        { }
        public void HandleIncoming(DataInterceptedEventArgs e)
        { }
        #endregion
    }
}