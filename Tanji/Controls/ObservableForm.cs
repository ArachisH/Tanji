using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Tanji.Helpers;
using Tanji.Services;
using Tanji.Properties;

namespace Tanji.Controls
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public class ObservableForm : Form, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Binding> _bindings;

        public ObservableForm()
        {
            _bindings = new Dictionary<string, Binding>();

            BackColor = Color.White;
            Icon = Resources.Tanji_256;
            StartPosition = FormStartPosition.CenterScreen;

            if (Program.Master != null)
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

        protected void Bind(IBindableComponent component, string sourceName, IValueConverter converter = null)
        {
            string targetName = null;
            switch (component)
            {
                case TextBox textBox:
                {
                    targetName = "Text";
                    break;
                }
                case CheckBox checkBox:
                {
                    targetName = "Checked";
                    break;
                }
                case NumericUpDown numericUpDown:
                {
                    targetName = "Value";
                    break;
                }
            }
            if (string.IsNullOrWhiteSpace(targetName))
            {
                throw new ArgumentException("Unable to find the appropriate target name for the given component.", nameof(component));
            }
            Bind(component, targetName, sourceName, converter);
        }
        protected void Bind(IBindableComponent component, string targetName, string sourceName, IValueConverter converter = null)
        {
            var binding = new CustomBinding(targetName, this, sourceName, converter);
            component.DataBindings.Add(binding);
            _bindings[sourceName] = binding;
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