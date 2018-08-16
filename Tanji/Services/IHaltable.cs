using System.ComponentModel;

using Tanji.Network;

namespace Tanji.Services
{
    public interface IHaltable : ISynchronizeInvoke
    {
        void Halt();
        void Restore(ConnectedEventArgs e);
    }
}