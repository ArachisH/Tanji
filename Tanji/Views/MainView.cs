using System.Windows.Forms;

using Tanji.Properties;

namespace Tanji.Views;

public partial class MainView : Form
{
    public MainView()
    {
        InitializeComponent();
        Icon = Resources.Tanji_256;
    }
}
