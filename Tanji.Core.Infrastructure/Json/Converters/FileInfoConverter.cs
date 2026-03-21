using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tanji.Core.Infrastructure.Json.Converters;

public sealed class FileInfoConverter : JsonConverter<FileInfo>
{
    public override FileInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? fullPath = reader.GetString();

        return string.IsNullOrWhiteSpace(fullPath)
            ? throw new Exception("Failed to convert Json token into valid FileInfo object.")
            : new FileInfo(fullPath);
    }

    public override void Write(Utf8JsonWriter writer, FileInfo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}
