namespace Tanji.Core.Services;

public class PacketMiddlemanService : IPacketMiddlemanService
{
    public ValueTask<bool> PacketInboundAsync(Memory<byte> buffer)
    {
        throw new NotImplementedException();
    }
    public ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer)
    {
        throw new NotImplementedException();
    }
}