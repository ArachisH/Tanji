using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Tanji.Internals;

internal static partial class NativeMethods
{
    [LibraryImport("user32.dll", EntryPoint = "ShowCaret")]
    public static partial long ShowCaret(IntPtr hwnd);

    [LibraryImport("user32.dll", EntryPoint = "HideCaret")]
    public static partial long HideCaret(IntPtr hwnd);

    [LibraryImport("user32.dll")]
    public static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

    [LibraryImport("user32.dll")]
    public static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);
}