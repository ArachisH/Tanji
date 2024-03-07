using System.Security.Cryptography.X509Certificates;

using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Network;

public readonly record struct HConnectionOptions
{
    public int ListenPort { get; init; } = default;
    public int MinimumConnectionAttempts { get; init; } = 1;
    public bool IsFakingPolicyRequest { get; init; } = true;
    public bool IsWebSocketConnection { get; init; } = default;

    public IHFormat SendPacketFormat { get; init; } = IHFormat.EvaWire;
    public IHFormat ReceivePacketFormat { get; init; } = IHFormat.EvaWire;

    public X509Certificate? WebSocketServerCertificate { get; init; } = default;

    public HConnectionOptions(IGame game, GamePatchingOptions appliedPatches)
    {
        SendPacketFormat = game.SendPacketFormat;
        ReceivePacketFormat = game.ReceivePacketFormat;
        ListenPort = appliedPatches.InjectedAddress?.Port ?? 0;
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        IsFakingPolicyRequest = MinimumConnectionAttempts > 1;
        IsWebSocketConnection = game.Platform == HPlatform.Unity;
    }
}