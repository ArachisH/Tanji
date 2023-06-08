using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

namespace Tanji.Controls;

public sealed class TanjiLabelBox : Control, ISkinnable
{
    private Pen _pen;
    private Rectangle _titleRect;

    [Browsable(false)]
    public TanjiTextBox Box { get; }

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

    [AllowNull]
    [DefaultValue(null)]
    [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
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

            _pen?.Dispose();
            _pen = new Pen(value);

            Invalidate();
        }
    }

    private string? _title;
    [AllowNull]
    [DefaultValue(null)]
    public string? Title
    {
        get => _title;
        set
        {
            _title = value;

            ResizeBox();
            Invalidate();
        }
    }

    [DefaultValue(true)]
    public new bool TabStop
    {
        get => Box.TabStop;
        set => Box.TabStop = value;
    }

    private Size _defaultSize = new(200, 20);
    [DefaultValue(typeof(Size), "200, 20")]
    protected override Size DefaultSize
    {
        get => _defaultSize;
    }

    public TanjiLabelBox()
    {
        _pen = new Pen(Skin);

        SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;

        base.TabStop = false;

        Box = new TanjiTextBox(this);
        Controls.Add(Box);
    }

    private void ResizeBox()
    {
        Size titleSize = TextRenderer.MeasureText(Title, Font);
        titleSize.Width += TextPaddingWidth;
        titleSize.Height = Height;
        _titleRect = new Rectangle(new Point(0, -1), titleSize);

        Box.Size = new Size(Width - titleSize.Width - 7, Height);
    }

    internal void RaiseOnKeyDown(KeyEventArgs e) => OnKeyDown(e);
    internal void RaiseOnTextChange(EventArgs e) => OnTextChanged(e);
    internal void RaiseOnKeyPress(KeyPressEventArgs e) => OnKeyPress(e);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pen?.Dispose();
            Box.Dispose();
        }
        base.Dispose(disposing);
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
            e.Graphics.DrawLine(_pen, _titleRect.Right, 0, _titleRect.Right, Height);
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
        base.SetBoundsCore(x, y, width, height, specified);
        Box.Multiline = height > 20;
        ResizeBox();
    }
}