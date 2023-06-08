using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tanji.Controls;

[DesignerCategory("Code")]
public sealed class TanjiTabControl : TabControl, ISkinnable
{
    private Pen _pen;
    private SolidBrush _brush;

    private bool _isDisplayingBorder = false;
    [DefaultValue(false)]
    public bool IsDisplayingBorder
    {
        get => _isDisplayingBorder;
        set
        {
            _isDisplayingBorder = value;
            Invalidate();
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
            _pen = new Pen(Skin);

            _brush?.Dispose();
            _brush = new SolidBrush(value);

            Invalidate();
        }
    }

    private Color _titleColor = Color.Black;
    [DefaultValue(typeof(Color), "Black")]
    public Color TitleColor
    {
        get => _titleColor;
        set
        {
            _titleColor = value;
            Invalidate();
        }
    }

    private Color _backcolor = Color.White;
    [DefaultValue(typeof(Color), "White")]
    public Color Backcolor
    {
        get => _backcolor;
        set
        {
            _backcolor = value;
            Invalidate();
        }
    }

    public TanjiTabControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

        _pen = new Pen(_skin);
        _brush = new SolidBrush(_skin);

        DoubleBuffered = true;
        ItemSize = new Size(95, 24);
        SizeMode = TabSizeMode.Fixed;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pen.Dispose();
            _brush?.Dispose();
        }
        base.Dispose(disposing);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Backcolor);
        if (IsDisplayingBorder)
        {

            e.Graphics.DrawLine(_pen, 0, Height - 1, Width - 1, Height - 1);
        }
        if (TabPages.Count > 0)
        {
            Rectangle tabRegion, titleRegion, glowRegion;
            var format = TextFormatFlags.VerticalCenter;

            for (int i = 0; i < TabPages.Count; i++)
            {
                tabRegion = GetTabRect(i);
                bool isSelected = SelectedIndex == i;

                switch (Alignment)
                {
                    default:
                    case TabAlignment.Top:
                    {
                        format |= TextFormatFlags.HorizontalCenter;
                        titleRegion = GetVerticalTitleRegion(tabRegion, -4, i == 0);
                        glowRegion = GetVerticalGlowRegion(titleRegion, tabRegion.Y + (tabRegion.Height - 2));
                        break;
                    }
                    case TabAlignment.Bottom:
                    {
                        format |= TextFormatFlags.HorizontalCenter;
                        titleRegion = GetVerticalTitleRegion(tabRegion, 0, i == 0);
                        glowRegion = GetVerticalGlowRegion(titleRegion, tabRegion.Y);
                        break;
                    }
                    case TabAlignment.Left:
                    {
                        format |= TextFormatFlags.Right;
                        titleRegion = GetHorizontalTitleRegion(tabRegion, -2, i == 0);
                        glowRegion = GetHorizontalGlowRegion(titleRegion, (titleRegion.X + tabRegion.Width));
                        break;
                    }
                    case TabAlignment.Right:
                    {
                        format |= TextFormatFlags.Left;
                        titleRegion = GetHorizontalTitleRegion(tabRegion, 4, i == 0);
                        glowRegion = GetHorizontalGlowRegion(titleRegion, tabRegion.X);
                        break;
                    }
                }
                e.Graphics.FillRectangle(isSelected ? _brush : Brushes.Silver, glowRegion);
                TextRenderer.DrawText(e.Graphics, TabPages[i].Text, Font, titleRegion, TitleColor, format);
            }
        }
        base.OnPaint(e);
    }
    protected override void OnControlAdded(ControlEventArgs e)
    {
        base.OnControlAdded(e);
        if (e.Control is TabPage tab)
        {
            tab.BackColor = Color.White;
            tab.Padding = new Padding(0);
        }
    }

    private static Rectangle GetVerticalGlowRegion(Rectangle titleRegion, int y) => new()
    {
        X = titleRegion.X,
        Y = y,

        Width = titleRegion.Width,
        Height = 1
    };
    private static Rectangle GetHorizontalGlowRegion(Rectangle titleRegion, int x) => new()
    {
        X = x,
        Y = titleRegion.Y,

        Width = 1,
        Height = titleRegion.Height
    };

    private static Rectangle GetVerticalTitleRegion(Rectangle tabRegion, int offset, bool isFirst) => new()
    {
        X = tabRegion.X + (isFirst ? 2 : 0),
        Y = tabRegion.Y + 2,

        Width = tabRegion.Width - (isFirst ? 4 : 2),
        Height = tabRegion.Height + offset
    };
    private static Rectangle GetHorizontalTitleRegion(Rectangle tabRegion, int offset, bool isFirst) => new()
    {
        X = tabRegion.X + offset,
        Y = tabRegion.Y + (isFirst ? 2 : 0),

        Width = tabRegion.Width - 2,
        Height = tabRegion.Height - (isFirst ? 4 : 2)
    };
}