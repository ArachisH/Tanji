using System.Text.Json;
using System.Collections.ObjectModel;

using Microsoft.Extensions.Options;

using Tanji.Core.Json;
using Tanji.Core.Habbo.Canvas;

using CommunityToolkit.HighPerformance.Buffers;

namespace Tanji.Core.Configuration;

internal sealed class PostConfigureTanjiOptions : IPostConfigureOptions<TanjiOptions>
{
    public void PostConfigure(string? name, TanjiOptions options)
    {
        options.LauncherPath = Environment.ExpandEnvironmentVariables(options.LauncherPath);
        var versionsFileInfo = new FileInfo(Path.Combine(options.LauncherPath, "versions.json"));
        if (!versionsFileInfo.Exists) return;

        using var versionsFileBuffer = MemoryOwner<byte>.Allocate((int)versionsFileInfo.Length);
        using var versionsFileStream = File.OpenRead(versionsFileInfo.FullName);
        versionsFileStream.Read(versionsFileBuffer.Span);

        options.Versions = JsonSerializer.Deserialize<LauncherVersions>(versionsFileBuffer.Span,
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
