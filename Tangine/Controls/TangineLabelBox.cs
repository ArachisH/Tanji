using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tangine.Controls
{
    [DesignerCategory("Code")]
    public class TangineLabelBox : Control
    {
        [Browsable(false)]
        public TextBox Box { get; }

        [DefaultValue(false)]
        public bool IsReadOnly
        {
            get => Box.ReadOnly;
            set => Box.ReadOnly = value;
        }

        [DefaultValue(null)]
        public override string Text
        {
            get => Box.Text;
            set => Box.Text = value;
        }

        private int _textPaddingWidth = 10;
        [DefaultValue(10)]
        public int TextPaddingWidth
        {
            get => _textPaddingWidth;
            set
            {
                _textPaddingWidth = value;
                Invalidate();
            }
        }

        private string _title;
        [DefaultValue(null)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                Invalidate();
            }
        }

        public TangineLabelBox()
        {
            SetStyle((ControlStyles)2050, true);
            Size = new Size(200, 20);
            DoubleBuffered = true;

            Box = new TextBox();
            Box.TextAlign = HorizontalAlignment.Center;
            Box.Anchor = (AnchorStyles.Left | AnchorStyles.Right);

            Controls.Add(Box);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (!string.IsNullOrWhiteSpace(Title))
            {
                using (var format = new StringFormat())
                using (var lineColor = new Pen(Color.FromArgb(243, 63, 63)))
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    var titleSize = e.Graphics.MeasureString(Title, Font).ToSize();
                    titleSize.Width += (TextPaddingWidth + 1);
                    titleSize.Height = Height;

                    var titleRect = new Rectangle(new Point(0, 0), titleSize);
                    Box.Size = new Size((Width - titleRect.Width) - 7, Height);
                    Box.Location = new Point(titleRect.Right + 7, 0);

                    e.Graphics.DrawString(Title, Font, Brushes.Black, titleRect, format);
                    e.Graphics.DrawLine(lineColor, titleRect.Right, 0, titleRect.Right, Height - 1);
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