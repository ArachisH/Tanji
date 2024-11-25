namespace Tanji.Core.Net.Interception;

public interface IMiddleman
{
    bool IsHandlingInbound { get; }
    bool IsHandlingOutbound { get; }

    ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination);
    ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination);
}