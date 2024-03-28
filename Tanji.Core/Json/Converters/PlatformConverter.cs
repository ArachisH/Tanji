using System.Text.Json;
using System.Text.Json.Serialization;

using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Json;

public sealed class PlatformConverter : JsonConverter<HPlatform>
{
    public override void Write(Utf8JsonWriter writer, HPlatform value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case HPlatform.Flash: writer.WriteStringValue("Flash"); break;
            case HPlatform.Unity: writer.WriteStringValue("Unity"); break;
        }
    }
    public override HPlatform Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => ToPlatform(reader.GetString());

    public static string ToClientName(HPlatform platform) => platform switch
    {
        HPlatform.Flash => "HabboAir.swf",
        _ => string.Empty
    };
    public static string ToExecutableName(HPlatform platform) => platform switch
    {
        HPlatform.Flash => "Habbo.exe",
        HPlatform.Unity => "habbo2020-global-prod.exe",
        _ => string.Empty
    };

    public static HPlatform ToPlatform(string? value) => value?.ToLowerInvariant() switch
    {
        "flash" or "air" => HPlatform.Flash,
        "unity" => HPlatform.Unity,
        _ => HPlatform.Unknown
    };
}