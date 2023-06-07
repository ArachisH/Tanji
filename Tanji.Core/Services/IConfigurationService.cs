using System.Drawing;

namespace Tanji.Core.Services;

public interface IConfigurationService
{
    string[] UnityInterceptionTriggers { get; }
    string[] FlashInterceptionTriggers { get; }

    int GameListenPort { get; set; }
    int ProxyListenPort { get; set; }

    int ModulesListenPort { get; set; }
    bool IsQueuingModulePackets { get; set; }

    Color UIScheme { get; set; }
    bool IsCheckingForUpdates { get; set; }
    bool IsModifyingRetroWebResponses { get; set; }

    string? AirPath { get; }
    string? UnityPath { get; }
    string? LauncherPath { get; }
    string CacheDirectory { get; }
    string[] ProxyOverrides { get; }
}