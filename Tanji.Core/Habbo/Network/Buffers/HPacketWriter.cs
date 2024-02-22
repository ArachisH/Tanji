using System.Buffers;

using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Network.Buffers;

/// <summary>
/// Represents a rented heap-based, byte array output sink into which unmanaged values, and UTF8 encoded strings can be written to.
/// </summary>
public sealed class HPacketWriter : IBufferWriter<byte>, IDisposable
{
    private bool _diposed;
    private Memory<byte> _buffer;
    private IMemoryOwner<byte>? _owner;

    public short Id { get; set; }
    public IHFormat Format { get; init; }

    public int WrittenCount { get; private set; }
    public int Capacity => _buffer.Length - (WrittenCount + Format.MinBufferSize);

    // Only return the written content of the packet, and not the header.
    public ReadOnlySpan<byte> WrittenSpan => _buffer.Span.Slice(Format.MinBufferSize, WrittenCount);
    public ReadOnlyMemory<byte> WrittenMemory => _buffer.Slice(Format.MinBufferSize, WrittenCount);

    public HPacketWriter(IHFormat format)
        : this(format, 0)
    { }
    public HPacketWriter(IHFormat format, short id)
    {
        ArgumentNullException.ThrowIfNull(format);

        Id = id;
        Format = format;
        WrittenCount += format.MinBufferSize;

        Span<byte> headerSpan = GetSpan(format.MinBufferSize);
        if (!format.TryWriteHeader(headerSpan, format.MinPacketLength, id, out int bytesWritten))
        {
            ThrowHelper.ThrowArgumentException("Failed to write the packet header.", nameof(format));
        }
        Advance(bytesWritten);
    }

    public void Write<T>(T value) where T : struct
    {
        int size = Format.GetSize(value);
        Span<byte> destination = GetSpan(size);
        if (!Format.TryWrite(destination, value, out int bytesWritten))
        {
            // TODO: What exception to throw here?
        }
        Advance(bytesWritten);
    }
    public void WriteUTF8(ReadOnlySpan<char> value)
    {
        int size = Format.GetSize(value);
        Span<byte> destination = GetSpan(size);
        if (!Format.TryWriteUTF8(destination, value, out int bytesWritten))
        {
            // TODO: What exception to throw here?
        }
        Advance(bytesWritten);
    }
    public void WriteUTF8(string? value) => WriteUTF8(value.AsSpan());

    public void Advance(int count)
    {
        if (count < 0)
        {
            ThrowHelper.ThrowArgumentException("The count must be a non-negative integer.", nameof(count));
        }

        if (WrittenCount > _buffer.Length - count)
        {
            ThrowHelper.ThrowInvalidOperationException("Attempting to advance past the buffer length.");
        }

        WrittenCount += count;
    }
    public Span<byte> GetSpan(int sizeHint = 0)
    {
        Grow(sizeHint);
        return _buffer.Span.Slice(WrittenCount);
    }
    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        Grow(sizeHint);
        return _buffer.Slice(WrittenCount);
    }

    private void Grow(int sizeHint)
    {
        if (sizeHint < 0)
        {
            ThrowHelper.ThrowArgumentException("The size hint must be a non-negative integer.", nameof(sizeHint));
        }

        if (sizeHint == 0)
        {
            sizeHint = 1;
        }

        if (sizeHint > Capacity)
        {
            // Calculate Size
            int growSize = (_buffer.Length + (sizeHint - Capacity)) * 2;

            // Grow Buffer
            IMemoryOwner<byte> _grownOwner = MemoryPool<byte>.Shared.Rent(growSize);
            Memory<byte> grownBuffer = _grownOwner.Memory;

            // Copy & Replace Buffer
            _buffer.CopyTo(grownBuffer);
            _buffer = grownBuffer;

            // Dispose & Replace Owner
            _owner?.Dispose();
            _owner = _grownOwner;
        }
    }

    public void Dispose()
    {
        if (_diposed || _owner == null) return;

        _owner.Dispose();
        _diposed = true;
    }
}