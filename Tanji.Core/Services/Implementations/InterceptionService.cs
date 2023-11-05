using System.Threading.Channels;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Eavesdrop;

using Tanji.Core.Habbo;
using Tanji.Core.Configuration;

namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private static ReadOnlySpan<char> TicketVariableName => "\"ticket\":\"";

    private readonly Channel<string> _gameTicketsChannel;

    private readonly TanjiOptions _options;
    private readonly ILogger<InterceptionService> _logger;

    public bool IsInterceptingWebTraffic => Eavesdropper.IsRunning;
    public bool IsInterceptingGameTraffic => false;

    public InterceptionService(ILogger<InterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _gameTicketsChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = true
        });

        _logger.LogInformation("InterceptionService ctor");

        Eavesdropper.Targets.AddRange(_options.ProxyOverrides);
        Eavesdropper.RequestInterceptedAsync += WebRequestInterceptedAsync;
        Eavesdropper.ResponseInterceptedAsync += WebResponseInterceptedAsync;
        Eavesdropper.Certifier = new Certifier("Tanji", "Tanji Root Certificate Authority");
    }

    public ValueTask<string> InterceptGameTicketAsync()
    {
        TryStartWebTrafficInterception();
        return _gameTicketsChannel.Reader.ReadAsync();
    }

    private Task WebRequestInterceptedAsync(object? sender, RequestInterceptedEventArgs e)
    {
        return Task.CompletedTask;
    }
    private async Task WebResponseInterceptedAsync(object? sender, ResponseInterceptedEventArgs e)
    {
        if (e.Uri?.AbsolutePath == "/api/client/clientnative/url")
        {
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
                await _gameTicketsChannel.Writer.WriteAsync(ticket).ConfigureAwait(false);
            }
            else _logger.LogDebug("Failed to extract ticket: {Body}", body);
        }
    }


    private bool TryStartWebTrafficInterception()
    {
        if (Eavesdropper.IsRunning) return true;
        if (!Eavesdropper.Certifier.CreateTrustedRootCertificate())
        {
            _logger.LogWarning("User declined root certificate installation");
            return false;
        }

        Eavesdropper.Initiate(_options.ProxyListenPort);
        return true;
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
}