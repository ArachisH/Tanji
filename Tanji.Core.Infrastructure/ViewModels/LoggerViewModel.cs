using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class LoggerViewModel : ObservableObject
{
    private readonly ILogger<LoggerViewModel> _logger;

    public LoggerViewModel(ILogger<LoggerViewModel> logger)
    {
        _logger = logger;
    }
}