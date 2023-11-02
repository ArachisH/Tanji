using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Eavesdrop;

using Tanji.Core.Configuration;

namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private readonly Channel<string> _gameTokensChannel;

    private readonly TanjiOptions _options;
    private readonly ILogger<InterceptionService> _logger;

    public bool IsInterceptingWebTraffic => Eavesdropper.IsRunning;
    public bool IsInterceptingGameTraffic => false; // TODO

    public InterceptionService(ILogger<InterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        _logger.LogInformation("InterceptionService ctor");

        _gameTokensChannel = Channel.CreateUnbounded<string>();

        Eavesdropper.Targets.AddRange(_options.ProxyOverrides);
        Eavesdropper.RequestInterceptedAsync += WebRequestInterceptedAsync;
        Eavesdropper.ResponseInterceptedAsync += WebResponseInterceptedAsync;
        Eavesdropper.Certifier = new Certifier("Tanji", "Tanji Root Certificate Authority");
    }

    private Task WebRequestInterceptedAsync(object? sender, RequestInterceptedEventArgs e)
    {
        _logger.LogDebug(e.Uri.AbsoluteUri);
        return Task.CompletedTask;
    }

    private Task WebResponseInterceptedAsync(object? sender, ResponseInterceptedEventArgs e)
    {
        // Testing for now...
        // TODO: Bring over updated v1.5 interception logic
        _gameTokensChannel.Writer.WriteAsync(e.Uri.AbsoluteUri);

        _logger.LogDebug(e.Uri.AbsoluteUri);
        return Task.CompletedTask;
    }

    public ValueTask<string> InterceptGameTokenAsync()
    {
        StartWebTrafficInterception();
        return _gameTokensChannel.Reader.ReadAsync();
    }

    private void StartWebTrafficInterception()
    {
        if (Eavesdropper.IsRunning) return;
        if (!Eavesdropper.Certifier.CreateTrustedRootCertificate())
        {
            _logger.LogWarning("User declined root certificate installation");
            return;
        }

        Eavesdropper.Initiate(_options.ProxyListenPort);
    }
}