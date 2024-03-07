namespace Tanji.Core.Habbo.Network.Formats;

/// <summary>
/// Provides a high-performance low-level APIs for reading and writing byte buffers.
/// </summary>
/// <remarks>
/// Due to heavy optimizations, do not trust <c>out</c> parameters in <c>Try</c>-prefixed methods when the operation is unsuccessful (when the method returns <c>false</c>). 
/// The <c>out</c> parameters contain un-initialized values when the operation is unsuccessful - this is standard behavior and can be also seen in runtime libraries.
/// </remarks>
public interface IHFormat
{
    public static EvaWireFormat EvaWire { get; } = new EvaWireFormat(isUnity: false);
    public static EvaWireFormat EvaWireUnity { get; } = new EvaWireFormat(isUnity: true);

    public static WedgieFormat WedgieIn { get; } = new WedgieFormat(isOutgoing: false);
    public static WedgieFormat WedgieOut { get; } = new WedgieFormat(isOutgoing: true);

    /// <summary>
    /// Minimum buffer size required from a packet in bytes.
    /// </summary>
    public int MinBufferSize { get; }
    /// <summary>
    /// Minimum length required from a packet in bytes.
    /// </summary>
    public int MinPacketLength { get; }
    /// <summary>
    /// Indicates whether the format has a length-prefix or not.
    /// </summary>
    public bool HasLengthIndicator { get; }

    /// <summary>
    /// Returns the amount of bytes it takes to write <paramref name="value"/> of type <typeparamref name="T"/>.
    /// </summary>
    public int GetSize<T>(T value) where T : struct;
    /// <summary>
    /// Returns the amount of bytes it takes to write <paramref name="value"/> string.
    /// </summary>
    public int GetSize(ReadOnlySpan<char> value);

    public bool TryReadLength(ReadOnlySpan<byte> source, out int length, out int bytesRead);
    public bool TryWriteLength(Span<byte> destination, int length, out int bytesWritten);

    public bool TryReadId(ReadOnlySpan<byte> source, out short id, out int bytesRead);
    public bool TryWriteId(Span<byte> destination, short id, out int bytesWritten);

    public bool TryReadHeader(ReadOnlySpan<byte> source, out int length, out short id, out int bytesRead);
    public bool TryWriteHeader(Span<byte> destination, int length, short id, out int bytesWritten);

    /// <summary>
    /// Reads a value of type <typeparamref name="T"/> from <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The source span from which <paramref name="value"/> is read.</param>
    /// <param name="value">The value upon returning <c>true</c>.</param>
    /// <param name="bytesRead">The amount of bytes read from <paramref name="source"/>.</param>
    /// <returns>true if the value was read successfully; otherwise, false.</returns>
    public bool TryRead<T>(ReadOnlySpan<byte> source, out T value, out int bytesRead) where T : struct;
    /// <summary>
    /// Writes a <paramref name="value"/> of type <typeparamref name="T"/> into <paramref name="destination"/>.
    /// </summary>
    /// <param name="destination">The destination span where <paramref name="value"/> is written.</param>
    /// <param name="value">The value to write.</param>    
    /// <param name="bytesWritten">The amount of bytes written into <paramref name="destination"/> span.</param>
    /// <returns>true if the value was written successfully; otherwise, false.</returns>
    public bool TryWrite<T>(Span<byte> destination, T value, out int bytesWritten) where T : struct;

    public bool TryReadUTF8(ReadOnlySpan<byte> source, out string value, out int bytesRead);

    public bool TryReadUTF8(ReadOnlySpan<byte> source, Span<char> destination, out int bytesRead, out int charsWritten);
    public bool TryWriteUTF8(Span<byte> destination, ReadOnlySpan<char> value, out int bytesWritten);
}