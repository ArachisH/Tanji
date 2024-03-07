using Tanji.Core.Services;
using Tanji.Core.Habbo.Network;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.ViewModels;

public partial class ConnectionViewModel : ObservableObject
{
    private readonly IWebInterceptionService _interceptions;

    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private HotelEndPoint _hotelServer;

    [ObservableProperty]
    private string _customClientPath;

    public ConnectionViewModel(IWebInterceptionService interception)
    {
        _interceptions = interception;
    }
}