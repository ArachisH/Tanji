using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class PacketLoggerViewModel : ObservableObject
{
    private readonly ILogger<PacketLoggerViewModel> _logger;

    public PacketLoggerViewModel(ILogger<PacketLoggerViewModel> logger)
    {
        _logger = logger;
    }
}