using System.ComponentModel;

using Tanji.Network;
using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SchedulerPage : NotifiablePage, IHaltable
    {
        public SchedulerPage()
        {
            InitializeComponent();
        }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        { }
        #endregion
    }
}