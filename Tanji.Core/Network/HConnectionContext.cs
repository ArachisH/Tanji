using System.Security.Cryptography.X509Certificates;

using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Network;

public readonly record struct HConnectionContext
{
    public string? ClientPath { get; init; }
    public HPlatform Platform { get; init; }

    public int MinimumConnectionAttempts { get; init; } = 1;
    public bool IsFakingPolicyRequest { get; init; } = true;
    public bool IsWebSocketConnection { get; init; } = default;
    public GamePatchingOptions AppliedPatchingOptions { get; init; } = default;

    public IHFormat SendPacketFormat { get; init; } = IHFormat.EvaWire;
    public IHFormat ReceivePacketFormat { get; init; } = IHFormat.EvaWire;

    public X509Certificate? WebSocketServerCertificate { get; init; } = default;

    public HConnectionContext(IGame game)
    {
        ClientPath = game.Path;
        Platform = game.Platform;

        SendPacketFormat = game.SendPacketFormat;
        ReceivePacketFormat = game.ReceivePacketFormat;
        AppliedPatchingOptions = game.AppliedPatchingOptions;
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        IsFakingPolicyRequest = MinimumConnectionAttempts > 1;
        IsWebSocketConnection = game.Platform == HPlatform.Unity;
    }
}