﻿using System.Diagnostics;

using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Services;

public interface IClientHandlerService<TGame> where TGame : IGame
{
    public DirectoryInfo PatchedClientsDirectory { get; init; }

    public TGame PatchClient(HPlatform platform, string? clientPath = null);
    public Process? LaunchClient(HPlatform platform, string ticket, string? clientPath = null);
}