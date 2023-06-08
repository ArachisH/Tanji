using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Views.Pages;

[ToolboxItem(true)]
[DesignerCategory("UserControl")]
public partial class ExtensionsView : ExtensionsPageControl
{
    protected override BindingSource? Source => viewModelSource;

    public ExtensionsView()
    {
        InitializeComponent();
    }
}