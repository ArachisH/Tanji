using Tanji.Core.Net;

namespace Tanji.Infrastructure.Factories;

public interface IConnectionFactory
{
    IHConnection Create(HConnectionContext context);
}