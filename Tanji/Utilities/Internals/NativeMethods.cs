using System;
using System.Runtime.InteropServices;

namespace Tanji.Utilities
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    }
}