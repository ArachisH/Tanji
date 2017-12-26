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
            _packetLogger = new PacketLoggerFrm(this);

            InitializeComponent();
        }

        #region IHaltable Implementation
        public void Halt()
        {
            TopMost = true;
        }
        public void Restore()
        {
            TopMost = _packetLogger.IsAlwaysOnTop;
        }
        #endregion
    }
}