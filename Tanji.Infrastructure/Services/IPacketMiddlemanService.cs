using Tanji.Core.Habbo.Network;

namespace Tanji.Infrastructure.Services;

public interface IPacketMiddlemanService
{
    ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode destination);
    ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode destination);
}