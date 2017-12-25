using System;
using System.ComponentModel;

using Tanji.Controls;

using Sulakore.Network;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ModulesPage : ObservablePage, IHaltable, IReceiver
    {
        public ModulesPage()
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

        #region IReceiver Implementation
        public bool IsReceiving { get; }
        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public void HandleIncoming(DataInterceptedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}