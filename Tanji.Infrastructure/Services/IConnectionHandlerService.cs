using System.Collections.ObjectModel;

using Tanji.Core.Net;

namespace Tanji.Infrastructure.Services;

public interface IConnectionHandlerService
{
    ObservableCollection<IHConnection> Connections { get; }

    Task<IHConnection> LaunchAndInterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default);
}