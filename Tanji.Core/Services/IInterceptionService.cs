namespace Tanji.Core.Services;

public interface IInterceptionService
{
    public bool IsInterceptingWebTraffic { get; }
    public bool IsInterceptingGameTraffic { get; }

    public ValueTask<string> InterceptGameTokenAsync();
}