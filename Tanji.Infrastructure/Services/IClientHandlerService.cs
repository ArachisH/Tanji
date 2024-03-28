using System.Diagnostics;

using Tanji.Core.Habbo;
using Tanji.Core.Habbo.Canvas;

namespace Tanji.Infrastructure.Services;

public interface IClientHandlerService
{
    DirectoryInfo MessagesDirectory { get; }
    DirectoryInfo PatchedClientsDirectory { get; }

    Task<IGame> PatchClientAsync(HPlatform platform, string? clientPath = null);
    Task<Process> LaunchClientAsync(HPlatform platform, string ticket, string? clientPath = null);

    bool TryGetIdentifiers(string? revision, out Outgoing? outgoing, out Incoming? incoming);
}