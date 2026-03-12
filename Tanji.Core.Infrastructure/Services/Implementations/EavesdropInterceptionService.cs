using System.Threading.Channels;

using Tanji.Core.Canvas;
using Tanji.Core.Infrastructure.Configuration;

using Eavesdrop;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using CommunityToolkit.HighPerformance;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class EavesdropInterceptionService : IWebInterceptionService
{
    private readonly Channel<string> _ticketsChannel;

    private static ReadOnlySpan<char> TicketVariableName => "\"ticket\":\"";

    private readonly TanjiOptions _options;
    private readonly ILogger<EavesdropInterceptionService> _logger;

    public bool IsIntercepting => Eavesdropper.IsRunning;

    public EavesdropInterceptionService(ILogger<EavesdropInterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _ticketsChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = true
        });

        Eavesdropper.Proxy = _options.HttpSystemProxy.GetProxy();
        Eavesdropper.Targets.AddRange(_options.ProxyOverrides);

        Eavesdropper.RequestInterceptedAsync += WebRequestInterceptedAsync;
        Eavesdropper.ResponseInterceptedAsync += WebResponseInterceptedAsync;
        Eavesdropper.Certifier = new Certifier("Tanji", "Tanji Root Certificate Authority");
    }

    public void Start()
    {
        if (Eavesdropper.IsRunning) return;
        if (Eavesdropper.Certifier == null)
        {
            _logger.LogCritical("Eavesdropper Certifier instance is null.");
            throw new NullReferenceException("Eavesdropper Certifier instance is null.");
        }

        if (Eavesdropper.Certifier.CreateTrustedRootCertificate())
        {
            _logger.LogInformation("Initiating system wide HTTP/S interceptor on port: {ProxyListenPort}", _options.ProxyListenPort);
            Eavesdropper.Initiate(_options.ProxyListenPort);
        }
        else
        {
            if (!Eavesdropper.IsOnlyInterceptingHttp)
            {
                _logger.LogCritical("User declined to trust self-signed certificate, and will therefore be unable to intercepted HTTPS traffic from the system.");
                throw new Exception("User declined to trust self-signed certificate, and will therefore be unable to intercepted HTTPS traffic from the system.");
            }
            else _logger.LogInformation("User declined to add self-signed certificate as authority to trusted store on local machine.");
        }
    }
    public void Stop() => Eavesdropper.Terminate();

    public ValueTask<string> InterceptTicketAsync(CancellationToken cancellationToken = default)
    {
        Start();
        return _ticketsChannel.Reader.ReadAsync(cancellationToken);
    }

    private Task WebRequestInterceptedAsync(object? sender, RequestInterceptedEventArgs e)
    {
        return Task.CompletedTask;
    }
    private async Task WebResponseInterceptedAsync(object? sender, ResponseInterceptedEventArgs e)
    {
        if (e.Uri?.AbsolutePath == "/api/client/clientnative/url")
        {
            await HandleTicketScrapingAsync(e.Response, e.Uri).ConfigureAwait(false);
        }
    }

    private async Task HandleTicketScrapingAsync(HttpResponseMessage message, Uri requestUri)
    {
        if (requestUri.DnsSafeHost.AsSpan().ToHotel() == HHotel.Unknown)
        {
            _logger.LogError("Failed to determine HHotel object type from '{Host}'.", requestUri.DnsSafeHost);
            return;
        }

        if (!message.IsSuccessStatusCode)
        {
            _logger.LogError("Status Code: {Code}", message.StatusCode);
            return;
        }

        string body = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (TryExtractTicket(body, out string? ticket) && !string.IsNullOrWhiteSpace(ticket))
        {
            _logger.LogDebug("Ticket Extracted: {ticket}", ticket);
            await _ticketsChannel.Writer.WriteAsync(ticket).ConfigureAwait(false);
        }
        else _logger.LogError("Failed to extract ticket: {Body}", body);
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