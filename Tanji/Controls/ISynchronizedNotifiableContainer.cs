using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Tanji.Controls
{
    public interface ISynchronizedNotifiableContainer : INotifyPropertyChanged
    {
        bool IsInDesignMode { get; }
        ContainerControl Container { get; }
        Dictionary<string, Binding> Bindings { get; }

        PropertyChangedEventHandler GetPropertyChangedEventHandler();

        event PropertyChangedEventHandler asd;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
           // findfo
            //PropertyChangedEventHandler handler = PropertyChanged;
            Container.FindForm().Invoke(GetPropertyChangedEventHandler(), this, e);
            // PropertyChanged(this, e);
            //PropertyChanged?.Invoke(this, e);
        }
        protected void RaiseOnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }


    //public class TestClass : UserControl, ISynchronizedNotifiableContainer
    //{
    //    public bool IsInDesignMode => DesignMode;
    //    public ContainerControl Container { get; }
    //    public Dictionary<string, Binding> Bindings { get; }

    //    public PropertyChangedEventHandler GetPropertyChangedEventHandler() => PropertyChanged;

    //    public event PropertyChangedEventHandler asd;
    //    public event PropertyChangedEventHandler PropertyChanged;
    //}
}