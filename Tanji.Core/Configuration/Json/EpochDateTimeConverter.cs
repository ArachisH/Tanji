using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tanji.Core.Configuration.Json;

internal sealed class EpochDateTimeConverter : JsonConverter<DateTime>
{
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateTime.UnixEpoch.AddMilliseconds(reader.GetUInt64()).ToLocalTime();
}