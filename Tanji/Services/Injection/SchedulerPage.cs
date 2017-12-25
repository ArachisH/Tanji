using System;
using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SchedulerPage : ObservablePage, IHaltable
    {
        public SchedulerPage()
        {
            InitializeComponent();
        }

        #region IHaltable Implementation
        public void Halt()
        {
            throw new NotImplementedException();
        }
        public void Restore()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}