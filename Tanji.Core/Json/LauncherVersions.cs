using System.Text.Json.Serialization;

using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Json;

public readonly record struct PlatformCheck
{
    public required string Version { get; init; }
    public required string Path { get; init; }
}
public readonly record struct Installation
{
    public required string Version { get; init; }
    public required string Path { get; init; }

    [JsonPropertyName("client")]
    [JsonConverter(typeof(PlatformConverter))]
    public required HPlatform Platform { get; init; }

    [JsonConverter(typeof(EpochDateTimeConverter))]
    public required DateTime LastModified { get; init; }
}
public readonly record struct UpdateCheck
{
    public required PlatformCheck Unity { get; init; }
    public required PlatformCheck Air { get; init; }

    [JsonConverter(typeof(EpochDateTimeConverter))]
    public required DateTime Time { get; init; }
}
public readonly record struct LauncherVersions
{
    public required Installation[] Installations { get; init; }
    public required UpdateCheck LastCheck { get; init; }
}