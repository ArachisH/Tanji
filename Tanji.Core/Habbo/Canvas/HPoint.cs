using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Canvas;

/// <summary>
/// Represents a three-tuple of x-, y-, and z-coordinates that define a point in a three-dimensional space.
/// </summary>
public struct HPoint : IEquatable<HPoint>,
    ISpanFormattable, ISpanParsable<HPoint>,
    IAdditiveIdentity<HPoint, HPoint>,
    IAdditionOperators<HPoint, HPoint, HPoint>,
    ISubtractionOperators<HPoint, HPoint, HPoint>,
    IEqualityOperators<HPoint, HPoint, bool>
{
    public const float DEFAULT_EPSILON = 0.01f;

    private static readonly HPoint _origin = new();
    public static ref readonly HPoint Origin => ref _origin;

    static HPoint IAdditiveIdentity<HPoint, HPoint>.AdditiveIdentity => Origin;

    public int X { readonly get; set; }
    public int Y { readonly get; set; }
    public float Z { readonly get; set; }

    public HPoint()
        : this(0, 0, 0f)
    { }
    public HPoint(int x, int y)
        : this(x, y, 0f)
    { }
    public HPoint(int x, int y, char level)
        : this(x, y, ToZ(level))
    { }
    public HPoint(int x, int y, float z)
        => (X, Y, Z) = (x, y, z);

    public readonly void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
    public readonly void Deconstruct(out int x, out int y, out float z) => (x, y, z) = (X, Y, Z);

    public override readonly string ToString()
        => string.Create(CultureInfo.InvariantCulture, stackalloc char[64], $"{X}, {Y}, {Z}");

    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);
    public override readonly bool Equals(object? obj) => obj switch
    {
        HPoint other => Equals(other),
        ValueTuple<int, int> xy => Equals(xy),
        ValueTuple<int, int, float> xyx => Equals(xyx),
        _ => false
    };

    public readonly bool Equals(HPoint other)
        => X == other.X && Y == other.Y;
    public readonly bool Equals(HPoint other, float epsilon = DEFAULT_EPSILON)
        => X == other.X && Y == other.Y && Math.Abs(other.Z - Z) < epsilon;

    public readonly bool Equals((int X, int Y) point)
        => X == point.X && Y == point.Y;
    public readonly bool Equals((int X, int Y, int Z) point, float epsilon = DEFAULT_EPSILON)
        => X == point.X && Y == point.Y && Math.Abs(point.Z - Z) < epsilon;
    public readonly bool Equals((int X, int Y, float Z) point, float epsilon = DEFAULT_EPSILON)
        => X == point.X && Y == point.Y && Math.Abs(point.Z - Z) < epsilon;

    public readonly bool TryFormat(Span<byte> destination, IHFormat format, out int bytesWritten, ReadOnlySpan<char> formatString)
        => destination.TryWrite(format, $"{X}{Y}", out bytesWritten);

    public static bool operator !=(HPoint left, HPoint right) => !(left == right);
    public static bool operator ==(HPoint left, HPoint right) => left.Equals(right);

    public static implicit operator Point(HPoint point) => new(point.X, point.Y);
    public static implicit operator HPoint(Point point) => new(point.X, point.Y);

    public static implicit operator Vector2(HPoint point) => new(point.X, point.Y);
    public static implicit operator Vector3(HPoint point) => new(point.X, point.Y, point.Z);

    public static implicit operator HPoint((int X, int Y) point) => new(point.X, point.Y);
    public static implicit operator HPoint((int X, int Y, float Z) point) => new(point.X, point.Y, point.Z);

    public static HPoint operator +(HPoint point, (int X, int Y) offset) => new(point.X + offset.X, point.Y + offset.Y);
    public static HPoint operator +(HPoint point, (int X, int Y, float Z) offset) => new(point.X + offset.X, point.Y + offset.Y, point.Z + offset.Z);

    public static HPoint operator -(HPoint point, (int X, int Y) offset) => new(point.X - offset.X, point.Y - offset.Y);
    public static HPoint operator -(HPoint point, (int X, int Y, float Z) offset) => new(point.X - offset.X, point.Y - offset.Y, point.Z - offset.Z);

    public static HPoint operator +(HPoint left, HPoint right) => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    public static HPoint operator -(HPoint left, HPoint right) => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

    public static char ToLevel(float z) => z switch
    {
        >= 0 and <= 9 => (char)(z + 48),
        >= 10 and <= 29 => (char)(z + 87),
        _ => 'x'
    };
    public static float ToZ(char level) => level switch
    {
        >= '0' and <= '9' => level - 48,
        >= 'a' and <= 't' => level - 87,
        _ => 0
    };

    /// <summary>
    /// Parses a comma-separated two- or three-tuple as <see cref="HPoint"/> value.
    /// </summary>
    public static HPoint Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (!TryParse(s, out HPoint result))
            ThrowHelper.ThrowFormatException();
        return result;
    }
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static HPoint Parse(string s, IFormatProvider? provider = null) => Parse(s.AsSpan());

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static bool TryParse(ReadOnlySpan<char> s, out HPoint result)
    {
        Unsafe.SkipInit(out result);

        // Try parse an integer before first comma.
        int separatorIndex = s.IndexOf(',');
        if (separatorIndex <= 0 ||
            !int.TryParse(s.Slice(0, separatorIndex), NumberStyles.Integer, CultureInfo.InvariantCulture, out int x))
        {
            return false;
        }

        // Slice out the parsed X-coordinate and the first comma.
        s = s.Slice(separatorIndex + 1);

        // After the first comma, parse the Y-coordinate as an integer.
        // If there's still one more comma in the input,
        // try to parse the Z-coordinate as a floating-point number.

        int y;
        float z;

        separatorIndex = s.IndexOf(',');
        if (separatorIndex == -1)
        {
            if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out y))
            {
                result = new HPoint(x, y);
                return true;
            }
        }
        else if (int.TryParse(s.Slice(0, separatorIndex), NumberStyles.Integer, CultureInfo.InvariantCulture, out y) &&
            float.TryParse(s.Slice(separatorIndex + 1), NumberStyles.Float, CultureInfo.InvariantCulture, out z))
        {
            result = new HPoint(x, y, z);
            return true;
        }

        return false;
    }
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    public static bool TryParse([NotNullWhen(true)] string? s, out HPoint result) => TryParse(s.AsSpan(), out result);

    readonly bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => destination.TryWrite(CultureInfo.InvariantCulture, $"{X}, {Y}, {Z}", out charsWritten);
    readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();

    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    static bool ISpanParsable<HPoint>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out HPoint result) => TryParse(s, out result);
    /// <inheritdoc cref="Parse(ReadOnlySpan{char}, IFormatProvider?)"/>
    static bool IParsable<HPoint>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out HPoint result) => TryParse(s.AsSpan(), out result);
}