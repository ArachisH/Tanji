using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Canvas;

public interface IGame
{
    bool IsPostShuffle { get; }
    HPlatform Platform { get; }

    IHFormat SendPacketFormat { get; }
    IHFormat ReceivePacketFormat { get; }

    string? Path { get; }
    string? Revision { get; }
    int MinimumConnectionAttempts { get; }

    bool TryResolveMessage(string name, uint hash, bool isOutgoing, out HMessage message);
}