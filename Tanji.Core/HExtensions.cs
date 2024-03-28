using System.Globalization;

using Tanji.Core.Canvas;

namespace Tanji.Core;

public static class HExtensions
{
    public static HSign GetRandomSign() => (HSign)Random.Shared.Next(0, 19);
    public static HTheme GetRandomTheme() => (HTheme)(Random.Shared.Next(3, 9) & 7);

    public static HDirection ToLeft(this HDirection facing)
    {
        return (HDirection)((int)facing - 1 & 7);
    }
    public static HDirection ToRight(this HDirection facing)
    {
        return (HDirection)((int)facing + 1 & 7);
    }

    public static HHotel ToHotel(this ReadOnlySpan<char> value)
    {
        if (value.Length < 2) return HHotel.Unknown;

        if (value.Length >= 4 && value.StartsWith("hh", StringComparison.OrdinalIgnoreCase)) value = value.Slice(2, 2); // hhxx
        else if (value.Length >= 7 && value.StartsWith("game-", StringComparison.OrdinalIgnoreCase)) value = value.Slice(5, 2); // game-xx

        if (value.Length != 2 && value.Length != 5)
        {
            // Slice to the domain
            int hostIndex = value.LastIndexOf("habbo", StringComparison.OrdinalIgnoreCase);
            if (hostIndex != -1)
                value = value.Slice(hostIndex + "habbo".Length);

            // Is second-level .com TLD
            int comDotIndex = value.IndexOf("com.", StringComparison.OrdinalIgnoreCase);
            if (comDotIndex != -1)
                value = value.Slice(comDotIndex + "com.".Length);

            // Corner-case where value was domain including the dot
            if (value[0] == '.') value = value.Slice(1);

            if (value.StartsWith("com", StringComparison.OrdinalIgnoreCase))
                return HHotel.US;

            // Slice out rest of value
            value = value.Slice(0, 2);
        }

        if (TryParse(value, StringComparison.OrdinalIgnoreCase, out HHotel hotel, allowNumeric: false))
            return hotel;

        return HHotel.Unknown;
    }

    public static string ToRegion(this HHotel hotel)
        => hotel.ToString().ToLowerInvariant();
    public static string ToDomain(this HHotel hotel)
        => hotel switch
        {
            HHotel.TR => "com.tr",
            HHotel.BR => "com.br",
            HHotel.Unknown => throw new ArgumentException($"Hotel cannot be '{nameof(HHotel.Unknown)}'.", nameof(hotel)),

            _ => hotel.ToString().ToLowerInvariant()
        };
    public static Uri ToUri(this HHotel hotel, string subdomain = "www")
    {
        return new Uri($"https://{subdomain}.habbo.{hotel.ToDomain()}");
    }

    /// <summary>
    /// Internal reflection-free helper parser for <see cref="HHotel"/>.
    /// </summary>
    /// <param name="value">The span representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="comparisonType">The string comparison type used to compare enums values.</param>
    /// <param name="allowNumeric">Attempt to parse input value as enums underlying integer representation.</param>
    /// <param name="checkIsDefined">Checks whether the enum parsed from integer input is defined. Only applies if <paramref name="allowNumeric"/> is true.</param>
    /// <returns></returns>
    internal static bool TryParse(ReadOnlySpan<char> value,
        StringComparison comparisonType,
        out HHotel result,
        bool allowNumeric = true,
        bool checkIsDefined = false)
    {
        result = default;
        if (value.IsEmpty) return false;

        if (value.Equals(nameof(HHotel.US), comparisonType)) result = HHotel.US;
        else if (value.Equals(nameof(HHotel.FI), comparisonType)) result = HHotel.FI;
        else if (value.Equals(nameof(HHotel.ES), comparisonType)) result = HHotel.ES;
        else if (value.Equals(nameof(HHotel.IT), comparisonType)) result = HHotel.IT;
        else if (value.Equals(nameof(HHotel.NL), comparisonType)) result = HHotel.NL;
        else if (value.Equals(nameof(HHotel.DE), comparisonType)) result = HHotel.DE;
        else if (value.Equals(nameof(HHotel.FR), comparisonType)) result = HHotel.FR;
        else if (value.Equals(nameof(HHotel.BR), comparisonType)) result = HHotel.BR;
        else if (value.Equals(nameof(HHotel.TR), comparisonType)) result = HHotel.TR;

        if (result == default && allowNumeric)
        {
            if (allowNumeric &&
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
            {
                if (checkIsDefined && intValue is < 0 or > 9) return false;
                else result = (HHotel)intValue;
            }
            else return false;
        }
        return true;
    }
}