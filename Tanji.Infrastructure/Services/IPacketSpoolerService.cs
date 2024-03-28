using Tanji.Core.Net;

namespace Tanji.Infrastructure.Services;

public interface IPacketSpoolerService
{
    ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination);
    ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination);
}