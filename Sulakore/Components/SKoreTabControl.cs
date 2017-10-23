using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreTabControl : TabControl
    {
        private bool _displayBoundary = false;
        [DefaultValue(false)]
        public bool DisplayBoundary
        {
            get { return _displayBoundary; }
            set { _displayBoundary = value; Invalidate(); }
        }

        private Color _skin = Color.SteelBlue;
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color Skin
        {
            get { return _skin; }
            set { _skin = value; Invalidate(); }
        }

        private Color _titleColor = Color.Black;
        [DefaultValue(typeof(Color), "Black")]
        public Color TitleColor
        {
            get { return _titleColor; }
            set { _titleColor = value; Invalidate(); }
        }

        private Color _backcolor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color Backcolor
        {
            get { return _backcolor; }
            set { _backcolor = value; Invalidate(); }
        }

        public SKoreTabControl()
        {
            SetStyle((ControlStyles)2050, true);
            DoubleBuffered = true;

            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(95, 24);

            DrawMode = TabDrawMode.OwnerDrawFixed;
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

            glowRegion.Width = 2;
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
            glowRegion.Height = 2;

            return glowRegion;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Backcolor);
            if (DisplayBoundary)
            {
                using (var pen = new Pen(Skin))
                    e.Graphics.DrawLine(pen, 0, Height - 1, Width - 1, Height - 1);
            }
            if (TabPages.Count > 0)
            {
                Rectangle tabRegion, titleRegion, glowRegion;

                using (var titleFormat = new StringFormat())
                using (var skinBrush = new SolidBrush(Skin))
                using (var titleBrush = new SolidBrush(TitleColor))
                {
                    titleFormat.Alignment = StringAlignment.Center;
                    titleFormat.LineAlignment = StringAlignment.Center;
                    for (int i = 0; i < TabPages.Count; i++)
                    {
                        tabRegion = GetTabRect(i);
                        bool isSelected = (SelectedIndex == i);

                        switch (Alignment)
                        {
                            default:
                            case TabAlignment.Top:
                            {
                                titleRegion = GetVerticalTitleRegion(i, -4);
                                glowRegion = GetVerticalGlowRegion(tabRegion, titleRegion, tabRegion.Height - 2);
                                break;
                            }
                            case TabAlignment.Bottom:
                            {
                                titleRegion = GetVerticalTitleRegion(i, 0);
                                glowRegion = GetVerticalGlowRegion(tabRegion, titleRegion, 0);
                                break;
                            }
                            case TabAlignment.Left:
                            {
                                titleFormat.Alignment = StringAlignment.Far;
                                titleRegion = GetHorizontalTitleRegion(i, -2);
                                glowRegion = GetHorizontalGlowRegion(tabRegion, titleRegion, (titleRegion.X + tabRegion.Width));
                                break;
                            }
                            case TabAlignment.Right:
                            {
                                titleFormat.Alignment = StringAlignment.Near;
                                titleRegion = GetHorizontalTitleRegion(i, 4);
                                glowRegion = GetHorizontalGlowRegion(tabRegion, titleRegion, tabRegion.X);
                                break;
                            }
                        }

                        e.Graphics.FillRectangle(
                            (isSelected ? skinBrush : Brushes.Silver), glowRegion);

                        e.Graphics.DrawString(TabPages[i].Text,
                            Font, titleBrush, titleRegion, titleFormat);
                    }
                }
            }
            base.OnPaint(e);
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Padding = new Padding(3, 3, 3, 3);
            base.OnControlAdded(e);
        }
    }
}