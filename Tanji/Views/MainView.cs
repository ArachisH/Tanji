using System.Windows.Forms;
using System.Collections.Specialized;

using Tanji.Properties;
using Tanji.Infrastructure.Services;

using WindowsFormsLifetime;

namespace Tanji.Views;

public partial class MainView : Form
{
    private PacketLoggerView? _packetLoggerView;

    private readonly IGuiContext _guiContext;
    private readonly IFormProvider _formProvider;

    public MainView(
        IConnectionHandlerService connectionHandler,
        IGuiContext guiContext,
        IFormProvider formProvider)
    {
        _guiContext = guiContext;
        _formProvider = formProvider;

        InitializeComponent();

        Icon = Resources.Tanji_256;
        connectionHandler.Connections.CollectionChanged += Connections_CollectionChanged;
    }

    private async void Connections_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Do not assume this event is running on UI thread.
        _packetLoggerView ??= await _formProvider.GetFormAsync<PacketLoggerView>().ConfigureAwait(false);

        if (e.NewItems?.Count > 0)
        {
            _guiContext.Invoke(_packetLoggerView.Show);
        }
        else _packetLoggerView.Hide();
    }
}