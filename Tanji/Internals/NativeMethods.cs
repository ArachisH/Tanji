using System;
using System.Runtime.InteropServices;

namespace Tanji.Internals;

internal static partial class NativeMethods
{
    [LibraryImport("user32.dll", EntryPoint = "ShowCaret", SetLastError = true)]
    public static partial long ShowCaret(IntPtr hwnd);

    [LibraryImport("user32.dll", EntryPoint = "HideCaret", SetLastError = true)]
    public static partial long HideCaret(IntPtr hwnd);

    [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
    public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);
}