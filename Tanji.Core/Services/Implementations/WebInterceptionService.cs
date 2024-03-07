using System.Threading.Channels;

using Tanji.Core.Habbo;
using Tanji.Core.Configuration;

using Eavesdrop;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using CommunityToolkit.HighPerformance;

namespace Tanji.Core.Services;

public sealed class WebInterceptionService : IWebInterceptionService
{
    private readonly Channel<string> _ticketsChannel;

    private static ReadOnlySpan<char> TicketVariableName => "\"ticket\":\"";

    private readonly TanjiOptions _options;
    private readonly ILogger<WebInterceptionService> _logger;

    public bool IsIntercepting => Eavesdropper.IsRunning;

    public WebInterceptionService(ILogger<WebInterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _ticketsChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = true
        });

        Eavesdropper.Targets.AddRange(_options.ProxyOverrides);
        Eavesdropper.RequestInterceptedAsync += WebRequestInterceptedAsync;
        Eavesdropper.ResponseInterceptedAsync += WebResponseInterceptedAsync;
        Eavesdropper.Certifier = new Certifier("Tanji", "Tanji Root Certificate Authority");
    }

    public void StopWebInterception() => TryStartWebTrafficInterception(_logger, _options.ProxyListenPort);
    public void StartWebInterception() => Eavesdropper.Terminate();

    public ValueTask<string> InterceptTicketAsync(CancellationToken cancellationToken = default)
    {
        TryStartWebTrafficInterception(_logger, _options.ProxyListenPort);
        return _ticketsChannel.Reader.ReadAsync(cancellationToken);
    }

    private Task WebRequestInterceptedAsync(object? sender, RequestInterceptedEventArgs e)
    {
        return Task.CompletedTask;
    }
    private async Task WebResponseInterceptedAsync(object? sender, ResponseInterceptedEventArgs e)
    {
        if (e.Uri?.AbsolutePath != "/api/client/clientnative/url") return;

        if (e.Uri.DnsSafeHost.AsSpan().ToHotel() == HHotel.Unknown)
        {
            _logger.LogDebug("Failed to determine HHotel object type from '{Host}'.", e.Uri.DnsSafeHost);
            return;
        }

        if (!e.IsSuccessStatusCode)
        {
            _logger.LogDebug("Status Code: {Code}", e.StatusCode);
            return;
        }

        string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (TryExtractTicket(body, out string? ticket) && !string.IsNullOrWhiteSpace(ticket))
        {
            await _ticketsChannel.Writer.WriteAsync(ticket).ConfigureAwait(false);
        }
        else _logger.LogDebug("Failed to extract ticket: {Body}", body);
    }

    private static bool TryExtractTicket(ReadOnlySpan<char> body, out string? ticket)
    {
        ticket = null;

        int ticketStart = body.IndexOf(TicketVariableName);
        if (ticketStart != -1)
        {
            ticketStart += TicketVariableName.Length;
            body = body.Slice(ticketStart);

            ticket = body.Slice(0, body.Length - 2).ToString();
            return true;
        }

        return false;
    }
    private static bool TryStartWebTrafficInterception(ILogger<WebInterceptionService> logger, int proxyListenPort)
    {
        if (Eavesdropper.IsRunning) return true;
        if (!Eavesdropper.Certifier?.CreateTrustedRootCertificate() ?? false)
        {
            logger.LogWarning("User declined root certificate installation.");
            return false;
        }

        Eavesdropper.Initiate(proxyListenPort);
        return true;
    }
}