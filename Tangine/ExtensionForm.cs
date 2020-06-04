using System;
using System.Net;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

using Tangine.Helpers;

using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;

namespace Tangine
{
    public class ExtensionForm : Form, IModule, INotifyPropertyChanged
    {
        private readonly TService _service;
        private readonly Dictionary<string, Binding> _bindings;

        public virtual bool IsStandalone { get; }
        public IInstaller Installer { get; set; }

        public Incoming In => Installer.In;
        public Outgoing Out => Installer.Out;

        public HGame Game => Installer.Game;
        public HGameData GameData => Installer.GameData;
        public IHConnection Connection => Installer.Connection;

        public ReadOnlyDictionary<int, HEntity> Entities => _service.Entities;
        public ReadOnlyDictionary<int, HWallItem> WallItems => _service.WallItems;
        public ReadOnlyDictionary<int, HFloorObject> FloorObjects => _service.FloorObjects;

        public ExtensionForm()
            : this(null)
        { }
        public ExtensionForm(IPEndPoint moduleServer)
        {
            _bindings = new Dictionary<string, Binding>();
            _service = new TService(this, moduleServer);
        }

        void IModule.OnConnected()
        {
            OnConnected();
            _service.OnConnected();
        }
        public virtual void OnConnected()
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

        protected void Bind(IBindableComponent component, string propertyName, string dataMember, IValueConverter converter = null, DataSourceUpdateMode dataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged)
        {
            var binding = new CustomBinding(propertyName, this, dataMember, converter)
            {
                DataSourceUpdateMode = dataSourceUpdateMode,
                ControlUpdateMode = ControlUpdateMode.OnPropertyChanged
            };
            component.DataBindings.Add(binding);
            _bindings[dataMember] = binding;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (Owner != null)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2, Owner.Location.Y + Owner.Height / 2 - Height / 2);
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                FindForm()?.Invoke(handler, this, e);
            }
            if (DesignMode)
            {
                _bindings[e.PropertyName].ReadValue();
            }
        }
        protected void RaiseOnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}