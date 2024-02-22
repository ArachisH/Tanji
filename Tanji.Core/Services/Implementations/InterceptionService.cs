using System.Diagnostics;
using System.Threading.Channels;
using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Json;
using Tanji.Core.Habbo;
using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;

using Eavesdrop;

using CommunityToolkit.HighPerformance;

namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private readonly Channel<string> _gameTicketsChannel;

    private static ReadOnlySpan<char> TicketVariableName => "\"ticket\":\"";

    // Dependency Injected Fields
    private readonly TanjiOptions _options;
    private readonly ILogger<InterceptionService> _logger;
    private readonly IFileCachingService<PlatformPaths, CachedGame> _caching;

    private readonly Dictionary<string, HConnection> _connections;
    private IReadOnlyDictionary<string, HConnection> Connections { get; init; }

    public bool IsInterceptingWebTraffic => Eavesdropper.IsRunning;
    public bool IsInterceptingGameTraffic => false;

    public InterceptionService(ILogger<InterceptionService> logger, IOptions<TanjiOptions> options,
        IFileCachingService<PlatformPaths, CachedGame> caching)
    {
        _logger = logger;
        _caching = caching;
        _options = options.Value;
        _connections = new Dictionary<string, HConnection>(8);
        _gameTicketsChannel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = true
        });

        Connections = new ReadOnlyDictionary<string, HConnection>(_connections);

        Eavesdropper.Targets.AddRange(_options.ProxyOverrides);
        Eavesdropper.RequestInterceptedAsync += WebRequestInterceptedAsync;
        Eavesdropper.ResponseInterceptedAsync += WebResponseInterceptedAsync;
        Eavesdropper.Certifier = new Certifier("Tanji", "Tanji Root Certificate Authority");
    }

    public ValueTask<string> InterceptGameTicketAsync(CancellationToken cancellationToken = default)
    {
        TryStartWebTrafficInterception();
        return _gameTicketsChannel.Reader.ReadAsync(cancellationToken);
    }
    public ValueTask<HConnection> LaunchInterceptableClientAsync(string ticket, HPlatform platform, CancellationToken cancellationToken = default)
    {
        if (_options.PlatformPaths == null || _options.PlatformPaths.Count == 0)
        {
            throw new Exception("No associated paths for any platform available.");
        }

        if (!_options.PlatformPaths.TryGetValue(platform, out PlatformPaths paths))
        {
            ThrowHelper.ThrowArgumentException("The provided platform does not have any paths associated with it.", nameof(platform));
        }

        CachedGame game = _caching.GetOrAdd(paths);

        // We can create hard links without admin, but not symbolic links ??
        _logger.LogInformation("Creating hard link to patched client");
        string linkPath = Path.Combine(paths.RootPath, $"patched.{PlatformConverter.ToClientName(paths.Platform)}");

        File.Delete(linkPath);
        if (!NativeMethods.CreateHardLink(linkPath, game.ClientPath, IntPtr.Zero))
        {
            _logger.LogError("Failed to create a hard link at the provided {linkPath}.", linkPath);
        }

        // TODO: Populate with relevant game client information.
        var connection = new HConnection() { };

        // TODO: Begin packet interception, then return the task.
        Process.Start(paths.ExecutablePath, $"server {ticket[..4]} ticket {ticket[5..]}");

        return ValueTask.FromResult(connection);
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
        if (!Eavesdropper.Certifier?.CreateTrustedRootCertificate() ?? false)
        {
            _logger.LogWarning("User declined root certificate installation.");
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
    private static void ToggleLauncherSettings(PlatformPaths paths, bool isPointingToPatchedClient)
    {
        // TODO: Check if any Unity game launcher settings need to be processed prior to launcher the client.
        if (paths.Platform != HPlatform.Flash) return;

        string applicationXMLPath = Path.Combine(paths.RootPath, "META-INF\\AIR\\application.xml");

        var habboAirSettings = new XmlDocument();
        habboAirSettings.Load(applicationXMLPath);

        XmlElement? idElement = habboAirSettings.DocumentElement?["id"];
        if (idElement == null)
        {
            ThrowHelper.ThrowNullReferenceException("The 'id' element does not exist in the application's XML configuration file.");
        }

        idElement.InnerText = idElement.InnerText.StartsWith("TNJ") ? idElement.InnerText[4..] : ("TNJ." + idElement.InnerText);
        XmlElement? contentElement = habboAirSettings["application"]?["initialWindow"]?["content"];
        if (contentElement == null)
        {
            ThrowHelper.ThrowNullReferenceException("The 'application.initialWindow.content' element does not exist in the application's XML configuration file.");
        }

        string fileName = isPointingToPatchedClient ? "patched." : "";
        fileName += PlatformConverter.ToClientName(paths.Platform);

        contentElement.InnerText = fileName;
        habboAirSettings.Save(applicationXMLPath);
    }
}