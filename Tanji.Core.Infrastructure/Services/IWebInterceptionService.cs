namespace Tanji.Core.Infrastructure.Services;

public interface IWebInterceptionService
{
    public bool IsIntercepting { get; }

    public void Stop();
    public void Start();

    public ValueTask<string> InterceptTicketAsync(CancellationToken cancellationToken = default);
}