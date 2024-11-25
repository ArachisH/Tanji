using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Infrastructure.ViewModels;

public partial class LoggerViewModel : ObservableObject, IMiddleman
{
    private readonly ILogger<LoggerViewModel> _logger;

    public bool IsHandlingInbound { get; }
    public bool IsHandlingOutbound { get; }

    public LoggerViewModel(ILogger<LoggerViewModel> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        return false;
    }
    public async ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        return false;
    }
}