using Tanji.Core.Canvas;

namespace Tanji.Infrastructure.Configuration;

public readonly record struct PlatformPaths
{
    public required HPlatform Platform { get; init; }

    public readonly string RootPath { get; init; }
    public required string ClientPath { get; init; }
    public required string ExecutablePath { get; init; }
}