using System.Drawing;
using System.Text.Json.Nodes;
using System.Collections.Specialized;

namespace Tanji.Core.Services;

public sealed class ConfigurationService : NameValueCollection, IConfigurationService
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
    public string[]? ProxyOverrides { get; }

#pragma warning disable CS8600 // Ignore null-reference warnings, and allow the end user to crash if configuration file is invalid.
#pragma warning disable CS8602
#pragma warning disable CS8604
    public ConfigurationService(NameValueCollection appSettings)
        : base(appSettings)
    {
        CacheDirectory = Path.GetFullPath("Cache");
        Directory.CreateDirectory(CacheDirectory);

        UnityInterceptionTriggers = Split(this[nameof(UnityInterceptionTriggers)]);
        FlashInterceptionTriggers = Split(this[nameof(FlashInterceptionTriggers)]);

        GameListenPort = int.Parse(this[nameof(GameListenPort)]);
        ProxyListenPort = int.Parse(this[nameof(ProxyListenPort)]);

        ModulesListenPort = int.Parse(this[nameof(ModulesListenPort)]);
        IsQueuingModulePackets = Convert.ToBoolean(this[nameof(IsQueuingModulePackets)]);

        UIScheme = ColorTranslator.FromHtml(this[nameof(UIScheme)]);
        IsCheckingForUpdates = Convert.ToBoolean(this[nameof(IsCheckingForUpdates)]);
        IsModifyingRetroWebResponses = Convert.ToBoolean(this[nameof(IsModifyingRetroWebResponses)]);

        ProxyOverrides = Split(this[nameof(ProxyOverrides)]);

        LauncherPath = Environment.ExpandEnvironmentVariables(this[nameof(LauncherPath)]);
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
