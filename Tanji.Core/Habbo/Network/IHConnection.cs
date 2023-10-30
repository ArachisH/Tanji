namespace Tanji.Core.Habbo.Network;

public interface IHConnection
{
    Incoming? In { get; }
    Outgoing? Out { get; }
    HotelEndPoint RemoteEndPoint { get; }

    ValueTask SendToClientAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);
    ValueTask SendToServerAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default);
}