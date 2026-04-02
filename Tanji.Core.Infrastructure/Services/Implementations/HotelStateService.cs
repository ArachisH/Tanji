using System.Collections.Specialized;

using Microsoft.Extensions.Logging;

using Tanji.Core.Net.Interception;
using Tanji.Core.Cryptography.Ciphers;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class HotelStateService : IHotelStateService
{
    private readonly ILogger<HotelStateService> _logger;
    private readonly IConnectionHandlerService _connectionHandler;

    public HotelStateService(ILogger<HotelStateService> logger,
        IConnectionHandlerService connectionHandler)
    {
        _logger = logger;
        _connectionHandler = connectionHandler;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _connectionHandler.Connections.CollectionChanged -= Connections_CollectionChanged;
        return Task.CompletedTask;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _connectionHandler.Connections.CollectionChanged += Connections_CollectionChanged;
        return Task.CompletedTask;
    }

    private void Connections_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems == null && e.OldItems == null) return;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
            {
                foreach (HConnection connection in e.NewItems!)
                {
                    connection.PacketOutgoingAsync += Outgoing_RetrieveSharedKeyAsync;
                }
                break;
            }
            case NotifyCollectionChangedAction.Remove:
            {
                foreach (HConnection connection in e.OldItems!)
                {
                    connection.PacketOutgoingAsync -= Outgoing_RetrieveSharedKeyAsync;
                }
                break;
            }
            default: break;
        }
    }

    private ValueTask Outgoing_RetrieveSharedKeyAsync(HConnection connection, PacketInterceptedEventArgs e)
    {
        ReadOnlySpan<byte> packetBufferSpan = e.PacketBuffer.Span;
        e.PacketFormat.TryReadId(packetBufferSpan, out short id, out int bytesRead);

        if (id == 4002) // TODO: Use GamePatchingOptions to check correct packet id.
        {
            e.PacketFormat.TryReadUTF8(packetBufferSpan.Slice(e.PacketFormat.MinBufferSize), out string sharedKeyHex, out _);

            if (sharedKeyHex.Length % 2 != 0)
            {
                sharedKeyHex = "0" + sharedKeyHex;
            }
            _logger.LogDebug("Retrieved shared key: {key}", sharedKeyHex);

            byte[] sharedKeyBytes = Convert.FromHexString(sharedKeyHex);
            connection.Remote.EncryptCipher = new RC4(sharedKeyBytes);

            // Unsubscribe, this is only done during the handshake phase.
            e.Cancel = true;
            connection.PacketOutgoingAsync -= Outgoing_RetrieveSharedKeyAsync;
        }
        return ValueTask.CompletedTask;
    }
}