using System;
using System.ComponentModel;

using Tanji.Controls;

using Sulakore.Network;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class FiltersPage : ObservablePage, IHaltable, IReceiver
    {
        public FiltersPage()
        {
            InitializeComponent();
        }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore()
        { }
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