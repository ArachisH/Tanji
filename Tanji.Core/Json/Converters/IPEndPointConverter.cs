using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tanji.Core.Json.Converters;

public sealed class IPEndPointConverter : JsonConverter<IPEndPoint>
{
    public override IPEndPoint? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return !string.IsNullOrWhiteSpace(value) ? IPEndPoint.Parse(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}