using Tanji.Core.Net;
using Tanji.Core.Canvas;

namespace Tanji.Infrastructure.Factories.Implementations;

public sealed class ConnectionFactory : IConnectionFactory
{
    private readonly IServiceProvider _services;

    public ConnectionFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IHConnection Create(HConnectionContext context)
    {
        return context.Platform switch
        {
            HPlatform.Flash => new HConnection(context),
            _ => throw new NotSupportedException($"{context.Platform} is currently not a supported platform.")
        };
    }
}