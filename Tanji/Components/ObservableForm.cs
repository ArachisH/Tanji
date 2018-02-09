using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Tanji.Properties;

using Sulakore.Components;

namespace Tanji.Components
{
    [ToolboxItem(false)]
    [DesignerCategory("Code")]
    public class ObservableForm : Form, INotifyPropertyChanged
    {
        public ObservableForm()
        {
            BackColor = Color.White;
            Icon = Resources.Tanji_256;
            FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        private void ApplyScheme(Control control, Color scheme)
        {
            if (control.Name == "AboutTab") return;
            if (control is ISkinnable skinnable)
            {
                skinnable.Skin = scheme;
            }
            else if (control is Label && control.Height == 1)
            {
                control.BackColor = scheme;
            }

            foreach (Control childControl in control.Controls)
            {
                ApplyScheme(childControl, scheme);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (!DesignMode)
            {
                ApplyScheme(e.Control, (Color)Program.Settings["UIScheme"]);
            }
            base.OnControlAdded(e);
        }

        protected void Bind(IBindableComponent component, string propertyName, string dataMember)
        {
            component.DataBindings.Add(propertyName, this, dataMember, false, DataSourceUpdateMode.OnPropertyChanged);
        }
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                Invoke(handler, this, e);
            }
        }
        #endregion
    }
}