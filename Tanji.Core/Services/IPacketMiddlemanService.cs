namespace Tanji.Core.Services;

public interface IPacketMiddlemanService
{
    ValueTask<bool> PacketInboundAsync(Memory<byte> buffer);
    ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer);
}