using System;
using System.Windows.Forms;

namespace Tanji.Utilities
{
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys Hotkey { get; }

        public KeyPressedEventArgs(Keys hotkey)
        {
            Hotkey = hotkey;
        }
    }
}