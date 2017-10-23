using Tangine.Habbo;

using Sulakore.Modules;
using Sulakore.Habbo.Web;
using Sulakore.Communication;

namespace Tangine
{
    public interface ITExtension : IModule
    {
        ITContext Context { get; }
        HTriggers Triggers { get; }

        void ModifyGame(HGame game);
        void ModifyGameData(HGameData gameData);

        void HandleOutgoing(DataInterceptedEventArgs e);
        void HandleIncoming(DataInterceptedEventArgs e);
    }
}