using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

using Tanji.Core.Json;
using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;

namespace Tanji.Core.Canvas;

public sealed class CachedGame : IGame
{
    public required string Path { get; init; }

    public required bool IsPostShuffle { get; init; }
    [JsonConverter(typeof(PlatformConverter))]
    public required HPlatform Platform { get; init; }

    [JsonConverter(typeof(FormatConverter))]
    public required IHFormat SendPacketFormat { get; init; }
    [JsonConverter(typeof(FormatConverter))]
    public required IHFormat ReceivePacketFormat { get; init; }

    public required string Revision { get; init; }
    public required int MinimumConnectionAttempts { get; init; }

    public required GamePatchingOptions AppliedPatchingOptions { get; init; }

    [JsonIgnore]
    public bool IsDisposed { get; private set; }

    public CachedGame()
    { }
    [SetsRequiredMembers]
    public CachedGame(IGame game, GamePatchingOptions appliedPatches, string clientPath)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));

        IsPostShuffle = game.IsPostShuffle;
        Platform = game.Platform;

        SendPacketFormat = game.SendPacketFormat;
        ReceivePacketFormat = game.ReceivePacketFormat;

        Revision = game.Revision ?? "< Unknown Revision >";
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        Path = clientPath;
        AppliedPatchingOptions = appliedPatches;
    }

    public void Disassemble() => throw new NotSupportedException();
    public void Assemble(string path) => throw new NotSupportedException();
    public void GenerateMessageHashes() => throw new NotSupportedException();
    public void Patch(GamePatchingOptions options) => throw new NotSupportedException();

    public bool TryResolveMessage(uint hash, out HMessage message) => throw new NotSupportedException();
    public bool TryResolveMessage(string name, out HMessage message) => throw new NotSupportedException();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }
            IsDisposed = true;
        }
    }
}