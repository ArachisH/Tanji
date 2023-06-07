using System.Drawing;
using System.Text.Json.Nodes;

namespace Tanji.Core.Services;

public sealed class ConfigurationService : IConfigurationService
{
    public string[] UnityInterceptionTriggers { get; }
    public string[] FlashInterceptionTriggers { get; }

    public int GameListenPort { get; set; }
    public int ProxyListenPort { get; set; }

    public int ModulesListenPort { get; set; }
    public bool IsQueuingModulePackets { get; set; }

    public Color UIScheme { get; set; }
    public bool IsCheckingForUpdates { get; set; }
    public bool IsModifyingRetroWebResponses { get; set; }

    public string? AirPath { get; }
    public string? UnityPath { get; }
    public string? LauncherPath { get; }
    public string CacheDirectory { get; }
    public string[] ProxyOverrides { get; }

#pragma warning disable CS8600 // Ignore null-reference warnings, and allow the end user to crash if configuration file is invalid.
#pragma warning disable CS8602
#pragma warning disable CS8604
    public ConfigurationService(IConfigurationDataProviderService data)
    {
        CacheDirectory = Path.GetFullPath("Cache");
        Directory.CreateDirectory(CacheDirectory);

        UnityInterceptionTriggers = Split(data.GetValue(nameof(UnityInterceptionTriggers));
        FlashInterceptionTriggers = Split(data.GetValue(nameof(FlashInterceptionTriggers)));

        GameListenPort = int.Parse(data.GetValue(nameof(GameListenPort)));
        ProxyListenPort = int.Parse(data.GetValue(nameof(ProxyListenPort)));

        ModulesListenPort = int.Parse(data.GetValue(nameof(ModulesListenPort)));
        IsQueuingModulePackets = Convert.ToBoolean(data.GetValue(nameof(IsQueuingModulePackets)));

        UIScheme = ColorTranslator.FromHtml(data.GetValue(nameof(UIScheme)));
        IsCheckingForUpdates = Convert.ToBoolean(data.GetValue(nameof(IsCheckingForUpdates)));
        IsModifyingRetroWebResponses = Convert.ToBoolean(data.GetValue(nameof(IsModifyingRetroWebResponses)));

        ProxyOverrides = Split(data.GetValue(nameof(ProxyOverrides)));

        LauncherPath = Environment.ExpandEnvironmentVariables(data.GetValue(nameof(LauncherPath)));
        if (!Directory.Exists(LauncherPath)) return;

        string versionsJson = File.ReadAllText(Path.Combine(LauncherPath, "versions.json"));
        JsonNode versionsNode = JsonNode.Parse(versionsJson);
        foreach (JsonNode installation in versionsNode["installations"].AsArray())
        {
            switch (installation["client"].GetValue<string>())
            {
                case "air": AirPath = installation["path"].GetValue<string>(); break;
                case "unity": UnityPath = installation["path"].GetValue<string>(); break;
            }
        }
    }
#pragma warning restore

    private static string[] Split(string? value)
    {
        return !string.IsNullOrWhiteSpace(value)
        ? value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        : Array.Empty<string>();
    }
}
