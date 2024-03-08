using System.Collections.ObjectModel;

using Tanji.Core.Network;

namespace Tanji.Core.Services;

public interface IConnectionHandlerService<TPacketMiddleman> where TPacketMiddleman : IPacketMiddlemanService
{
    ObservableCollection<HConnection<TPacketMiddleman>> Connections { get; }

    Task<HConnection<TPacketMiddleman>> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext options, CancellationToken cancellationToken = default);
}