using System.Net;
using System.Xml;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Cryptography;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Json;
using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;
using Tanji.Core.Json.Converters;
using Tanji.Core.Habbo.Canvas.Flash;

using Flazzy.Tools;

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Services;

public sealed class ClientHandlerService(ILogger<ClientHandlerService> logger, IOptions<TanjiOptions> options) : IClientHandlerService<CachedGame>
{
    private static readonly JsonSerializerOptions SerializerOptions;

    private readonly TanjiOptions _options = options.Value;
    private readonly ILogger<ClientHandlerService> _logger = logger;

    public DirectoryInfo PatchedClientsDirectory { get; } = Directory.CreateDirectory("Patched Clients");
    public DirectoryInfo MessagesDirectory { get; } = Directory.CreateDirectory("Messages");

    static ClientHandlerService()
    {
        SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        SerializerOptions.Converters.Add(new FormatConverter());
        SerializerOptions.Converters.Add(new PlatformConverter());
        SerializerOptions.Converters.Add(new IPEndPointConverter());
    }

    public CachedGame PatchClient(HPlatform platform, string? clientPath = null)
    {
        if (string.IsNullOrWhiteSpace(clientPath))
        {
            PlatformPaths paths = GetPlatformPaths(platform, _options.PlatformPaths);
            clientPath = paths.ClientPath;
        }

        if (!File.Exists(clientPath))
        {
            _logger.LogError("File does not exist: {filePath}", clientPath);
            ThrowHelper.ThrowFileNotFoundException("The provided file path does not exist.", clientPath);
        }

        // Compute Chopped Hash
        Span<byte> clientFileHash = stackalloc byte[16];
        using FileStream tempClientFileStream = File.OpenRead(clientPath);

        // Attempt to load game data from a json file.
        MD5.HashData(tempClientFileStream, clientFileHash);
        string identifier = Convert.ToHexString(clientFileHash.Slice(0, 4));
        foreach (FileInfo fileInfo in PatchedClientsDirectory.EnumerateFiles())
        {
            if (!fileInfo.Name.StartsWith(identifier, StringComparison.InvariantCultureIgnoreCase) ||
                !fileInfo.Name.EndsWith(".json")) continue;

            using var deserializationStream = File.OpenRead(fileInfo.FullName);
            CachedGame? deserializedCachedGame = JsonSerializer.Deserialize<CachedGame>(deserializationStream, SerializerOptions);

            return deserializedCachedGame ?? throw new Exception("Failed to deserialize the cached game.");
        }

        var clientFileInfo = new FileInfo(clientPath);
        using Stream clientFileStream = AcquireGameStream(platform, clientFileInfo);
        using IGame game = AcquireGame(platform, clientFileStream);

        _logger.LogInformation("Disassembling client");
        game.Disassemble();

        if (game.Platform == HPlatform.Flash)
        {
            _logger.LogInformation("Generating message hashes");
            game.GenerateMessageHashes();
        }

        _logger.LogInformation("Patching client");
        GamePatchingOptions patchingOptions = AcquireGamePatchingOptions(game.Platform, _options.GameListenPort);
        game.Patch(patchingOptions);

        _logger.LogInformation("Assembling client");
        string assemblePath = Path.Combine(PatchedClientsDirectory.FullName, $"{identifier}_{game.Revision}_{clientFileInfo.Name}");
        game.Assemble(assemblePath);

        var incoming = new Incoming(game);
        var outgoing = new Outgoing(game);
        var cachedGame = new CachedGame(game, patchingOptions, assemblePath);

        using FileStream gameSerializationStream = File.Create(Path.Combine(PatchedClientsDirectory.FullName, $"{identifier}_{game.Revision}.json"));
        JsonSerializer.Serialize(gameSerializationStream, cachedGame, SerializerOptions);

        return cachedGame;
    }
    public bool TryGetIdentifiers(string? revision, out Outgoing? outgoing, out Incoming? incoming)
    {
        outgoing = null;
        incoming = null;
        if (string.IsNullOrWhiteSpace(revision)) return false;
        foreach (FileInfo fileInfo in MessagesDirectory.EnumerateFiles())
        {
            if (!fileInfo.Name.EndsWith($"{revision}.json")) continue;
            
            using FileStream messagesDeserializationStream = File.OpenRead(fileInfo.FullName);
            var identifiers = JsonSerializer.Deserialize<CachedIdentifiers>(messagesDeserializationStream, SerializerOptions);

            outgoing = identifiers.Outgoing;
            incoming = identifiers.Incoming;
            return true;
        }
        return false;
    }
    public Process? LaunchClient(HPlatform platform, string ticket, string? clientPath = null)
    {
        PlatformPaths paths = GetPlatformPaths(platform, _options.PlatformPaths);
        if (string.IsNullOrWhiteSpace(clientPath))
        {
            clientPath = paths.ClientPath;
        }

        string targetLinkPath = Path.Combine(paths.RootPath, $"patched.{PlatformConverter.ToClientName(paths.Platform)}");
        _logger.LogInformation("Creating Hard Link: {targetLinkPath} -> {clientPath}", targetLinkPath, clientPath);

        File.Delete(targetLinkPath); // Load the original unmodified client if no explicit client path has been provided.
        if (!NativeMethods.CreateHardLink(targetLinkPath, clientPath, IntPtr.Zero))
        {
            _logger.LogError("Failed to create a hard link at the provided {linkPath}.", targetLinkPath);
        }

        if (platform == HPlatform.Flash)
        {
            ApplyFlashLauncherSettings(paths.RootPath, "patched.", "Tanji.");
        }

        var processStartInfo = new ProcessStartInfo(paths.ExecutablePath, $"server {ticket[..4]} ticket {ticket[5..]}")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        var clientProcess = Process.Start(processStartInfo);
        if (!clientProcess?.WaitForInputIdle(1000) ?? true)
        {
            _logger.LogError("Process idle state not yet reached, consider increasing timeout threshold.");
        }

        if (platform == HPlatform.Flash)
        {
            ApplyFlashLauncherSettings(paths.RootPath);
        }

        return clientProcess;
    }

