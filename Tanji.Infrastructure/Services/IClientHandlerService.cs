using System.Diagnostics;

using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Canvas;

namespace Tanji.Infrastructure.Services;

public interface IClientHandlerService
{
    DirectoryInfo PatchedClientsDirectory { get; }

    IGame PatchClient(HPlatform platform, string? clientPath = null);
    Process? LaunchClient(HPlatform platform, string ticket, string? clientPath = null);
    bool TryGetIdentifiers(string? revision, out Outgoing? outgoing, out Incoming? incoming);
}