using System;

using Sulakore.Habbo;
using Sulakore.Habbo.Web;
using Sulakore.Communication;

namespace Sulakore.Modules
{
    public interface IModule : IDisposable
    {
        HTriggers Triggers { get; }
        IInstaller Installer { get; set; }

        void ModifyGame(HGame game);
        void ModifyGameData(HGameData gameData);

        void HandleOutgoing(DataInterceptedEventArgs e);
        void HandleIncoming(DataInterceptedEventArgs e);
    }
}