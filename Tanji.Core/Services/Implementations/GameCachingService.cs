using System.Net;
using System.Text.Json;
using System.Security.Cryptography;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Json;
using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;
using Tanji.Core.Habbo.Canvas.Flash;

using Flazzy.Tools;

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Services;

public sealed class GameCachingService : IFileCachingService<PlatformPaths, CachedGame>
{
    private static readonly JsonSerializerOptions SerializerOptions;
    private static readonly IPAddress LocalHostIP = IPAddress.Parse("127.0.0.1");

    private readonly TanjiOptions _options;
    private readonly ILogger<GameCachingService> _logger;

    public required DirectoryInfo Root { get; init; }
    public required DirectoryInfo ModifiedClients { get; init; }

    static GameCachingService()
    {
        SerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        SerializerOptions.Converters.Add(new FormatConverter());
        SerializerOptions.Converters.Add(new PlatformConverter());
    }
    public GameCachingService(ILogger<GameCachingService> logger, IOptions<TanjiOptions> options)
    {
        _logger = logger;
        _options = options.Value;

        Root = Directory.CreateDirectory("Cache");
        ModifiedClients = Root.CreateSubdirectory("Modified Clients");
    }

    public CachedGame GetOrAdd(PlatformPaths paths)
    {
        if (!File.Exists(paths.ClientPath))
        {
            _logger.LogError("File does not exist: {filePath}", paths.ClientPath);
            ThrowHelper.ThrowFileNotFoundException("The provide file path does not exist.", paths.ClientPath);
        }

        // Compute Chopped Hash
        Span<byte> fileHash = stackalloc byte[16];
        using FileStream fs = File.OpenRead(paths.ClientPath);

        // Attempt to load game data from a json file.
        MD5.HashData(fs, fileHash);
        string identifier = Convert.ToHexString(fileHash.Slice(0, 4));
        foreach (var item in Root.EnumerateFiles())
        {
            if (!item.Name.StartsWith(identifier, StringComparison.InvariantCultureIgnoreCase) ||
                !item.Name.EndsWith(".json")) continue;

            using var deserializationStream = File.OpenRead(item.FullName);
            CachedGame? deserializedCachedGame = JsonSerializer.Deserialize<CachedGame>(deserializationStream, SerializerOptions);

            return deserializedCachedGame ?? throw new Exception("Failed to deserialize the cached game.");
        }

        var gameInfo = new FileInfo(paths.ClientPath);
        using Stream gameStream = AcquireGameStream(gameInfo, paths.Platform);
        using IGame game = AcquireGame(gameStream, paths.Platform);

        _logger.LogInformation("Disassembling client");
        game.Disassemble();

        if (game.Platform == HPlatform.Flash)
        {
            _logger.LogInformation("Generating message hashes");
            game.GenerateMessageHashes();
        }

        _logger.LogInformation("Patching client");
        game.Patch(AcquireGamePatchingOptions(_options, game.Platform));

        _logger.LogInformation("Assembling client");
        string assemblePath = Path.Combine(ModifiedClients.FullName, $"{identifier}_{game.Revision}{Path.GetExtension(paths.ClientPath)}");
        game.Assemble(assemblePath);

        var incoming = new Incoming(game);
        var outgoing = new Outgoing(game);
        var cachedGame = new CachedGame(game, outgoing, incoming, assemblePath);

        using FileStream serializationStream = File.OpenWrite(Path.Combine(Root.FullName, $"{identifier}_{game.Revision}.json"));
        JsonSerializer.Serialize(serializationStream, cachedGame, SerializerOptions);

        return cachedGame;
    }

    private static IGame AcquireGame(Stream gameStream, HPlatform platform) => platform switch
    {
        HPlatform.Flash => new FlashGame(gameStream),
        _ => throw new ArgumentException("Failed to initialize a game instance for the provided platform.", nameof(platform))
    };
    private static Stream AcquireGameStream(FileInfo gameInfo, HPlatform platform)
    {
        if (platform != HPlatform.Flash) return gameInfo.OpenRead();

        Stream? gameStream = null;
        bool wasLoadedIntoMemory = false;
        try
        {
            gameStream = gameInfo.OpenRead();
            int firstByte = gameStream.ReadByte();

            gameStream.Position = 0;
            if (firstByte > 'Z') // Encrypted
            {
                using var gameBuffer = MemoryOwner<byte>.Allocate((int)gameInfo.Length);
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
    private static GamePatchingOptions AcquireGamePatchingOptions(TanjiOptions options, HPlatform platform)
    {
        switch (platform)
        {
            case HPlatform.Flash:
            {
                return new GamePatchingOptions(HPatches.FlashDefaults)
                {
                    KeyShoutingId = 4002,
                    AddressShoutingId = 4000,
                    RemoteAddress = new IPEndPoint(LocalHostIP, options.GameListenPort),
                };
            }
            default: throw new NotSupportedException("Unable to acquire game patch options for the provided platform.");
        }
    }
}