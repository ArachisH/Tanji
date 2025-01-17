using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

namespace Tanji.Core.Infrastructure.Factories;

public interface IConnectionFactory
{
    HConnection Create(HNode local, HNode remote, HConnectionContext context);
}