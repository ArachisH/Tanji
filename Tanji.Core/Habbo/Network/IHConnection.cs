using System.Net;

using Tanji.Core.Network;

namespace Tanji.Core.Habbo.Network;

public interface IHConnection
{
    HNode? Local { get; }
    HNode? Remote { get; }

    bool IsConnected { get; }
    int TotalInboundPackets { get; }
    int TotalOutboundPackets { get; }

    HConnectionContext Context { get; }

    Task AttachNodesAsync(CancellationToken cancellationToken = default);
    Task InterceptLocalConnectionAsync(CancellationToken cancellationToken = default);
    Task EstablishRemoteConnectionAsync(IPEndPoint remoteEndPoint, CancellationToken cancellationToken = default);
}