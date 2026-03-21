using System.Diagnostics.CodeAnalysis;

using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;

namespace Tanji.Core.Canvas;

public sealed class CachedGame : IGame
{
    public required FileInfo? Path { get; init; }

    public required bool IsPostShuffle { get; init; }
    public required HPlatform Platform { get; init; }

    public required IHFormat InboundPacketFormat { get; init; }
    public required IHFormat OutboundPacketFormat { get; init; }

    public required string Revision { get; init; }
    public required int MinimumConnectionAttempts { get; init; }

    public required GamePatchingOptions PatchingOptions { get; init; }

    public CachedGame()
    { }
    [SetsRequiredMembers]
    public CachedGame(IGame game, GamePatchingOptions patchingOptions, string clientPath)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));

        IsPostShuffle = game.IsPostShuffle;
        Platform = game.Platform;

        OutboundPacketFormat = game.OutboundPacketFormat;
        InboundPacketFormat = game.InboundPacketFormat;

        Revision = game.Revision ?? "< Unknown Revision >";
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        Path = new FileInfo(clientPath);
        PatchingOptions = patchingOptions;
    }

    void IGame.Disassemble() => throw new NotSupportedException();
    void IGame.Assemble(string path) => throw new NotSupportedException();
    void IGame.GenerateMessageHashes() => throw new NotSupportedException();
    void IGame.Patch(GamePatchingOptions options) => throw new NotSupportedException();

    bool IGame.TryResolveMessage(uint hash, out HMessage message) => throw new NotSupportedException();
    bool IGame.TryResolveMessage(string name, out HMessage message) => throw new NotSupportedException();

    void IDisposable.Dispose() => throw new NotSupportedException();
}