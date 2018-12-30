using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Tangine.Helpers;

using Tanji.Services;
using Tanji.Properties;

namespace Tanji.Controls
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public class ObservableForm : Form, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Binding> _bindings;

        protected Program Master => Program.Master;

        public ObservableForm()
        {
            _bindings = new Dictionary<string, Binding>();

            BackColor = Color.White;
            Icon = Resources.Tanji_256;
            StartPosition = FormStartPosition.CenterScreen;

            if (Program.Master != null && !DesignMode)
            {
                if (this is IHaltable haltable)
                {
                    Program.Master.AddHaltable(haltable);
                }
                if (this is IReceiver receiver)
                {
                    Program.Master.AddReceiver(receiver);
                }
            }
        }

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
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}