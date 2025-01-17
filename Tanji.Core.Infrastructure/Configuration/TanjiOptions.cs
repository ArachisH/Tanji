using System.Drawing;
using System.Text.Json.Serialization;

using Tanji.Core.Canvas;
using Tanji.Core.Infrastructure.Json;

namespace Tanji.Core.Infrastructure.Configuration;

public sealed class TanjiOptions
{
    public required string[] UnityInterceptionTriggers { get; init; }
    public required string[] FlashInterceptionTriggers { get; init; }

    public required ProxyProvider HttpSystemProxy { get; set; }
    public required ProxyProvider SOCKS5ClientProxy { get; set; }

    public required int GameListenPort { get; init; }
    public required int ProxyListenPort { get; init; }
    public required int ModulesListenPort { get; init; }

    public required Color UIScheme { get; init; }
    public required bool IsCheckingForUpdates { get; init; }

    public required string LauncherPath { get; set; }
    public required string[] ProxyOverrides { get; init; }

    public required bool IsUsingAirDebugLauncher { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public LauncherVersions Versions { get; internal set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public IReadOnlyDictionary<HPlatform, PlatformPaths>? PlatformPaths { get; internal set; }
}