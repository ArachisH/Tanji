using CommunityToolkit.Mvvm.ComponentModel;

using Sulakore.Communication;

using Tanji.Core.Services;

namespace Tanji.Core.ViewModels;

public partial class ConnectionViewModel : ObservableObject
{
    private readonly IInterceptionService _interceptions;

    [ObservableProperty]
    private string _status;

    [ObservableProperty]
    private HotelEndPoint _hotelServer;

    [ObservableProperty]
    private string _customClientPath;

    public ConnectionViewModel(IInterceptionService interception)
    {
        _interceptions = interception;
    }
}