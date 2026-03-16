using System.Security.Cryptography.X509Certificates;

using Tanji.Core.Canvas;
using Tanji.Core.Net.Formats;

namespace Tanji.Core.Net.Interception;

public readonly record struct HConnectionContext
{
    public string? ClientPath { get; init; }
    public HPlatform Platform { get; init; }

    public int MinimumConnectionAttempts { get; init; } = 1;
    public bool IsFakingPolicyRequest { get; init; } = true;
    public bool IsWebSocketConnection { get; init; } = default;
    public GamePatchingOptions PatchingOptions { get; init; } = default;

    public IHFormat InboundPacketFormat { get; init; } = IHFormat.EvaWire;
    public IHFormat OutboundPacketFormat { get; init; } = IHFormat.EvaWire;

    public X509Certificate? WebSocketClientCertificate { get; init; } = default;
    public X509Certificate? WebSocketServerCertificate { get; init; } = default;

    public HConnectionContext(IGame game)
    {
        ClientPath = game.Path;
        Platform = game.Platform;

        PatchingOptions = game.PatchingOptions;
        InboundPacketFormat = game.InboundPacketFormat;
        OutboundPacketFormat = game.OutboundPacketFormat;
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        IsFakingPolicyRequest = MinimumConnectionAttempts > 1;
        IsWebSocketConnection = game.Platform == HPlatform.Unity;
    }
}