using Tanji.Core.Habbo.Network;

using Microsoft.Extensions.Logging;

namespace Tanji.Infrastructure.Services.Implementations;

public class PacketMiddlemanService : IPacketMiddlemanService
{
    private readonly ILogger<PacketMiddlemanService> _logger;

    public PacketMiddlemanService(ILogger<PacketMiddlemanService> logger)
    {
        _logger = logger;
    }

    public ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode destination)
    {
        throw new NotImplementedException();
    }
    public ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode destination)
    {
        throw new NotImplementedException();
    }
}