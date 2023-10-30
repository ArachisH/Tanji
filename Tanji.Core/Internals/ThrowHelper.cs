using System.Diagnostics.CodeAnalysis;

namespace Tanji.Core;
internal static class ThrowHelper
{
    [DoesNotReturn]
    internal static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();

    [DoesNotReturn]
    internal static void ThrowArgumentOutOfRangeException(string? message, string? paramName, object? value) => throw new ArgumentOutOfRangeException(paramName, value, message);

    [DoesNotReturn]
    internal static void ThrowArgumentException(string? message, string? paramName) => throw new ArgumentException(message, paramName);

    [DoesNotReturn]
    internal static void ThrowInvalidOperationException(string? message) => throw new InvalidOperationException(message);

    [DoesNotReturn]
    internal static void ThrowNullReferenceException(string? message = null) => throw new NullReferenceException(message);

    [DoesNotReturn]
    internal static void ThrowFormatException(string? message = null) => throw new FormatException(message);

    [DoesNotReturn]
    internal static void ThrowNotSupportedException(string? message = null) => throw new NotSupportedException(message);

    [DoesNotReturn]
    internal static void ThrowArgumentNullException(string? paramName) => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    internal static void ThrowObjectDisposedException(string? message) => throw new ObjectDisposedException(message);

    [DoesNotReturn]
    internal static void ThrowFileNotFoundException(string? message, string? fileName) => throw new FileNotFoundException(message, fileName);
}