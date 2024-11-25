using System.Diagnostics.CodeAnalysis;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Net.Interception;

public sealed class HConnection : IDisposable
{
    private Task? _weldTask;

    public int TotalInboundPackets { get; }
    public int TotalOutboundPackets { get; }

    public bool IsDisposed => Local.IsDisposed || Remote.IsDisposed;
    public bool IsConnected => Local.IsConnected && Remote.IsConnected;

    public required HNode Local { get; init; }
    public required HNode Remote { get; init; }
    public required IMiddleman Middleman { get; init; }
    public required HConnectionContext Context { get; init; }

    [SetsRequiredMembers]
    public HConnection(HNode local, HNode remote, IMiddleman middleman, HConnectionContext context)
    {
        Local = local;
        Remote = remote;
        Context = context;
        Middleman = middleman;
    }

    public Task AttachNodesAsync(CancellationToken cancellationToken = default)
    {
        if (_weldTask != null && !_weldTask.IsCompleted)
        {
            return _weldTask;
        }

        Task localToRemote = AttachNodesAsync(Local, Remote, true, Middleman, cancellationToken);
        Task remoteToLocal = AttachNodesAsync(Remote, Local, false, Middleman, cancellationToken);
        return _weldTask = Task.WhenAll(localToRemote, remoteToLocal);
    }

    public void Dispose()
    {
        Disconnect();
    }
    public void Disconnect()
    {
        if (!Local.IsDisposed)
        {
            Local.Dispose();
        }
        if (!Remote.IsDisposed)
        {
            Remote.Dispose();
        }
    }

    private static async Task AttachNodesAsync(HNode source, HNode destination, bool isOutbound, IMiddleman middleman, CancellationToken cancellationToken = default)
    {
        int received;
        while (source.IsConnected && destination.IsConnected && !cancellationToken.IsCancellationRequested)
        {
            // Do not dispose 'bufferWriter' here, instead, dispose of it within the 'TransferPacketAsync' method
            var writer = new ArrayPoolBufferWriter<byte>(source.ReceivePacketFormat.MinBufferSize);
            received = await source.ReceivePacketAsync(writer, cancellationToken).ConfigureAwait(false);

            if (received > 0)
            {
                // Continuously attempt to receive packets from the node
                _ = HandleInterceptedPacketAsync(source, destination, isOutbound, middleman, writer, cancellationToken);
            }
            else writer.Dispose();
        }
    }
    private static async Task HandleInterceptedPacketAsync(HNode source, HNode destination, bool isOutbound, IMiddleman middleman, ArrayPoolBufferWriter<byte> writer, CancellationToken cancellationToken = default)
    {
        try
        {
            // Mutable packet buffer, which will be encrypted if the node has an active cipher instance.
            Memory<byte> buffer = writer.DangerousGetArray();
            if ((middleman.IsHandlingOutbound && isOutbound) || (middleman.IsHandlingInbound && !isOutbound))
            {
                ValueTask<bool> packetProcessTask = !isOutbound
                    ? middleman.PacketInboundAsync(buffer, source, destination)
                    : middleman.PacketOutboundAsync(buffer, source, destination);

                // If true, the packet is to be ignored/blocked
                if (await packetProcessTask.ConfigureAwait(false)) return;
            }
            await destination.SendPacketAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        finally { writer.Dispose(); }
    }
}