using System.Net;
using System.Security.Cryptography;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Internals;
using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration;
using Tanji.Core.Habbo.Canvas.Flash;
using Tanji.Core.Configuration.Json;

using Flazzy.Tools;

using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Services;

public sealed class GameCachingService : IFileCachingService<PlatformPaths, CachedGame>
{
    private readonly TanjiOptions _options;
    private readonly ILogger<GameCachingService> _logger;

    public required DirectoryInfo Root { get; init; }
    public required DirectoryInfo ModifiedClients { get; init; }

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

            using var jsonBuffer = MemoryOwner<byte>.Allocate((int)item.Length);
            Span<byte> jsonBufferSpan = jsonBuffer.Span;

            using var jsonBufferFs = File.OpenRead(item.FullName);
            jsonBufferFs.Read(jsonBufferSpan);

            return new CachedGame(jsonBufferSpan);
        }

        var gameInfo = new FileInfo(paths.ClientPath);
        using Stream gameStream = AcquireGameStream(gameInfo, paths.Platform);

        using HGame game = AcquireGame(gameStream, paths.Platform);
        game.Path = gameInfo.FullName;

        _logger.LogInformation("Disassembling client");
        game.Disassemble();

        if (game.Platform == HPlatform.Flash)
        {
            _logger.LogInformation("Generating message hashes");
            game.GenerateMessageHashes();
        }

        _logger.LogInformation("Patching client");
        game.Patch();

        _logger.LogInformation("Assembling client");
        string assemblePath = Path.Combine(ModifiedClients.FullName, $"{identifier}_{game.Revision}.swf");
        game.Assemble(assemblePath);

        // We can create hard links without admin, but not symbolic links ??
        _logger.LogInformation("Creating hard link to patched client");
        string linkPath = Path.Combine(paths.RootPath, $"patched.{PlatformConverter.ToClientName(paths.Platform)}");
        NativeMethods.CreateHardLink(linkPath, assemblePath, IntPtr.Zero);

        // TODO: Load Incoming/Outgoing identifier instances

        return null;
    }

    private HGame AcquireGame(Stream gameStream, HPlatform platform) => platform switch
    {
        HPlatform.Flash => new FlashGame(gameStream)
        {
            KeyShouterId = 4002,
            RemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _options.GameListenPort)
        },
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
}