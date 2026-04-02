using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Internals;

namespace Tanji.Controls;

public sealed class TanjiPacketViewer : RichTextBox
{
    private IntPtr _eventMask;
    private int _suspendIndex, _suspendLength;

    private const int WM_USER = 0x400;
    private const int WM_VSCROLL = 277;
    private const int WM_SETREDRAW = 0x000B;

    private const int SB_PAGEBOTTOM = 7;
    private const int EM_GETEVENTMASK = WM_USER + 59;
    private const int EM_SETEVENTMASK = WM_USER + 69;

    [DefaultValue(true)]
    public bool IsHidingCaret
    {
        get => field;
        set
        {
            field = value;
            Invalidate();
        }
    }

    public TanjiPacketViewer()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

        IsHidingCaret = true;
        HideSelection = true;
        ForeColor = Color.White;
        ShowSelectionMargin = true;
        BorderStyle = BorderStyle.None;
        Font = new Font("Consolas", 11F);
        BackColor = Color.FromArgb(22, 22, 22);
        ScrollBars = RichTextBoxScrollBars.ForcedVertical;
    }

    public void ScrollToBottom()
    {
        NativeMethods.SendMessage(Handle, WM_VSCROLL, SB_PAGEBOTTOM, IntPtr.Zero);
    }

    public void ResumePaint()
    {
        Select(_suspendIndex, _suspendLength);
        NativeMethods.SendMessage(Handle, EM_SETEVENTMASK, 0, _eventMask);
        NativeMethods.SendMessage(Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        Invalidate();
    }
    public void SuspendPaint()
    {
        _suspendIndex = SelectionStart;
        _suspendLength = SelectionLength;

        NativeMethods.SendMessage(Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        _eventMask = NativeMethods.SendMessage(Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
    }

    [DebuggerStepThrough]
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (IsHidingCaret)
        {
            NativeMethods.HideCaret(Handle);
        }
    }
}