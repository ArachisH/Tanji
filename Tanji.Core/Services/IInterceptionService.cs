using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Services;

public interface IInterceptionService
{
    public bool IsInterceptingWebTraffic { get; }
    public bool IsInterceptingGameTraffic { get; }

    public ValueTask<string> InterceptGameTicketAsync(CancellationToken cancellationToken = default);
    public Task LaunchInterceptableClientAsync(string ticket, HConnection connection, HPlatform platform, CancellationToken cancellationToken = default);
}