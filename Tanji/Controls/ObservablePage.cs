using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Tanji.Helpers;
using Tanji.Services;

namespace Tanji.Controls
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public class ObservablePage : UserControl, INotifyPropertyChanged
    {
        private readonly Dictionary<string, Binding> _bindings;

        protected Program Master => Program.Master;
        protected override Size DefaultSize => new Size(486, 348);

        public ObservablePage()
        {
            _bindings = new Dictionary<string, Binding>();

            BackColor = Color.White;
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
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}