using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreTabControl : TabControl
    {
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

        [Obsolete]
        [Browsable(false)]
        public bool DisplayBoundary
        {
            get => IsDisplayingBorder;
            set => IsDisplayingBorder = value;
        }

        public SKoreTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);

            DoubleBuffered = true;
            ItemSize = new Size(95, 24);
            SizeMode = TabSizeMode.Fixed;
        }

        protected Rectangle GetHorizontalTitleRegion(int tabIndex, int xOffset)
        {
            Rectangle tabRegion = GetTabRect(tabIndex);
            var titleRegion = new Rectangle();
            bool isFirstTab = (tabIndex == 0);

            titleRegion.X = (tabRegion.X + xOffset);
            titleRegion.Y = (tabRegion.Y + (isFirstTab ? 2 : 0));

            titleRegion.Width = (tabRegion.Width - 2);
            titleRegion.Height = (tabRegion.Height - (isFirstTab ? 4 : 2));

            return titleRegion;
        }
        protected Rectangle GetHorizontalGlowRegion(Rectangle tabRegion, Rectangle titleRegion, int x)
        {
            var glowRegion = new Rectangle();

            glowRegion.X = x;
            glowRegion.Y = titleRegion.Y;

            glowRegion.Width = 1;
            glowRegion.Height = titleRegion.Height;

            return glowRegion;
        }

        protected Rectangle GetVerticalTitleRegion(int tabIndex, int heightOffset)
        {
            Rectangle tabRegion = GetTabRect(tabIndex);
            var titleRegion = new Rectangle();
            bool isFirstTab = (tabIndex == 0);

            titleRegion.X = (tabRegion.X + (isFirstTab ? 2 : 0));
            titleRegion.Y = (tabRegion.Y + 2);

            titleRegion.Width = (tabRegion.Width - (isFirstTab ? 4 : 2));
            titleRegion.Height = (tabRegion.Height + heightOffset);

            return titleRegion;
        }
        protected Rectangle GetVerticalGlowRegion(Rectangle tabRegion, Rectangle titleRegion, int yOffset)
        {
            var glowRegion = new Rectangle();

            glowRegion.X = titleRegion.X;
            glowRegion.Y = (tabRegion.Y + yOffset);

            glowRegion.Width = titleRegion.Width;
            glowRegion.Height = 1;

            return glowRegion;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Backcolor);
            if (IsDisplayingBorder)
            {
                using (var pen = new Pen(Skin))
                {
                    e.Graphics.DrawLine(pen, 0, Height - 1, Width - 1, Height - 1);
                }
            }
            if (TabPages.Count > 0)
            {
                Rectangle tabRegion, titleRegion, glowRegion;

                using (var skinBrush = new SolidBrush(Skin))
                {
                    var format = TextFormatFlags.VerticalCenter;
                    for (int i = 0; i < TabPages.Count; i++)
                    {
                        tabRegion = GetTabRect(i);
                        bool isSelected = (SelectedIndex == i);

                        switch (Alignment)
                        {
                            default:
                            case TabAlignment.Top:
                            {
                                format |= TextFormatFlags.HorizontalCenter;
                                titleRegion = GetVerticalTitleRegion(i, -4);
                                glowRegion = GetVerticalGlowRegion(tabRegion, titleRegion, tabRegion.Height - 2);
                                break;
                            }
                            case TabAlignment.Bottom:
                            {
                                format |= TextFormatFlags.HorizontalCenter;
                                titleRegion = GetVerticalTitleRegion(i, 0);
                                glowRegion = GetVerticalGlowRegion(tabRegion, titleRegion, 0);
                                break;
                            }
                            case TabAlignment.Left:
                            {
                                format |= TextFormatFlags.Right;
                                titleRegion = GetHorizontalTitleRegion(i, -2);
                                glowRegion = GetHorizontalGlowRegion(tabRegion, titleRegion, (titleRegion.X + tabRegion.Width));
                                break;
                            }
                            case TabAlignment.Right:
                            {
                                format |= TextFormatFlags.Left;
                                titleRegion = GetHorizontalTitleRegion(i, 4);
                                glowRegion = GetHorizontalGlowRegion(tabRegion, titleRegion, tabRegion.X);
                                break;
                            }
                        }
                        e.Graphics.FillRectangle((isSelected ? skinBrush : Brushes.Silver), glowRegion);
                        TextRenderer.DrawText(e.Graphics, TabPages[i].Text, Font, titleRegion, TitleColor, format);
                    }
                }
            }
            base.OnPaint(e);
        }
    }
}