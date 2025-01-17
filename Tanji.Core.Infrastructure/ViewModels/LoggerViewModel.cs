using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.ComponentModel;
using Tanji.Core.Net.Buffers;
using CommunityToolkit.Mvvm.Input;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class LoggerViewModel : ObservableObject, IMiddleman
{
    private readonly ILogger<LoggerViewModel> _logger;

    public bool IsHandlingInbound { get; } = true;
    public bool IsHandlingOutbound { get; }

    [ObservableProperty]
    private string _status = "Testing";

    public LoggerViewModel(ILogger<LoggerViewModel> logger)
    {
        _logger = logger;
    }

    [RelayCommand]
    public void asd()
    {
        Status = "Pressed button";
    }

    public ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        //var pktReader = new HPacketReader(source.ReceivePacketFormat, buffer.Span);
        //lock (_consoleWriteLock)
        //{
        //    _logger.LogTrace("[Inbound({Id}, {Length})] < {Buffer}\r\n--------", pktReader.Id, pktReader.Length, ToString(buffer.Span));
        //}




        Status = Status + "1";
        OnPropertyChanged("Status");


        return new ValueTask<bool>(false);
    }
    public ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        return new ValueTask<bool>(false);
    }
}