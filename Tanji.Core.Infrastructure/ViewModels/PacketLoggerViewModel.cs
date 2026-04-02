using System.Collections.Specialized;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Net.Interception;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.Configuration;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class PacketLoggerViewModel : ObservableObject
{
    private readonly Timer _packetsPerSecondTimer;

    private readonly ILogger<PacketLoggerViewModel> _logger;
    private readonly IPacketLogHandlerService _packetLogHandler;
    private readonly IConnectionHandlerService _connectionHandler;

    private int _outgoingWithinSecond, _incomingWithinSecond;

    [ObservableProperty]
    private int _incomingPerSecond;

    [ObservableProperty]
    private int _outgoingPerSecond;

    [ObservableProperty]
    private PacketLoggingOptions _packetLoggingOptions;

    public PacketLoggerViewModel(ILogger<PacketLoggerViewModel> logger, IOptions<TanjiOptions> options,
        IPacketLogHandlerService packetLogHandler,
        IConnectionHandlerService connectionHandler)
    {
        _logger = logger;

        _connectionHandler = connectionHandler;
        _connectionHandler.Connections.CollectionChanged += Connections_CollectionChanged;

        _packetLogHandler = packetLogHandler;
        _packetLoggingOptions = options.Value.PacketLoggingOptions;

        _packetsPerSecondTimer = new Timer(UpdatePacketsPerSecond);
    }

    private void UpdatePacketsPerSecond(object? state)
    {
        IncomingPerSecond = _incomingWithinSecond;
        _incomingWithinSecond -= IncomingPerSecond;

        OutgoingPerSecond = _outgoingWithinSecond;
        _outgoingWithinSecond -= OutgoingPerSecond;
    }

    private void Connections_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            foreach (HConnection connection in e.NewItems!)
            {
                connection.PacketIncomingAsync += Connection_PacketInterceptedAsync;
                connection.PacketOutgoingAsync += Connection_PacketInterceptedAsync;
            }
            _packetsPerSecondTimer.Change(0, 1000);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            foreach (HConnection connection in e.OldItems!)
            {
                connection.PacketIncomingAsync -= Connection_PacketInterceptedAsync;
                connection.PacketOutgoingAsync -= Connection_PacketInterceptedAsync;
            }
            if (e.NewItems!.Count < 1)
            {
                _packetsPerSecondTimer.Change(Timeout.Infinite, 1000);
            }
        }
    }
    private ValueTask Connection_PacketInterceptedAsync(HConnection connection, PacketInterceptedEventArgs e)
    {
        if (e.IsOutgoing)
        {
            _outgoingWithinSecond++;
        }
        else _incomingWithinSecond++;

        if (e.IsOutgoing && !PacketLoggingOptions.IsLoggingOutgoing) return ValueTask.CompletedTask;
        if (!e.IsOutgoing && !PacketLoggingOptions.IsLoggingIncoming) return ValueTask.CompletedTask;

        _ = _packetLogHandler.WritePacketLog(e.PacketBuffer.Span, e.IsOutgoing, e.PacketFormat, connection.Context.Revision);

        return ValueTask.CompletedTask;
    }
}