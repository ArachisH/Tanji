using Tanji.Core.Services;

namespace Tanji.Core.Habbo.Network;

public interface IHConnection<TMiddleman> where TMiddleman : IPacketMiddlemanService
{
    HNode? Local { get; }
    HNode? Remote { get; }

    Incoming? In { get; }
    Outgoing? Out { get; }

    TMiddleman? Middleman { get; }
}