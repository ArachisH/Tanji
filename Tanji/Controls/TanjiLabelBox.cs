using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tanji.Controls;

[DesignerCategory("Code")]
public class TanjiLabelBox : Control, ISkinnable
{
    private Rectangle _titleRect;

    [Browsable(false)]
    public TextBox Box { get; }

    private bool _isReadOnly = false;
    [DefaultValue(false)]
    public bool IsReadOnly
    {
        get => _isReadOnly;
        set
        {
            _isReadOnly = value;
            Box.ForeColor = value ? Color.FromArgb(150, 150, 150) : Color.Black;
        }
    }

    [DefaultValue(false)]
    public bool IsNumbersOnly { get; set; }

    [DefaultValue(null)]
    public override string Text
    {
        get => Box.Text;
        set => Box.Text = value;
    }

    [DefaultValue(32767)]
    public int MaxLength
    {
        get => Box.MaxLength;
        set => Box.MaxLength = value;
    }

    private int _textPaddingWidth = 10;
    [DefaultValue(10)]
    public int TextPaddingWidth
    {
        get => _textPaddingWidth;
        set
        {
            _textPaddingWidth = value;
            Title = Title;
        }
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

    private string _title;
    [DefaultValue(null)]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;

            Size titleSize = TextRenderer.MeasureText(Title, Font);
            titleSize.Width += TextPaddingWidth;
            titleSize.Height = Height;
            _titleRect = new Rectangle(new Point(0, -1), titleSize);

            Box.Size = new Size(Width - titleSize.Width - 7, Height);
            Invalidate();
        }
    }

    [DefaultValue(true)]
    public new bool TabStop
    {
        get => Box.TabStop;
        set => Box.TabStop = value;
    }

    public TanjiLabelBox()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;

        base.TabStop = false;
        Size = new Size(200, 21);

        Box = new TextBox
        {
            TabStop = true,
            AutoSize = false,
            Dock = DockStyle.Right,
            ForeColor = Color.Black,
            BackColor = Color.White,
            TextAlign = HorizontalAlignment.Center
        };
        Box.KeyDown += Box_KeyDown;
        Box.KeyPress += Box_KeyPress;
        Box.TextChanged += Box_TextChanged;
        Controls.Add(Box);
    }

    private void Box_KeyDown(object sender, KeyEventArgs e)
    {
        if (IsReadOnly)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        else OnKeyDown(e);
    }
    private void Box_TextChanged(object sender, EventArgs e)
    {
        OnTextChanged(e);
    }
    private void Box_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (IsNumbersOnly)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        Box.Focus();
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Color.White);
        if (!string.IsNullOrWhiteSpace(Title))
        {
            TextRenderer.DrawText(e.Graphics, Title, Font, _titleRect, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

            using var lineColor = new Pen(Skin);
            e.Graphics.DrawLine(lineColor, _titleRect.Right, 0, _titleRect.Right, Height);
        }
        base.OnPaint(e);
    }
    protected override void Select(bool directed, bool forward)
    {
        base.Select(directed, forward);
        Box.Select();
    }
    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, width, 21, specified);
        if (Box != null)
        {
            Title = Title;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Box.KeyDown -= Box_KeyDown;
            Box.KeyPress -= Box_KeyPress;
            Box.TextChanged -= Box_TextChanged;
            Box.Dispose();
        }
        base.Dispose(disposing);
    }
}