    private static IGame AcquireGame(HPlatform platform, Stream clientFileStream) => platform switch
    {
        HPlatform.Flash => new FlashGame(clientFileStream),
        //HPlatform.Unity => new UnityGame(gameStream),
        _ => throw new ArgumentException("Failed to initialize a game instance for the provided platform.", nameof(platform))
    };
    private static Stream AcquireGameStream(HPlatform platform, FileInfo clientFileInfo)
    {
        if (platform != HPlatform.Flash) return clientFileInfo.OpenRead();

        Stream? gameStream = null;
        bool wasLoadedIntoMemory = false;
        try
        {
            gameStream = clientFileInfo.OpenRead();
            int firstByte = gameStream.ReadByte();

            gameStream.Position = 0;
            if (firstByte > 'Z') // Encrypted
            {
                using var gameBuffer = MemoryOwner<byte>.Allocate((int)clientFileInfo.Length);
                Span<byte> gameBufferSpan = gameBuffer.Span;

                gameStream.Read(gameBufferSpan);
                wasLoadedIntoMemory = true;

                int decryptedLength = FlashCrypto.Decrypt(ref gameBufferSpan, out int writtenOffset);
                return gameBuffer.Slice(writtenOffset, decryptedLength).AsStream();
            }
            return gameStream;
        }
        finally
        {
            if (wasLoadedIntoMemory)
            {
                // Original file stream should be disposed, as we'll be returning another stream that references a rented buffer.
                gameStream?.Dispose();
            }
        }
    }
    private static GamePatchingOptions AcquireGamePatchingOptions(HPlatform platform, int gameListenPort)
    {
        switch (platform)
        {
            case HPlatform.Flash:
            {
                return new GamePatchingOptions(HPatches.FlashDefaults)
                {
                    KeyShoutingId = 4002,
                    AddressShoutingId = 4000,
                    InjectedAddress = new IPEndPoint(IPAddress.Loopback, gameListenPort),
                };
            }
            default: throw new NotSupportedException("Unable to acquire game patch options for the provided platform.");
        }
    }
    private static void ApplyFlashLauncherSettings(string launcherRootPath, string? contentPrefix = null, string? idPrefix = null)
    {
        string applicationXMLPath = Path.Combine(launcherRootPath, "META-INF\\AIR\\application.xml");
        var habboAirSettings = new XmlDocument();
        habboAirSettings.Load(applicationXMLPath);

        XmlElement? idElement = habboAirSettings.DocumentElement?["id"];
        if (idElement == null)
        {
            ThrowHelper.ThrowNullReferenceException("The 'id' element does not exist in the application's XML configuration file.");
        }
        idElement.InnerText = $"{idPrefix}com.sulake.habboair";

        XmlElement? contentElement = habboAirSettings["application"]?["initialWindow"]?["content"];
        if (contentElement == null)
        {
            ThrowHelper.ThrowNullReferenceException("The 'application.initialWindow.content' element does not exist in the application's XML configuration file.");
        }
        contentElement.InnerText = $"{contentPrefix}{PlatformConverter.ToClientName(HPlatform.Flash)}";

        habboAirSettings.Save(applicationXMLPath);
    }
    private static PlatformPaths GetPlatformPaths(HPlatform platform, IReadOnlyDictionary<HPlatform, PlatformPaths>? platformPaths)
    {
        if (platformPaths == null || platformPaths.Count == 0)
        {
            throw new Exception("No associated paths for any platform available.");
        }
        if (!platformPaths.TryGetValue(platform, out PlatformPaths paths))
        {
            ThrowHelper.ThrowArgumentException("The provided platform does not have any paths associated with it.", nameof(platform));
        }
        return paths;
    }

    private readonly record struct CachedIdentifiers
    {
        public required Outgoing Outgoing { get; init; }
        public required Incoming Incoming { get; init; }
    }
}