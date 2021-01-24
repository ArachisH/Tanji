using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class InspectorPage : NotifiablePage
    {
        public InspectorPage()
        {
            InitializeComponent();
        }

        private void SaveAsBtn_Click(object sender, System.EventArgs e)
        {

        }
    }
}