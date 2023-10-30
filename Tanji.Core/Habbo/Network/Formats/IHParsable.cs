using System.Diagnostics.CodeAnalysis;

namespace Tanji.Core.Habbo.Network.Formats;

/// <summary>
/// Defines a mechanism for parsing a binary representation of a value.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface.</typeparam>
public interface IHParsable<TSelf> where TSelf : IHParsable<TSelf>?
{
    /// <summary>Parses a span of bytes into a value.</summary>
    /// <param name="source">The span of bytes to parse.</param>
    /// <param name="format">The protocol format which is used to parse the value from <paramref name="source" />.</param>
    /// <returns>The result of parsing <paramref name="s" />.</returns>
    static abstract TSelf Parse(ReadOnlySpan<byte> source, IHFormat format);

    /// <summary>Tries to parse a span of bytes into a value.</summary>
    /// <param name="result">On return, contains the result of successfully parsing <paramref name="source" /> or an undefined value on failure.</param>
    /// <returns><c>true</c> if <paramref name="source" /> was successfully parsed; otherwise, <c>false</c>.</returns>
    /// <inheritdoc cref="Parse(ReadOnlySpan{byte}, IHFormat)"/>
    static abstract bool TryParse(ReadOnlySpan<byte> source, IHFormat format, [MaybeNullWhen(returnValue: false)] out TSelf result);
}