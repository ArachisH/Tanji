using System.Drawing;
using System.Text.Json.Serialization;

using Tanji.Core.Habbo.Canvas;
using Tanji.Core.Configuration.Json;

namespace Tanji.Core.Configuration;

public readonly record struct PlatformPaths
{
    public required HPlatform Platform { get; init; }

    public readonly string RootPath { get; init; }
    public required string ClientPath { get; init; }
    public required string ExecutablePath { get; init; }
}

public sealed class TanjiOptions
{
    public required string[] UnityInterceptionTriggers { get; init; }
    public required string[] FlashInterceptionTriggers { get; init; }

    public required int GameListenPort { get; init; }
    public required int ProxyListenPort { get; init; }
    public required int ModulesListenPort { get; init; }

    [JsonConverter(typeof(HexColorConverter))]
    public required Color UIScheme { get; init; }
    public required bool IsCheckingForUpdates { get; init; }

    public required string LauncherPath { get; set; }
    public required string[] ProxyOverrides { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public LauncherVersions Versions { get; internal set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public IReadOnlyDictionary<HPlatform, PlatformPaths>? PlatformPaths { get; internal set; }
}