using System.ComponentModel;

namespace Tanji.Core.Network;

public sealed class ConnectedEventArgs : CancelEventArgs
{
    public HConnectionOptions Options { get; set; }

    public ConnectedEventArgs(HConnectionOptions options)
    {
        Options = options;
    }
}