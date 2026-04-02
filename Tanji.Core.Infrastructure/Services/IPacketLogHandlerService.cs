using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;
using Tanji.Core.Infrastructure.Models;

namespace Tanji.Core.Infrastructure.Services;

public interface IPacketLogHandlerService
{
    ValueTask<bool> WaitForPacketLogsAsync(CancellationToken cancellationToken = default);
    ValueTask<PacketLog> ReadPacketLogAsync(CancellationToken cancellationToken = default);

    PacketLog? WritePacketLog(ReadOnlySpan<byte> packetBufferSpan, in HMessage message);
    PacketLog? WritePacketLog(ReadOnlySpan<byte> packetBufferSpan, bool isOutgoing, IHFormat format, string? revision);
}