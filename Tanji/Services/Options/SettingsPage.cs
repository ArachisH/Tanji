using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Options
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SettingsPage : ObservablePage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}