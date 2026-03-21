using System.Text.Json;
using System.Collections.ObjectModel;

using Tanji.Core.Canvas;
using Tanji.Core.Infrastructure.Json;
using Tanji.Core.Infrastructure.Json.Converters;

using Microsoft.Extensions.Options;

namespace Tanji.Core.Infrastructure.Configuration;

internal sealed class PostConfigureTanjiOptions : IPostConfigureOptions<TanjiOptions>
{
    private static readonly JsonSerializerOptions _options;

    static PostConfigureTanjiOptions()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public void PostConfigure(string? name, TanjiOptions options)
    {
        options.LauncherPath = Environment.ExpandEnvironmentVariables(options.LauncherPath);
        var versionsFileInfo = new FileInfo(Path.Combine(options.LauncherPath, "versions.json"));
        if (!versionsFileInfo.Exists) return;

        using var versionsFileStream = File.OpenRead(versionsFileInfo.FullName);
        options.Versions = JsonSerializer.Deserialize<LauncherVersions>(versionsFileStream, _options);
        if (options.Versions == default) return;

        var platformPaths = new Dictionary<HPlatform, PlatformPaths>();
        options.PlatformPaths = new ReadOnlyDictionary<HPlatform, PlatformPaths>(platformPaths);

        foreach (Installation installation in options.Versions.Installations)
        {
            if (platformPaths.ContainsKey(installation.Platform))
            {
                if (platformPaths[installation.Platform].Version < int.Parse(installation.Version))
                {
                    platformPaths.Remove(installation.Platform);
                }
                else
                {
                    continue;
                }
            }

            platformPaths.Add(installation.Platform, new PlatformPaths
            {
                Platform = installation.Platform,
                RootPath = installation.Path,
                ClientPath = Path.Combine(installation.Path, PlatformConverter.ToClientName(installation.Platform)),
                ExecutablePath = Path.Combine(installation.Path, PlatformConverter.ToExecutableName(installation.Platform)),
                Version = int.Parse(installation.Version)
            });
        }
    }
}
