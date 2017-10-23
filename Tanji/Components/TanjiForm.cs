using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Properties;

namespace Tanji.Components
{
    public class TanjiForm : Form, INotifyPropertyChanged
    {
        private readonly Action<PropertyChangedEventArgs> _onPropertyChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaiseOnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                OnPropertyChanged(
                    new PropertyChangedEventArgs(propertyName));
            }
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(_onPropertyChanged, e);
            }
            else
            {
                PropertyChanged?.Invoke(this, e);
            }
        }

        public TanjiForm()
        {
            _onPropertyChanged = OnPropertyChanged;

            BackColor = Color.White;
            Icon = Resources.Tanji_128;
        }
    }
}