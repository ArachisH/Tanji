using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tanji.Controls
{
    [DesignerCategory("Code")]
    public class RichLogBox : RichTextBox
    {
        private bool _isHidingCaret = true;
        [DefaultValue(true)]
        public bool IsHidingCaret
        {
            get => _isHidingCaret;
            set
            {
                _isHidingCaret = true;
                Invalidate();
            }
        }

        public IDisposable GetSuspender()
        {
            return new RichLogBoxPaintSuspender(this);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (IsHidingCaret)
            {
                NativeMethods.HideCaret(Handle);
            }
        }

        private class RichLogBoxPaintSuspender : IDisposable
        {
            private Point _scrollPoint;

            private readonly int _suspendIndex;
            private readonly IntPtr _eventMask;
            private readonly int _suspendLength;
            private readonly RichLogBox _logBox;

            private const int WM_USER = 0x400;
            private const int WM_SETREDRAW = 0x000B;
            private const int EM_GETEVENTMASK = WM_USER + 59;
            private const int EM_SETEVENTMASK = WM_USER + 69;
            private const int EM_GETSCROLLPOS = WM_USER + 221;
            private const int EM_SETSCROLLPOS = WM_USER + 222;

            public RichLogBoxPaintSuspender(RichLogBox logBox)
            {
                _logBox = logBox;

                _suspendIndex = _logBox.SelectionStart;
                _suspendLength = _logBox.SelectionLength;

                _scrollPoint = Point.Empty;
                NativeMethods.SendMessage(_logBox.Handle, EM_GETSCROLLPOS, 0, ref _scrollPoint);

                NativeMethods.SendMessage(_logBox.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                _eventMask = NativeMethods.SendMessage(_logBox.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            }

            public void Dispose()
            {
                _logBox.Select(_suspendIndex, _suspendLength);
                NativeMethods.SendMessage(_logBox.Handle, EM_SETSCROLLPOS, 0, ref _scrollPoint);
                NativeMethods.SendMessage(_logBox.Handle, EM_SETEVENTMASK, 0, _eventMask);
                NativeMethods.SendMessage(_logBox.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            }
        }
    }
}