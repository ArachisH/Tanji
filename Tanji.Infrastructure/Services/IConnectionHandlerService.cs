using System.Collections.ObjectModel;

using Tanji.Core.Network;
using Tanji.Core.Habbo.Network;

namespace Tanji.Infrastructure.Services;

public interface IConnectionHandlerService
{
    ObservableCollection<IHConnection> Connections { get; }

    Task<IHConnection> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default);
}