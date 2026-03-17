using System.Collections.ObjectModel;

using Tanji.Core.Net.Interception;

namespace Tanji.Core.Infrastructure.Services;

public interface IConnectionHandlerService
{
    ObservableCollection<HConnection> Connections { get; }

    Task<HConnection> InterceptConnectionAsync(string ticket, HConnectionContext context, CancellationToken cancellationToken = default);
}