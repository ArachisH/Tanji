using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Internals;

namespace Tanji.Controls;

public sealed class TanjiPacketViewer : RichTextBox
{
    private IDisposable? _renderJob;

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

        ForeColor = Color.White;
        ShowSelectionMargin = true;
        BorderStyle = BorderStyle.None;
        Font = new Font("Consolas", 11F);
        BackColor = Color.FromArgb(22, 22, 22);
        ScrollBars = RichTextBoxScrollBars.ForcedVertical;
    }

    public IDisposable FreezeCaret() => _renderJob ??= new TanjiPacketFreezeCaretJob(this);

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (IsHidingCaret)
        {
            NativeMethods.HideCaret(Handle);
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _renderJob?.Dispose();
        }
        base.Dispose(disposing);
    }

    private sealed class TanjiPacketFreezeCaretJob : IDisposable
    {
        private readonly TanjiPacketViewer _viewer;

        private Point _scrollPoint;
        private readonly IntPtr _eventMask;
        private readonly int _suspendIndex, _suspendLength;

        private const int WM_USER = 0x400;
        private const int WM_SETREDRAW = 0x000B;
        private const int EM_GETEVENTMASK = WM_USER + 59;
        private const int EM_SETEVENTMASK = WM_USER + 69;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;

        public TanjiPacketFreezeCaretJob(TanjiPacketViewer viewer)
        {
            _viewer = viewer;

            _suspendIndex = viewer.SelectionStart;
            _suspendLength = viewer.SelectionLength;

            _scrollPoint = Point.Empty;
            NativeMethods.SendMessage(viewer.Handle, EM_GETSCROLLPOS, 0, ref _scrollPoint);
            NativeMethods.SendMessage(viewer.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            _eventMask = NativeMethods.SendMessage(viewer.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
        }

        void IDisposable.Dispose()
        {
            _viewer.Select(_suspendIndex, _suspendLength);
            NativeMethods.SendMessage(_viewer.Handle, EM_SETSCROLLPOS, 0, ref _scrollPoint);
            NativeMethods.SendMessage(_viewer.Handle, EM_SETEVENTMASK, 0, _eventMask);
            NativeMethods.SendMessage(_viewer.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        }
    }
}