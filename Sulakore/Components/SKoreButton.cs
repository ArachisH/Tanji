using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel.Design;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreButton : Control, IButtonControl
    {
        private bool _isPressed;
        private Control _lastParent;

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; }

        [Browsable(false)]
        public override Color BackColor
        {
            get => Skin;
            set => Skin = value;
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get => base.BackgroundImage;
            set => base.BackgroundImage = value;
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get => base.BackgroundImageLayout;
            set => base.BackgroundImageLayout = value;
        }

        [SettingsBindable(true)]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        [Localizable(true)]
        [DefaultValue(typeof(Size), "100, 20")]
        new public Size Size
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
                base.BackColor = value;

                Invalidate();
            }
        }

        private bool _isShadowEnabled = true;
        [DefaultValue(true)]
        public bool IsShadowEnabled
        {
            get => _isShadowEnabled;
            set
            {
                _isShadowEnabled = value;
                Invalidate();
            }
        }

        private Point _textOffset = new Point(0, -1);
        [DefaultValue(typeof(Point), "0, -1")]
        public Point TextOffset
        {
            get => _textOffset;
            set
            {
                _textOffset = value;
                Invalidate();
            }
        }

        public SKoreButton()
        {
            SetStyle(ControlStyles.UserPaint, true);

            DoubleBuffered = true;
            Size = new Size(100, 20);
        }

        public void PerformClick()
        {
            if (Enabled)
            {
                Focus();
                base.OnClick(EventArgs.Empty);
            }
        }
        public void NotifyDefault(bool value)
        {
            Invalidate();
        }

        private void ParentDraw(object sender, PaintEventArgs e)
        {
            if (!Enabled || _isPressed || !IsShadowEnabled) return;
            using (var borderPen = new Pen(Color.FromArgb(50, Color.Black)))
            {
                e.Graphics.DrawLine(borderPen, Right, Location.Y, Right, Bottom);
                e.Graphics.DrawLine(borderPen, Location.X, Bottom, Right - 1, Bottom);
            }
        }

        protected override void OnClick(EventArgs e)
        { }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (Parent != null)
            {
                var borderRect = new Rectangle(Location.X, Location.Y, Size.Width + 2, Size.Height + 2);
                Parent.Invalidate(borderRect, false);
            }
            e.Graphics.Clear(Enabled ? Skin : Color.FromArgb(240, 240, 240));
            var format = (TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            var textRect = new Rectangle(1 + TextOffset.X, 1 + TextOffset.Y, Width - 2, Height - 2);
            if (Enabled)
            {
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, Color.White, format);
                if (_isPressed)
                {
                    var shadow = Color.FromArgb(20, Color.Black);
                    var topShadowRect = new Rectangle(0, 0, Width, 10);
                    var bottomShadowRect = new Rectangle(0, Height - 10, Width, 10);
                    using (var topShadowGradient = new LinearGradientBrush(topShadowRect, shadow, Color.Transparent, LinearGradientMode.Vertical))
                    using (var bottomShadowGradient = new LinearGradientBrush(bottomShadowRect, Color.Transparent, shadow, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(topShadowGradient, topShadowRect);
                        e.Graphics.FillRectangle(bottomShadowGradient, bottomShadowRect);
                    }
                }
            }
            else
            {
                ControlPaint.DrawStringDisabled(e.Graphics, Text, Font, SystemColors.Control, textRect, format);
            }
            using (var borderPen = new Pen(Color.FromArgb(50, Color.Black)))
            {
                e.Graphics.DrawRectangle(borderPen, new Rectangle(0, 0, Width - 1, Height - 1));
            }
            base.OnPaint(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool isLeft = (e.Button == MouseButtons.Left);
            if (isLeft)
            {
                _isPressed = false;

                Focus();
                Invalidate();
            }
            base.OnMouseUp(e);

            if (isLeft && ClientRectangle.Contains(e.Location))
            {
                base.OnClick(e);
            }
        }
        protected override void OnParentChanged(EventArgs e)
        {
            if (_lastParent != null)
            {
                _lastParent.Paint -= ParentDraw;
            }
            if (Parent != null)
            {
                _lastParent = Parent;
                _lastParent.Paint += ParentDraw;
            }
            base.OnParentChanged(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;

                Focus();
                Invalidate();
            }
            base.OnMouseDown(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Parent != null)
                {
                    Parent.Paint -= ParentDraw;
                }
            }
            base.Dispose(disposing);
        }
    }
}