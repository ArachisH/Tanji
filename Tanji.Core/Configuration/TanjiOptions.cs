using System.Drawing;
using System.Text.Json.Serialization;

using Tanji.Core.Configuration.Json;

namespace Tanji.Core.Configuration;

public sealed class TanjiOptions
{
    public required IReadOnlyList<string> UnityInterceptionTriggers { get; set; }
    public required IReadOnlyList<string> FlashInterceptionTriggers { get; set; }

    public required int GameListenPort { get; set; }
    public required int ProxyListenPort { get; set; }

    public required int ModulesListenPort { get; set; }
    public required bool IsQueuingModulePackets { get; set; }

    [JsonConverter(typeof(HexColorConverter))]
    public required Color UIScheme { get; set; }

    public required bool IsCheckingForUpdates { get; set; }
    public required bool IsModifyingRetroWebResponses { get; set; }

    public string? AirPath { get; set; }
    public string? UnityPath { get; set; }
    public string? LauncherPath { get; set; }
    public string? CacheDirectory { get; set; }
    
    public required IReadOnlyList<string> ProxyOverrides { get; set; }
}
