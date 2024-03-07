using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Canvas;

public interface IGame : IDisposable
{
    string? Path { get; }

    bool IsPostShuffle { get; }
    HPlatform Platform { get; }

    IHFormat SendPacketFormat { get; }
    IHFormat ReceivePacketFormat { get; }

    string? Revision { get; }
    int MinimumConnectionAttempts { get; }

    GamePatchingOptions AppliedPatchingOptions { get; }

    void Disassemble();
    void Assemble(string path);
    void GenerateMessageHashes();
    void Patch(GamePatchingOptions options);

    bool TryResolveMessage(uint hash, out HMessage message);
    bool TryResolveMessage(string name, out HMessage message);
}