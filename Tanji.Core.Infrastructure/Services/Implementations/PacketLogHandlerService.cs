using System.Buffers;
using System.Threading.Channels;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;

using Tanji.Core.Infrastructure.Models;
using Tanji.Core.Infrastructure.Configuration;

namespace Tanji.Core.Infrastructure.Services.Implementations;

public sealed class PacketLogHandlerService : IPacketLogHandlerService
{
    private readonly Lock _writeSync;
    private readonly Channel<PacketLog> _packetLogs;

    private readonly IClientHandlerService _clientHandler;
    private readonly ILogger<PacketLogHandlerService> _logger;
    private readonly PacketLoggingOptions _packetLoggingOptions;

    private byte[]? _lastPacketBuffer;
    private PacketLog? _lastPacketLogWritten;

    public PacketLogHandlerService(ILogger<PacketLogHandlerService> logger,
        IOptions<TanjiOptions> options,
        IClientHandlerService clientHandler)
    {
        _logger = logger;
        _clientHandler = clientHandler;
        _packetLoggingOptions = options.Value.PacketLoggingOptions;

        _writeSync = new Lock();
        _packetLogs = Channel.CreateUnbounded<PacketLog>(new()
        {
            SingleWriter = true,
            SingleReader = true
        });
    }

    public ValueTask<PacketLog> ReadPacketLogAsync(CancellationToken cancellationToken = default)
    {
        return _packetLogs.Reader.ReadAsync(cancellationToken);
    }
    public ValueTask<bool> WaitForPacketLogsAsync(CancellationToken cancellationToken = default) => _packetLogs.Reader.WaitToReadAsync(cancellationToken);

    public PacketLog? WritePacketLog(ReadOnlySpan<byte> packetBufferSpan, in HMessage message)
    {
        lock (_writeSync)
        {
            PacketLog? pLog = null;
            if (_packetLoggingOptions.IsCompactingRepetitions)
            {
                if (_lastPacketBuffer?.Length < packetBufferSpan.Length)
                {
                    ArrayPool<byte>.Shared.Return(_lastPacketBuffer);
                    _lastPacketBuffer = null;
                }

                _lastPacketBuffer ??= ArrayPool<byte>.Shared.Rent(packetBufferSpan.Length + 1);
                Span<byte> lastPacketBufferSpan = _lastPacketBuffer.AsSpan(); // Do not trim, could be shorter than current packet.

                if (_lastPacketLogWritten != null && IsPacketRepeated(message.IsOutgoing, packetBufferSpan, lastPacketBufferSpan))
                {
                    /*
                     * The original packet may not be there by the time we update the Repetitions property.
                     * We need to add it back to the queue as quickly as possible (lock), so that consumers may act on it.
                     */
                    _lastPacketLogWritten.Repetitions++;
                    pLog = _lastPacketLogWritten;
                }
                else
                {
                    packetBufferSpan.CopyTo(lastPacketBufferSpan);
                    lastPacketBufferSpan[^1] = (byte)(message.IsOutgoing ? 1 : 0);
                }
            }
            else if (_lastPacketBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(_lastPacketBuffer);
                _lastPacketBuffer = null;
            }

            pLog ??= PacketLog.Create(packetBufferSpan, message, _packetLoggingOptions);
            if (_packetLogs.Writer.TryWrite(pLog))
            {
                _lastPacketLogWritten = pLog;
                return _lastPacketLogWritten;
            }

            return null;
        }
    }
    public PacketLog? WritePacketLog(ReadOnlySpan<byte> packetBufferSpan, bool isOutgoing, IHFormat format, string? revision)
    {
        bool hasRevisionMessages = _clientHandler.TryGetIdentifiers(
            revision, out Outgoing? outgoing, out Incoming? incoming);

        _ = format.TryReadHeader(packetBufferSpan, out int length, out short id, out _);
        if (hasRevisionMessages)
        {
            // Reduce the number of copies created per packet interception by pulling the message as a reference.
            ref readonly HMessage refMessage = ref (isOutgoing
                ? ref outgoing![id]
                : ref incoming![id]);

            // Message could be null reference. (Unresolved)
            if (!Unsafe.IsNullRef(in refMessage))
            {
                return WritePacketLog(packetBufferSpan, in refMessage);
            }
        }

        var message = new HMessage(id, isOutgoing);
        return WritePacketLog(packetBufferSpan, in message);
    }

    private static bool IsPacketRepeated(bool isOutgoing, in ReadOnlySpan<byte> current, in Span<byte> last)
    {
        // 0 = Incoming, Non-Zero = Outgoing
        if (isOutgoing && last[^1] == 0) return false;
        if (!isOutgoing && last[^1] != 0) return false;

        // If current packet is larger than last packet (excluding direction byte), they can't be equal.
        return current.Length <= (last.Length - 1) && current.SequenceEqual(last[..current.Length]);
    }
}