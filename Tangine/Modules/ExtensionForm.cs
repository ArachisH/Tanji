using System.Windows.Forms;

using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;

namespace Tangine.Modules
{
    public class ExtensionForm : Form, IModule
    {
        private readonly TService _service;

        public virtual bool IsStandalone { get; }
        public IInstaller Installer { get; set; }

        public Incoming In => Installer.In;
        public Outgoing Out => Installer.Out;

        public HGame Game => Installer.Game;
        public HGameData GameData => Installer.GameData;
        public IHConnection Connection => Installer.Connection;

        public ExtensionForm()
        {
            _service = new TService(this);
        }

        void IModule.Synchronize(HGame game)
        {
            Synchronize(game);
            _service.Synchronize(game);
        }
        public virtual void Synchronize(HGame game)
        { }

        void IModule.Synchronize(HGameData gameData)
        {
            Synchronize(gameData);
            _service.Synchronize(gameData);
        }
        public virtual void Synchronize(HGameData gameData)
        { }

        void IModule.HandleOutgoing(DataInterceptedEventArgs e)
        {
            HandleOutgoing(e);
            _service.HandleOutgoing(e);
        }
        public virtual void HandleOutgoing(DataInterceptedEventArgs e)
        { }

        void IModule.HandleIncoming(DataInterceptedEventArgs e)
        {
            HandleIncoming(e);
            _service.HandleIncoming(e);
        }
        public virtual void HandleIncoming(DataInterceptedEventArgs e)
        { }
    }
}