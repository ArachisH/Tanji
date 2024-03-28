using System.Text.Json;
using System.Collections.ObjectModel;

using Tanji.Core.Json;
using Tanji.Core.Canvas;

using Microsoft.Extensions.Options;

namespace Tanji.Infrastructure.Configuration;

internal sealed class PostConfigureTanjiOptions : IPostConfigureOptions<TanjiOptions>
{
    public void PostConfigure(string? name, TanjiOptions options)
    {
        options.LauncherPath = Environment.ExpandEnvironmentVariables(options.LauncherPath);
        var versionsFileInfo = new FileInfo(Path.Combine(options.LauncherPath, "versions.json"));
        if (!versionsFileInfo.Exists) return;

        options.AirDebugLauncherFilePath = Environment.ExpandEnvironmentVariables(options.AirDebugLauncherFilePath);

        using var versionsFileStream = File.OpenRead(versionsFileInfo.FullName);
        options.Versions = JsonSerializer.Deserialize<LauncherVersions>(versionsFileStream,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        if (options.Versions == default) return;

        var platformPaths = new Dictionary<HPlatform, PlatformPaths>();
        options.PlatformPaths = new ReadOnlyDictionary<HPlatform, PlatformPaths>(platformPaths);

        foreach (Installation installation in options.Versions.Installations)
        {
            platformPaths.Add(installation.Platform, new PlatformPaths
            {
                Platform = installation.Platform,

                RootPath = installation.Path,
                ClientPath = Path.Combine(installation.Path, PlatformConverter.ToClientName(installation.Platform)),
                ExecutablePath = Path.Combine(installation.Path, PlatformConverter.ToExecutableName(installation.Platform))
            });
        }
    }
}
