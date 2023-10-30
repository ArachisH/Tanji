using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tanji.Core.Habbo.Network.Formats;

/// <summary>
/// Provides a handler used by the language compiler to format interpolated string into binary representation using specified <see cref="IHFormat"/>.
/// </summary>
[InterpolatedStringHandler]
[EditorBrowsable(EditorBrowsableState.Never)]
public ref struct IHFormatTryWriteInterpolatedStringHandler
{
    /// <summary>
    /// The destination buffer.
    /// </summary>
    private readonly Span<byte> _destination;
    /// <summary>
    /// The format used to write values into the <see cref="_destination"/>.
    /// </summary>
    private readonly IHFormat _format;
    /// <summary>
    /// The number of bytes written to <see cref="_destination"/>.
    /// </summary>
    internal int _position;
    /// <summary>
    /// true if all formatting operations have succeeded; otherwise, false.
    /// </summary>
    internal bool _success;

    /// <summary>
    /// Creates a handler used to write an interpolated string into a <paramref name="destination"/> buffer in specified <paramref name="format"/>.
    /// </summary>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="destination">The destination buffer.</param>
    /// <param name="shouldAppend">Upon return, true if the destination may be long enough to support the formatting, or false if it won't be.</param>
    /// <remarks>
    /// This is intended to be called only by compiler-generated code. Arguments are not validated as they'd otherwise be for members intended to be used directly.
    /// </remarks>
    public IHFormatTryWriteInterpolatedStringHandler(int literalLength, int formattedCount, IHFormat format, Span<byte> destination)
    {
        _format = format;
        _destination = destination;
        _success = false;
        _position = 0;
    }

    /// <summary>
    /// Writes the specified value type to the buffer.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public bool AppendFormatted<T>(T value)
        where T : unmanaged
    {
        if (_format.TryWrite(_destination.Slice(_position), value, out int bytesWritten))
        {
            _position += bytesWritten;
            return true;
        }
        return Fail();
    }

    /// <summary>
    /// Writes the specified value to the handler.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="format">The format string.</param>
    /// <typeparam name="T">The type of the value to write.</typeparam>
    public bool AppendFormatted<T>(T value, string? format = default)
        where T : IHFormattable
    {
        // constrained call avoiding boxing for value types
        if (((IHFormattable)value).TryFormat(_destination.Slice(_position), _format, out int bytesWritten, format))
        {
            _position += bytesWritten;
            return true;
        }
        return Fail();
    }

    /// <summary>
    /// Writes the specified character span to the handler.
    /// </summary>
    /// <param name="value">The span to write.</param>
    public bool AppendFormatted(ReadOnlySpan<char> value)
    {
        if (_format.TryWriteUTF8(_destination.Slice(_position), value, out int bytesWritten))
        {
            _position += bytesWritten;
            return true;
        }
        return Fail();
    }

    /// <summary>
    /// Writes the specified value to the handler.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public bool AppendFormatted(string? value) => AppendFormatted(value.AsSpan());

    /// <summary>
    /// Marks formatting as having failed and returns false.
    /// </summary>
    private bool Fail()
    {
        _success = false;
        return false;
    }
}