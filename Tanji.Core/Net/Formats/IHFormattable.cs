namespace Tanji.Core.Net.Formats;

/// <summary>
/// Provides functionality to format the binary representation of an object into a <see cref="Span{byte}"/>.
/// </summary>
public interface IHFormattable
{
    /// <summary>
    /// Tries to format the value of current instance into the provided span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, this instance's value formatted into bytes.</param>
    /// <param name="format">The protocol format in which the current instance is written to.</param>
    /// <param name="bytesWritten">When this method returns, the number of bytes that were written in destination.</param>
    /// <param name="formatString">
    /// A span containing the characters that represent a custom format string that defines the acceptable format for destination.
    /// </param>
    /// <returns>true if the formatting was successful; otherwise, false.</returns>
    bool TryFormat(Span<byte> destination, IHFormat format, out int bytesWritten, ReadOnlySpan<char> formatString);
}