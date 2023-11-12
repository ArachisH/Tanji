using System.Drawing;
using System.Text.Json;
using System.Buffers.Text;
using System.Text.Json.Serialization;

namespace Tanji.Core.Json;

internal sealed class HexColorConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse '#'-prefixed hex string as an integer. Expected format is "#RRGGBB".
        if (reader.TokenType == JsonTokenType.String &&
            reader.ValueSpan.Length == 7 &&
            reader.ValueSpan[0] == '#' &&
            Utf8Parser.TryParse(reader.ValueSpan.Slice(1, 6), out int value, out _, 'x'))
        {
            // Mask alpha to 0xFF
            return Color.FromArgb(value | 0xFF << 24);
        }
        else return default;
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        => throw new NotImplementedException();
}