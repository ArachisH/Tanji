using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

namespace Tanji.Core.Infrastructure.Factories.Implementations;

public sealed class ConnectionFactory : IConnectionFactory
{
    private readonly IPacketDistributionService _packetHandler;

    public ConnectionFactory(IPacketDistributionService packetHandler)
    {
        _packetHandler = packetHandler;
    }

    public HConnection Create(HNode local, HNode remote, HConnectionContext context)
    {
        return new HConnection(local, remote, _packetHandler, context);
    }
}