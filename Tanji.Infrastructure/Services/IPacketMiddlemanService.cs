using Tanji.Core.Net.Interception;

namespace Tanji.Infrastructure.Services;

public interface IPacketMiddlemanService : IMiddleman
{
    bool IsRendering { get; }
    IMiddleman Renderer { get; }
}