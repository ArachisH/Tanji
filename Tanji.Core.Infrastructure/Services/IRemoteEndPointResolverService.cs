using System.Net;

using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

namespace Tanji.Core.Infrastructure.Services;

public interface IRemoteEndPointResolverService<TEndPoint> where TEndPoint : IPEndPoint
{
    Task<TEndPoint> ResolveAsync(string ticket, CancellationToken cancellationToken = default);
    Task<TEndPoint> ResolveAsync(HNode local, HConnectionContext context, CancellationToken cancellationToken = default);
}