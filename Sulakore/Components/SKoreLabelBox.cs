using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreLabelBox : Control
    {
        private Rectangle _textRect;
        
        [Browsable(false)]
        public TextBox Box { get; }

        [DefaultValue(typeof(Color), "White")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [DefaultValue(typeof(Color), "SteelBlue")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private int _textPaddingWidth = 10;
        [DefaultValue(10)]
        public int TextPaddingWidth
        {
            get { return _textPaddingWidth; }
            set
            {
                _textPaddingWidth = value;
                Text = Text; // Best hack 2016.
            }
        }

        [DefaultValue(typeof(HorizontalAlignment), "Left")]
        public HorizontalAlignment ValueAlign
        {
            get { return Box.TextAlign; }
            set { Box.TextAlign = value; }
        }

        [DefaultValue(false)]
        public bool ValueReadOnly
        {
            get { return Box.ReadOnly; }
            set { Box.ReadOnly = value; }
        }

        public string Value
        {
            get { return Box.Text; }
            set { Box.Text = value; }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                using (Graphics gfx = CreateGraphics())
                using (var format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    Size titleSize = gfx.MeasureString(value, Font).ToSize();
                    titleSize.Height = Height;
                    titleSize.Width += (1 + _textPaddingWidth);

                    _textRect = new Rectangle(new Point(0, 0), titleSize);
                }

                Box.Location = new Point(_textRect.Right, 0);
                Box.Size = new Size(Width - _textRect.Width, Height);
                Invalidate();
            }
        }

        public SKoreLabelBox()
        {
            SetStyle((ControlStyles)2050, true);
            DoubleBuffered = true;

            Box = new TextBox();
            Box.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

            ForeColor = Color.White;
            BackColor = Color.SteelBlue;

            Size = new Size(200, 20);
            Controls.Add(Box);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (!string.IsNullOrWhiteSpace(Text))
            {
                using (var backColor = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(backColor, _textRect);

                using (var shadow = new Pen(Color.FromArgb(50, Color.Black)))
                {
                    e.Graphics.DrawRectangle(shadow, 0, 0,
                        _textRect.Width, _textRect.Height - 1);
                }
                using (var format = new StringFormat())
                using (var foreColor = new SolidBrush(ForeColor))
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    using (var textShadowBrush = new SolidBrush(Color.FromArgb(100, Color.Black)))
                    {
                        e.Graphics.DrawString(Text, Font, textShadowBrush,
                            new Rectangle(1, 1, _textRect.Width, _textRect.Height), format);
                    }
                    e.Graphics.DrawString(Text, Font, foreColor, _textRect, format);
                }
            }
            base.OnPaint(e);
        }
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 20, specified);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                Box.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}