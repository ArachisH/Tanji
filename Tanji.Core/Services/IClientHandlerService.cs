using System.Diagnostics;

using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Canvas;

namespace Tanji.Core.Services;

public interface IClientHandlerService
{
    DirectoryInfo PatchedClientsDirectory { get; }

    IGame PatchClient(HPlatform platform, string? clientPath = null);
    public Process? LaunchClient(HPlatform platform, string ticket, string? clientPath = null);
    bool TryGetIdentifiers(string? revision, out Outgoing? outgoing, out Incoming? incoming);
}