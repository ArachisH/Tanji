using System.ComponentModel;

namespace Tanji.Services
{
    public interface IHaltable : ISynchronizeInvoke
    {
        void Halt();
        void Restore();
    }
}