using System;

namespace Sulakore.Modules
{
    public interface IModule : IDisposable
    {
        IContractor Installer { get; }
    }
}