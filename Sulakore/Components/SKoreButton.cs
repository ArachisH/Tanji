using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreButton : Control, IButtonControl
    {
        private bool _isPressed;
        private const int GRAD_HEIGHT = 10;

        /// <summary>
        /// Gets or sets a value that is returned to the parent form when the button is clicked.
        /// </summary>
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [SettingsBindable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Invalidate(); }
        }

        [Localizable(true)]
        [DefaultValue(typeof(Size), "100, 20")]
        new public Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        private Color _skin = Color.SteelBlue;
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color Skin
        {
            get { return _skin; }
            set { _skin = value; Invalidate(); }
        }

        public SKoreButton()
        {
            SetStyle((ControlStyles)2050, true);
            DoubleBuffered = true;

            Size = new Size(100, 20);
            BackColor = Color.Transparent;
        }

        public void PerformClick()
        {
            Focus();
            base.OnClick(EventArgs.Empty);
        }
        public void NotifyDefault(bool value)
        {
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        { }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Enabled ?
                Skin : SystemColors.Control);

            using (var pen = new Pen(Color.FromArgb(50, Color.Black)))
            {
                e.Graphics.DrawRectangle(pen, ClientRectangle.X, ClientRectangle.Y,
                    ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            }
            using (var format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                if (Enabled)
                {
                    int textOffset = 0;
                    var clickShadow = Color.FromArgb(25, Color.Black);
                    var textShadow = Color.FromArgb(_isPressed ? 150 : 100, Color.Black);

                    if (_isPressed)
                    {
                        textOffset++;
                        var r1 = new Rectangle(0, 0, Width, GRAD_HEIGHT);
                        using (var clickShadowGradient = new LinearGradientBrush(r1, clickShadow, Color.Transparent, 90))
                            e.Graphics.FillRectangle(clickShadowGradient, r1);

                        var r2 = new Rectangle(0, Height - GRAD_HEIGHT, Width, GRAD_HEIGHT);
                        using (var clickShadowGradient = new LinearGradientBrush(r2, clickShadow, Color.Transparent, 270))
                            e.Graphics.FillRectangle(clickShadowGradient, r2);
                    }
                    using (var textShadowBrush = new SolidBrush(textShadow))
                    {
                        e.Graphics.DrawString(Text, Font, textShadowBrush,
                            new Rectangle(textOffset + 1, textOffset + 1, Width, Height), format);
                    }

                    e.Graphics.DrawString(Text, Font, Brushes.White,
                        new Rectangle(textOffset, textOffset, Width, Height), format);
                }
                else
                {
                    using (var solidBrush = new SolidBrush(Color.FromArgb(150, Color.Black)))
                    {
                        e.Graphics.DrawString(Text, Font, solidBrush,
                            new Rectangle(0, 0, Width, Height), format);
                    }
                }
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

            if (isLeft &&
                ClientRectangle.Contains(e.Location))
            {
                base.OnClick(e);
            }
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
    }
}