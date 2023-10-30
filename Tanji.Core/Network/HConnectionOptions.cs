using System.Net;
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

    public IHFormat ClientSendPacketFormat { get; init; } = IHFormat.EvaWire;
    public IHFormat ClientReceivePacketFormat { get; init; } = IHFormat.EvaWire;

    public EndPoint? RemoteEndPoint { get; init; } = default;
    public X509Certificate? WebSocketServerCertificate { get; init; } = default;

    public HConnectionOptions(HGame game)
    {
        RemoteEndPoint = game.RemoteEndPoint;
        ClientSendPacketFormat = game.SendPacketFormat;
        ClientReceivePacketFormat = game.ReceivePacketFormat;
        MinimumConnectionAttempts = game.MinimumConnectionAttempts;

        IsFakingPolicyRequest = MinimumConnectionAttempts > 1;
        IsWebSocketConnection = game.Platform == HPlatform.Unity;
    }
    public HConnectionOptions(EndPoint remoteEndPoint)
    {
        RemoteEndPoint = remoteEndPoint;
    }
    public HConnectionOptions(HConnectionOptions options)
    {
        ListenPort = options.ListenPort;
        MinimumConnectionAttempts = options.MinimumConnectionAttempts;
        IsFakingPolicyRequest = options.IsFakingPolicyRequest;
        IsWebSocketConnection = options.IsWebSocketConnection;

        ClientSendPacketFormat = options.ClientSendPacketFormat;
        ClientReceivePacketFormat = options.ClientReceivePacketFormat;

        RemoteEndPoint = options.RemoteEndPoint;
        WebSocketServerCertificate = options.WebSocketServerCertificate;
    }
}