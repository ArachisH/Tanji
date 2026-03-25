using System;
using System.Windows.Forms;

using Tanji.Properties;
using Tanji.Core.Infrastructure.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Views;

public partial class PacketLoggerView : Form
{
    public PacketLoggerView()
    {
        InitializeComponent();
        Icon = Resources.Tanji_256;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (Program.Services is not null)
        {
            DataContext = Program.Services.GetRequiredService<PacketLoggerViewModel>();
        }
        base.OnLoad(e);
    }
    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is ObservableObject observable)
        {
            viewModelSrc.DataSource = observable;
        }
        base.OnDataContextChanged(e);
    }
}