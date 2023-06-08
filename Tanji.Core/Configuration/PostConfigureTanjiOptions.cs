using System.Text.Json.Nodes;

using Microsoft.Extensions.Options;

namespace Tanji.Core.Configuration;

internal sealed class PostConfigureTanjiOptions : IPostConfigureOptions<TanjiOptions>
{
    public void PostConfigure(string? name, TanjiOptions options)
    {
        // If user has not defined a custom cache directory, use the default path.
        if (string.IsNullOrEmpty(options.CacheDirectory))
        {
            options.CacheDirectory = Path.GetFullPath("Cache");
        }
        options.CacheDirectory = Environment.ExpandEnvironmentVariables(options.CacheDirectory);
        Directory.CreateDirectory(options.CacheDirectory);

        // If the launcher directory is missing or doesn't exist, use the default path.
        if (string.IsNullOrEmpty(options.LauncherPath))
        {
            options.LauncherPath = "%USERPROFILE%\\AppData\\Roaming\\Habbo Launcher";
        }
        options.LauncherPath = Environment.ExpandEnvironmentVariables(options.LauncherPath);
        Directory.CreateDirectory(options.LauncherPath);

        // TODO: Very brittle logic, handle failures
        using var versionsJsonFs = File.OpenRead(Path.Combine(options.LauncherPath, "versions.json"));
        JsonNode? versionsNode = JsonNode.Parse(versionsJsonFs);

        foreach (JsonNode? installation in versionsNode?["installations"]?.AsArray() ?? new JsonArray())
        {
            if (installation?["path"] is JsonNode pathNode)
            {
                switch (installation?["client"]?.GetValue<string>())
                {
                    case "air": options.AirPath = pathNode.GetValue<string>(); break;
                    case "unity": options.UnityPath = pathNode.GetValue<string>(); break;
                }
            }
        }
    }
}
