using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Net.Interception;

public sealed class HConnection : IDisposable
{
    private Task? _bridgeNodesTask;

    public delegate ValueTask AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);

    public event AsyncEventHandler<PacketInterceptedEventArgs>? PacketIncomingAsync;
    public event AsyncEventHandler<PacketInterceptedEventArgs>? PacketOutgoingAsync;

    public HNode Local { get; }
    public HNode Remote { get; }
    public HConnectionContext Context { get; }

    public bool IsDisposed => Local.IsDisposed || Remote.IsDisposed;
    public bool IsConnected => Local.IsConnected && Remote.IsConnected;

    public HConnection(HNode local, HNode remote, HConnectionContext context)
    {
        Local = local;
        Remote = remote;
        Context = context;
    }

    public Task BridgeNodesAsync(CancellationToken cancellationToken = default)
    {
        if (_bridgeNodesTask != null && !_bridgeNodesTask.IsCompleted)
        {
            return _bridgeNodesTask;
        }

        Task localToRemote = BridgeNodesAsync(Local, Remote, true, cancellationToken);
        Task remoteToLocal = BridgeNodesAsync(Remote, Local, false, cancellationToken);
        return _bridgeNodesTask = Task.WhenAll(localToRemote, remoteToLocal);
    }

    public void Dispose() => Disconnect();
    public void Disconnect()
    {
        if (!Local.IsDisposed) Local.Dispose();
        if (!Remote.IsDisposed) Remote.Dispose();
    }

    private async Task BridgeNodesAsync(HNode source, HNode destination, bool isOutgoing, CancellationToken cancellationToken)
    {
        while (source.IsConnected && destination.IsConnected && !cancellationToken.IsCancellationRequested)
        {
            var packetBufferWriter = new ArrayPoolBufferWriter<byte>(source.PacketFormat.MinBufferSize);
            int received = await source.ReceivePacketAsync(packetBufferWriter, cancellationToken).ConfigureAwait(false);
            if (received > 0)
            {
                _ = HandleInterceptedPacketAsync(destination, isOutgoing, packetBufferWriter, cancellationToken);
            }
        }
    }
    private async Task HandleInterceptedPacketAsync(HNode destination, bool isOutgoing, ArrayPoolBufferWriter<byte> packetBufferWriter, CancellationToken cancellationToken)
    {
        try
        {
            AsyncEventHandler<PacketInterceptedEventArgs>? handler = isOutgoing ? PacketOutgoingAsync : PacketIncomingAsync;
            Memory<byte> mutablePacketBuffer = packetBufferWriter.DangerousGetArray();

            bool isReadOnlyBuffer = false;
            if (handler != null)
            {
                var args = new PacketInterceptedEventArgs(mutablePacketBuffer, destination.PacketFormat, isOutgoing)
                {
                    Cancel = cancellationToken.IsCancellationRequested
                };
                await handler.Invoke(this, args).ConfigureAwait(false);

                // Packet was replaced as read-only.
                if (isReadOnlyBuffer = args.IsReadOnly)
                {
                    await destination.SendPacketAsync(args.PacketBuffer, cancellationToken).ConfigureAwait(false);
                }
                else if (args.IsReplaced)
                {
                    mutablePacketBuffer = args.GetMutablePacketBuffer();
                }
            }

            if (!isReadOnlyBuffer)
            {
                await destination.SendPacketAsync(mutablePacketBuffer, cancellationToken).ConfigureAwait(false);
            }
        }
        finally { packetBufferWriter.Dispose(); }
    }
}