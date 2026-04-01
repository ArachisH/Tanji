using System.Net;
using System.Xml;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

using Flazzy.Tools;

using Tanji.Core.Canvas;
using Tanji.Core.Canvas.Flash;
using Tanji.Core.Net.Messages;
using Tanji.Core.Infrastructure.Configuration;
using Tanji.Core.Infrastructure.Json.Converters;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class ClientHandlerService : IClientHandlerService
{
    private static readonly JsonSerializerOptions SerializerOptions;

    private readonly TanjiOptions _options;
    private readonly ILogger<ClientHandlerService> _logger;

    public DirectoryInfo MessagesDirectory { get; }
    public DirectoryInfo PatchedClientsDirectory { get; }

    static ClientHandlerService()
    {
        SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        SerializerOptions.Converters.Add(new FormatConverter());
        SerializerOptions.Converters.Add(new PlatformConverter());
        SerializerOptions.Converters.Add(new FileInfoConverter());
        SerializerOptions.Converters.Add(new IPEndPointConverter());
    }
    public ClientHandlerService(ILogger<ClientHandlerService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        MessagesDirectory = Directory.CreateDirectory("Messages");
        MessagesDirectory.Attributes = FileAttributes.Normal;

        PatchedClientsDirectory = Directory.CreateDirectory("Patched Clients");
        PatchedClientsDirectory.Attributes = FileAttributes.Normal;
    }

    public async Task<IGame> PatchClientAsync(HPlatform platform, string? clientPath = null)
    {
        if (string.IsNullOrWhiteSpace(clientPath))
        {
            PlatformPaths paths = GetPlatformPaths(platform, _options.PlatformPaths);
            clientPath = paths.ClientPath;
        }

        if (!File.Exists(clientPath))
        {
            _logger.LogCritical("File does not exist: {filePath}", clientPath);
            ThrowHelper.ThrowFileNotFoundException("The provided file path does not exist.", clientPath);
        }

        // Compute Chopped Hash
        byte[] clientFileHash = new byte[16];
        using FileStream tempClientFileStream = File.OpenRead(clientPath);

        // Attempt to load game data from a json file.
        await MD5.HashDataAsync(tempClientFileStream, clientFileHash).ConfigureAwait(false);

        string md5Hash = Convert.ToHexString(clientFileHash, 0, 4);
        foreach (FileInfo fileInfo in PatchedClientsDirectory.EnumerateFiles())
        {
            if (!fileInfo.Name.StartsWith(md5Hash, StringComparison.InvariantCultureIgnoreCase) ||
                !fileInfo.Name.EndsWith(".json")) continue;

            using var deserializationStream = File.OpenRead(fileInfo.FullName);

            CachedGame? deserializedCachedGame = JsonSerializer.Deserialize<CachedGame>(deserializationStream, SerializerOptions)
                ?? throw new Exception("Failed to deserialize cached game file.");

            _logger.LogInformation("Discovered Cached Client > {path}", deserializedCachedGame.Path!.FullName);
            return deserializedCachedGame;
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
        string assemblePath = Path.Combine(PatchedClientsDirectory.FullName, $"{md5Hash}_{game.Revision}_{clientFileInfo.Name}");
        game.Assemble(assemblePath);
        var cachedGame = new CachedGame(game, patchingOptions, assemblePath);

        using FileStream gameSerializationStream = File.Create(Path.Combine(PatchedClientsDirectory.FullName, $"{md5Hash}_{game.Revision}.json"));
        JsonSerializer.Serialize(gameSerializationStream, cachedGame, SerializerOptions);

        using FileStream messagesSerializationStream = File.Create(Path.Combine(MessagesDirectory.FullName, $"{game.Revision}.json"));
        JsonSerializer.Serialize(messagesSerializationStream, new CachedIdentifiers
        {
            Outgoing = new Outgoing(game),
            Incoming = new Incoming(game)
        }, SerializerOptions);

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
    public Task<Process> LaunchClientAsync(HPlatform platform, string ticket, string? clientPath = null)
    {
        PlatformPaths paths = GetPlatformPaths(platform, _options.PlatformPaths);
        if (string.IsNullOrWhiteSpace(clientPath))
        {
            clientPath = paths.ClientPath;
        }

        string targetLinkPath = Path.Combine(paths.RootPath, $"patched.{PlatformConverter.ToClientName(paths.Platform)}");
        _logger.LogInformation("Creating Hard Link: {targetLinkPath} -> {clientPath}", targetLinkPath, clientPath);

        File.Delete(targetLinkPath); // Load the original unmodified client if no explicit client path has been provided.
        if (!NativeMethods.CreateHardLink(targetLinkPath, clientPath, nint.Zero))
        {
            _logger.LogError("Failed to create a hard link at the target file path: {linkPath}", targetLinkPath);
        }

        return platform switch
        {
            HPlatform.Flash => LaunchFlashClientAsync(paths, ticket),
            _ => throw new NotSupportedException($"{platform} is not currently supported for launching.")
        };
    }

    private void Process_Exited(object? sender, EventArgs e)
    {
        var process = (Process)sender!;
        process.Exited -= Process_Exited;
        process.ErrorDataReceived -= Process_DataReceived;
        process.OutputDataReceived -= Process_DataReceived;
        _logger.LogTrace("Client launcher process has exited.");
    }
    private void Process_DataReceived(object? sender, DataReceivedEventArgs e)
    {
        _logger.LogTrace("{Data}", e.Data);
    }
    private async Task<Process> LaunchFlashClientAsync(PlatformPaths paths, string ticket)
    {
        ProcessStartInfo info = _options.IsUsingAirDebugLauncher
            ? new ProcessStartInfo(Environment.ExpandEnvironmentVariables("%AIR_HOME%\\bin\\adl.exe"))
        {
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = paths.RootPath,
                Arguments = $"\".\\META-INF\\AIR\\application.xml\" -profile extendedDesktop root-dir . -- server {ticket[..4]} ticket {ticket[5..]}",
            } : new ProcessStartInfo(paths.ExecutablePath, $"server {ticket[..4]} ticket {ticket[5..]}");

        var launcherProcess = new Process
        {
            StartInfo = info,
            EnableRaisingEvents = true,
        };
        launcherProcess.Exited += Process_Exited;
        launcherProcess.ErrorDataReceived += Process_DataReceived;
        launcherProcess.OutputDataReceived += Process_DataReceived;

        try
        {
            ApplyFlashLauncherSettings(paths.RootPath, "patched.", "Tanji.");
            if (launcherProcess.Start())
            {
                // Wait for process to finish using the modified 'application.xml' file, then reset.
                await Task.Delay(1000).ConfigureAwait(false);
            }
            else throw new Exception("Failed to start the flash client process.");
        }
        finally { ApplyFlashLauncherSettings(paths.RootPath); }

        return launcherProcess;
    }

    private static IGame AcquireGame(HPlatform platform, Stream clientFileStream) => platform switch
    {
        HPlatform.Flash => new FlashGame(clientFileStream),
        //HPlatform.Unity => new UnityGame(clientFileStream),
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

                gameStream.ReadExactly(gameBufferSpan);
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
                // Original file stream should be disposed, as we'll be returning another smaller stream with just the required information.
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