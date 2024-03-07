using System.Collections.ObjectModel;

using Tanji.Core.Network;

namespace Tanji.Core.Services;

public interface IConnectionHandlerService
{
    ObservableCollection<HConnection> Connections { get; }

    Task<HConnection> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext options, CancellationToken cancellationToken = default);
}