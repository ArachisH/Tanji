using System.Windows.Forms;

using Tanji.Properties;

namespace Tanji.Views;

public partial class PacketLoggerView : Form
{
    public PacketLoggerView()
    {
        InitializeComponent();
        Icon = Resources.Tanji_256;
    }
}