using Tanji.Core.Network;
using Tanji.Core.Habbo.Network;

namespace Tanji.Infrastructure.Factories;

public interface IConnectionFactory
{
    IHConnection Create(HConnectionContext context);
}