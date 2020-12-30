using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Tanji.Services;

using Tangine.Helpers;

namespace Tanji.Controls
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public class NotifiablePage : UserControl, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Binding> _bindings;

        protected static Program Master => Program.Master;
        protected override Size DefaultSize => new Size(506, 315);

        public NotifiablePage()
        {
            _bindings = new Dictionary<string, Binding>();

            TabStop = false;
            BackColor = Color.White;
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

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Controls.Count == 0)
            {
                // Useful when debugging, so that we can see the region it occupies in the parent container.
                e.Graphics.Clear(Color.FromArgb(243, 63, 63));
            }
            base.OnPaint(e);
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