using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Toolbox
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ToolboxPage : NotifiablePage
    {
        public ToolboxPage()
        {
            InitializeComponent();
        }
    }
}