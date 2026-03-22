using System.Windows.Forms;
using System.Collections.Specialized;

using Tanji.Properties;
using Tanji.Core.Infrastructure.Services;

using WindowsFormsLifetime;

namespace Tanji.Views;

public partial class MainView : Form
{
    private readonly PacketLoggerView _packetLoggerView;

    private readonly IGuiContext _guiContext;
    private readonly IFormProvider _formProvider;

    public MainView(IGuiContext guiContext, IFormProvider formProvider,
        IConnectionHandlerService connectionHandler)
    {
        _guiContext = guiContext;
        _formProvider = formProvider;
        _packetLoggerView = _formProvider.GetForm<PacketLoggerView>();

        InitializeComponent();

        Icon = Resources.Tanji_256;
        connectionHandler.Connections.CollectionChanged += Connections_CollectionChanged;
    }

    private void Connections_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems?.Count > 0)
        {
            _guiContext.Invoke(_packetLoggerView.Show);
        }
        else _packetLoggerView.Hide();
    }
}