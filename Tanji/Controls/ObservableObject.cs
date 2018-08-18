using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tanji.Controls
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected Program Master => Program.Master;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}