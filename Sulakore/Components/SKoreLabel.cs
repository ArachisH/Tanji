using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreLabel : Control
    {
        private int _borderWidth = 1;
        [DefaultValue(1)]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                Invalidate();
            }
        }

        private bool _isBorderVisible = true;
        [DefaultValue(true)]
        public bool IsBorderVisible
        {
            get => _isBorderVisible;
            set
            {
                _isBorderVisible = value;
                Invalidate();
            }
        }

        [Localizable(true)]
        [DefaultValue(typeof(Size), "150, 20")]
        public new Size Size
        {
            get => base.Size;
            set => base.Size = value;
        }

        private Color _skin = Color.FromArgb(243, 63, 63);
        [DefaultValue(typeof(Color), "243, 63, 63")]
        public Color Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        public SKoreLabel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;

            Height = 20;
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (IsBorderVisible)
            {
                using (var brush = new SolidBrush(Skin))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, BorderWidth, Height);
                    e.Graphics.FillRectangle(brush, Width - BorderWidth, 0, BorderWidth, Height);
                }
            }
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor);
            base.OnPaint(e);
        }

        [Obsolete]
        [Browsable(false)]
        public bool DisplayBoundary
        {
            get => IsBorderVisible;
            set => IsBorderVisible = value;
        }

        [Obsolete]
        [Browsable(false)]
        public int AnimationInterval { get; set; }

        [Obsolete]
        public void SetDotAnimation(string format, params object[] args)
        {
            SetDotAnimation(string.Format(format, args));
        }
        [Obsolete]
        public void StopDotAnimation(string format, params object[] args)
        {
            StopDotAnimation(string.Format(format, args));
        }
    }
}