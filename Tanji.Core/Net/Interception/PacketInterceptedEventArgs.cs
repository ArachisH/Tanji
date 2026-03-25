using System.ComponentModel;

using Tanji.Core.Net.Formats;

namespace Tanji.Core.Net.Interception;

public sealed class PacketInterceptedEventArgs : CancelEventArgs
{
    private Memory<byte> _packetBuffer;
    private ReadOnlyMemory<byte>? _roPacketBuffer;

    public bool IsOutgoing { get; }
    public IHFormat PacketFormat { get; }
    public ReadOnlyMemory<byte> PacketBuffer => _roPacketBuffer ?? _packetBuffer;

    public bool IsReplaced { get; private set; }
    /// <summary>
    /// Determines whether the underlying packet buffer is read-only.
    /// </summary>
    public bool IsReadOnly { get; private set; }

    public PacketInterceptedEventArgs(Memory<byte> originalPacketBuffer, IHFormat packetFormat, bool isOutgoing)
    {
        _packetBuffer = originalPacketBuffer;

        IsOutgoing = isOutgoing;
        PacketFormat = packetFormat;
    }

    public ref Memory<byte> GetMutablePacketBuffer()
    {
        if (IsReadOnly)
        {
            throw new Exception("Packet buffer is read-only.");
        }

        // Aassume data has been changed.
        IsReplaced = true;
        return ref _packetBuffer;
    }

    /// <summary>
    /// Replaces packet with a mutable packet buffer.
    /// Encryption process will alter the buffer.
    /// </summary>
    /// <param name="packetBuffer"></param>
    public void Replace(Memory<byte> packetBuffer)
    {
        _packetBuffer = packetBuffer;
        IsReplaced = true;
    }
    /// <summary>
    /// Replaces packet with a non-mutable buffer.
    /// Copying will occur during the encryption process.
    /// </summary>
    /// <param name="packetBuffer"></param>
    public void Replace(ReadOnlyMemory<byte> packetBuffer)
    {
        _roPacketBuffer = packetBuffer;
        IsReadOnly = true;
        IsReplaced = true;
    }
}