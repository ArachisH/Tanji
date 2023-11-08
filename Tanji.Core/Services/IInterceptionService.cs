using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Services;

public interface IInterceptionService
{
    public bool IsInterceptingWebTraffic { get; }
    public bool IsInterceptingGameTraffic { get; }

    public ValueTask<string> InterceptGameTicketAsync(CancellationToken cancellationToken = default);
    public Task<HConnection> LaunchInterceptableClientAsync(string ticket, HPlatform platform, string? clientPath = null, CancellationToken cancellationToken = default);
}