using System.Diagnostics;
using System.Buffers.Binary;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Habbo.Network;

internal sealed class WebSocketStream : Stream
{
    private const int MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE = 125;

    private static readonly byte[] _emptyMask = new byte[4];

    private readonly byte[]? _mask;
    private readonly bool _isClient;
    private readonly bool _leaveOpen;
    private readonly Stream _innerStream;

    private bool _disposed;

    public WebSocketStream(Stream innerStream)
        : this(innerStream, null, false)
    { }
    public WebSocketStream(Stream innerStream, byte[]? mask, bool leaveOpen)
    {
        _mask = mask;
        _leaveOpen = leaveOpen;
        _isClient = mask != null;
        _innerStream = innerStream;
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        bool isMasked;
        int op, received, payloadLength;
        using MemoryOwner<byte> headerOwner = MemoryOwner<byte>.Allocate(14);
        do
        {
            received = await _innerStream.ReadAsync(headerOwner.Memory.Slice(0, 2), cancellationToken).ConfigureAwait(false);
            if (received != 2) return -1; // The size of the WebSocket frame header should at minimum be two bytes.
            HeaderDecode(headerOwner.Span, out isMasked, out payloadLength, out op);
        }
        while (payloadLength == 0 || op != 2); // Continue to receive fragments until a binary frame with a payload size more than zero is found.

        switch (payloadLength)
        {
            case 126:
            {
                received += await _innerStream.ReadAsync(headerOwner.Memory.Slice(received, sizeof(ushort)), cancellationToken).ConfigureAwait(false);
                payloadLength = BinaryPrimitives.ReadUInt16BigEndian(headerOwner.Span.Slice(2, sizeof(ushort)));
                break;
            }
            case 127:
            {
                received += await _innerStream.ReadAsync(headerOwner.Memory.Slice(received, sizeof(ulong)), cancellationToken).ConfigureAwait(false);
                payloadLength = (int)BinaryPrimitives.ReadUInt64BigEndian(headerOwner.Span.Slice(2, sizeof(ulong))); // I hope payloads aren't actually this big.
                break;
            }
        }

        Memory<byte> maskRegion = null;
        if (isMasked)
        {
            maskRegion = headerOwner.Memory.Slice(received, 4);
            await _innerStream.ReadAsync(maskRegion, cancellationToken).ConfigureAwait(false);
        }

        received = 0;
        Memory<byte> payloadRegion = buffer.Slice(0, Math.Min(payloadLength, buffer.Length));
        do
        {
            // Attempt to copy the entire WebSocket payload into the buffer, otherwise, if there is not enough room, specify how much data has been left with the '_leftoverPayloadBytes' field.
            received += await _innerStream.ReadAsync(payloadRegion.Slice(received, payloadRegion.Length - received), cancellationToken).ConfigureAwait(false);
        }
        while (received != payloadRegion.Length);

        if (isMasked)
        {
            PayloadUnmask(payloadRegion.Slice(0, received).Span, maskRegion.Span);
        }
        return received;
    }
    public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        using MemoryOwner<byte> headerOwner = MemoryOwner<byte>.Allocate(14);
        for (int i = 0, payloadLeft = buffer.Length; payloadLeft > 0; payloadLeft -= MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE, i++)
        {
            int headerLength = HeaderEncode(headerOwner.Span, payloadLeft, i == 0, _isClient, out bool isFinalFragment);
            int payloadLength = isFinalFragment ? payloadLeft : MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE;

            MemoryOwner<byte>? maskedOwner = null;
            ReadOnlyMemory<byte> payloadFragment = buffer.Slice(i * MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE, payloadLength);

            await _innerStream.WriteAsync(headerOwner.Memory.Slice(0, headerLength), cancellationToken).ConfigureAwait(false);
            if (_mask != null)
            {
                await _innerStream.WriteAsync(_mask, cancellationToken).ConfigureAwait(false);
                maskedOwner = PayloadMask(payloadFragment.Span, _mask);
            }

            if (maskedOwner != null) // MaskPayload could return null, which means that we used the '_emptyMask' instance, WHICH also means no masking was done on the payload.
            {
                await _innerStream.WriteAsync(maskedOwner.Memory.Slice(0, payloadFragment.Length), cancellationToken).ConfigureAwait(false);
                maskedOwner.Dispose();
            }
            else await _innerStream.WriteAsync(payloadFragment, cancellationToken).ConfigureAwait(false);

            if (_isClient) break;
        }
    }

    #region Stream Implementation
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();

    public override void Flush() => _innerStream.Flush();
    public override Task FlushAsync(CancellationToken cancellationToken) => _innerStream.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && !_disposed && !_leaveOpen)
        {
            _innerStream.Dispose();
            _disposed = true;
        }
    }
    #endregion

    private static void PayloadUnmask(Span<byte> payload, Span<byte> mask)
    {
        for (int i = 0; i < payload.Length; i++)
        {
            payload[i] ^= mask[i % 4];
        }
    }
    private static MemoryOwner<byte>? PayloadMask(ReadOnlySpan<byte> payload, ReadOnlySpan<byte> mask)
    {
        if (mask == _emptyMask) return null;
        var maskedOwner = MemoryOwner<byte>.Allocate(payload.Length);

        Span<byte> masked = maskedOwner.Span;
        for (int i = 0; i < payload.Length; i++)
        {
            masked[i] = (byte)(payload[i] ^ mask[i % 4]);
        }
        return maskedOwner;
    }

    [DebuggerStepThrough]
    private static void HeaderDecode(Span<byte> header, out bool isMasked, out int payloadLength, out int op)
    {
        op = header[0] & 0b00001111;
        payloadLength = header[1] & 0x7F;
        isMasked = (header[1] & 0x80) == 0x80;
    }
    private static int HeaderEncode(Span<byte> header, int payloadLeft, bool isFirstFragment, bool isClient, out bool isFinalFragment)
    {
        isFinalFragment = isClient || payloadLeft <= MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE;

        int headerBits = isFinalFragment ? 1 : 0;
        headerBits = (headerBits << 1) + 0; // RSV1 - IsDataCompressed
        headerBits = (headerBits << 1) + 0; // RSV2
        headerBits = (headerBits << 1) + 0; // RSV3
        headerBits = (headerBits << 4) + (isFirstFragment ? 2 : 0); // Mark as binary, otherwise continuation
        headerBits = (headerBits << 1) + (isClient ? 1 : 0); // Payload should be masked when acting as the client, otherwise specify that no mask is present(0).

        int headerLength = 2;
        int payloadLength = isFinalFragment ? payloadLeft : MAX_WEBSOCKET_TOCLIENT_PAYLOAD_SIZE;
        if (isClient && payloadLeft > 125) // Fragmenting the payload is not necessary when sending data to the server.
        {
            if (payloadLeft <= ushort.MaxValue)
            {
                payloadLength = 126;
                headerLength += sizeof(ushort);
                BinaryPrimitives.WriteUInt16BigEndian(header.Slice(2), (ushort)payloadLeft);
            }
            else
            {
                payloadLength = 127;
                headerLength += sizeof(ulong);
                BinaryPrimitives.WriteUInt64BigEndian(header.Slice(2), (ulong)payloadLeft);
            }
        }

        headerBits = (headerBits << 7) + payloadLength;
        BinaryPrimitives.WriteUInt16BigEndian(header, (ushort)headerBits);

        return headerLength;
    }
}