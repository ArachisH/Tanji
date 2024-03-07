namespace Tanji.Core.Habbo.Network;

public interface IHConnection
{
    HNode? Local { get; }
    HNode? Remote { get; }

    Incoming? In { get; }
    Outgoing? Out { get; }
}