using System.Diagnostics;
using System.Threading.Channels;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Habbo;
using Tanji.Core.Network;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;
using Tanji.Core.Habbo.Canvas.Flash;

using Eavesdrop;

using Flazzy.Tools;

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private readonly TanjiOptions _options;
    private readonly DirectoryInfo _cacheDirectory;
    private readonly Channel<string> _gameTicketsChannel;
    private readonly ILogger<InterceptionService> _logger;

    private readonly Dictionary<string, HConnection> _connections;
    private IReadOnlyDictionary<string, HConnection> Connections { get; init; }

    private static ReadOnlySpan<char> TicketVariableName => "\"ticket\":\"";

    public bool IsInterceptingWebTraffic => Eavesdropper.IsRunning;
    public bool IsInterceptingGameTraffic => false;

    public InterceptionService(ILogger<InterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        _cacheDirectory = Directory.CreateDirectory("Cache");
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
    public ValueTask<HConnection> LaunchInterceptableClientAsync(string ticket, HPlatform platform, string? clientPath = null, CancellationToken cancellationToken = default)
    {
        FileInfo? clientFileInfo = null;
        if (!string.IsNullOrWhiteSpace(clientPath) && File.Exists(clientPath))
        {
            clientFileInfo = new FileInfo(clientPath);
        }

        PlatformPaths paths = default;
        if (clientFileInfo == null)
        {
            if (_options.PlatformPaths == null)
            {
                throw new ArgumentNullException(nameof(clientPath), "No client/executable path available for fallback, provide a valid client file path.");
            }

            if (!_options.PlatformPaths.TryGetValue(platform, out paths))
            {
                throw new ArgumentException("No file paths available for the specified platform.", nameof(platform));
            }

            clientFileInfo = new FileInfo(paths.ClientPath);
        }

        if (!clientFileInfo.Exists)
        {
            ThrowHelper.ThrowFileNotFoundException("The provided client file path does not exist.", clientFileInfo.FullName);
        }

        Span<byte> hash = stackalloc byte[16];
        using FileStream clientFs = clientFileInfo.OpenRead();

        MD5.HashData(clientFs, hash);
        hash = hash.Slice(0, 4);

        CachedGame? cachedGame = null;
        string uniqueName = Convert.ToHexString(hash);
        foreach (var fileInfo in _cacheDirectory.EnumerateFileSystemInfos())
        {
            // {REVISION}_{32BITMD5HEAD}.json
            if (!fileInfo.Name.EndsWith($"{uniqueName}.json")) continue;
            cachedGame = new CachedGame(fileInfo.FullName);
        }

        if (cachedGame == null)
        {
            switch (platform)
            {
                case HPlatform.Flash:
                {
                    using var flashClientBuffer = MemoryOwner<byte>.Allocate((int)clientFileInfo.Length);
                    Span<byte> flashClientSpan = flashClientBuffer.Span;

                    using FileStream flashClientFs = clientFileInfo.OpenRead();
                    flashClientFs.Read(flashClientSpan);

                    // Attempt to decrypt the flash file.
                    int decryptedLength = DecryptFlashClient(ref flashClientSpan, out int writtenOffset);

                    // Either re-use the same initial buffer, or slice out the decrypted data.
                    using MemoryOwner<byte> decryptedFlashClientBuffer = decryptedLength > 0 ?
                        flashClientBuffer.Slice(writtenOffset, decryptedLength) : flashClientBuffer;

                    // We can dispose these instances, since we're only using them to extract data into a more lightweight type.
                    using var flashGame = new FlashGame(clientFileInfo.FullName, decryptedFlashClientBuffer.AsStream());

                    cachedGame = CacheModifiedClient(flashGame, in paths);
                    break;
                }

                default:
                case HPlatform.Unity:
                case HPlatform.HTML5:
                case HPlatform.Unknown:
                case HPlatform.Shockwave: break;
            }
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
    private CachedGame CacheModifiedClient(HGame game, in PlatformPaths paths)
    {
        throw new NotSupportedException();
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
    private static int DecryptFlashClient(ref Span<byte> flashClientBuffer, out int writtenOffset)
    {
        writtenOffset = 0;
        return flashClientBuffer[0] <= 'Z'
            ? flashClientBuffer.Length
            : FlashCrypto.Decrypt(ref flashClientBuffer, out writtenOffset);
    }
}