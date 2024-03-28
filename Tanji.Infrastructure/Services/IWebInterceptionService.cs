namespace Tanji.Infrastructure.Services;

public interface IWebInterceptionService
{
    public bool IsIntercepting { get; }

    public void StopWebInterception();
    public void StartWebInterception();

    public ValueTask<string> InterceptTicketAsync(CancellationToken cancellationToken = default);
}