using Tanji.Core.Net;
using Tanji.Core.Net.Interception;
using Tanji.Infrastructure.Services;

namespace Tanji.Infrastructure.Factories.Implementations;

public sealed class ConnectionFactory : IConnectionFactory
{
    private readonly IPacketMiddlemanService _middleman;

    public ConnectionFactory(IPacketMiddlemanService middleman)
    {
        _middleman = middleman;
    }

    public HConnection Create(HNode local, HNode remote, HConnectionContext context)
    {
        return new HConnection(local, remote, _middleman, context);
    }
}