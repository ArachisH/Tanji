using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Sulakore.Habbo;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    [DefaultEvent(nameof(TileClick))]
    public class SKoreMap : Control
    {
        private string[] _rows;
        private RectangleF _enteredRegion;

        private readonly List<RectangleF> _tileRegions;
        private readonly IDictionary<char, Color> _markers;
        private readonly Dictionary<RectangleF, HPoint> _tiles;
        private readonly IDictionary<int, IDictionary<int, Color>> _paints;

        public event EventHandler<TileEventArgs> TileClick;
        protected virtual void OnTileClick(TileEventArgs e)
        {
            TileClick?.Invoke(this, e);
        }

        public event EventHandler<TileEventArgs> TileDoubleClick;
        protected virtual void OnTileDoubleClick(TileEventArgs e)
        {
            TileDoubleClick?.Invoke(this, e);
        }

        public event EventHandler<TileEventArgs> TileEnter;
        protected virtual void OnTileEnter(TileEventArgs e)
        {
            TileEnter?.Invoke(this, e);
        }

        public event EventHandler<TileEventArgs> TileLeave;
        protected virtual void OnTileLeave(TileEventArgs e)
        {
            TileLeave?.Invoke(this, e);
        }

        private Color _voidColor = Color.Black;
        [DefaultValue(typeof(Color), "Black")]
        public Color VoidColor
        {
            get { return _voidColor; }
            set
            {
                _voidColor = value;
                _markers['x'] = value;

                Invalidate();
            }
        }

        public SKoreMap()
        {
            SetStyle((ControlStyles)2050, true);
            DoubleBuffered = true;

            _tileRegions = new List<RectangleF>();
            _markers = new Dictionary<char, Color>();
            _tiles = new Dictionary<RectangleF, HPoint>();
            _paints = new Dictionary<int, IDictionary<int, Color>>();

            SetDefaultMarkers();
        }

        public void PaintMap(HMap map)
        {
            PaintMap(map.ToString());
        }
        public virtual void PaintMap(string map)
        {
            _paints.Clear();
            _rows = HMap.Adjust(map).Split(new[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            Invalidate();
        }

        public void PaintTile(HPoint tile, Color marker)
        {
            PaintTile(tile.X, tile.Y, marker);
        }
        public virtual void PaintTile(int x, int y, Color marker)
        {
            if (!_paints.ContainsKey(y))
                _paints[y] = new Dictionary<int, Color>();

            _paints[y][x] = marker;
            Invalidate();
        }

        public void SetMarker(double z, Color marker)
        {
            SetMarker(HPoint.ToLevel(z), marker);
        }
        public virtual void SetMarker(char level, Color marker)
        {
            _markers[level] = marker;
            Invalidate();
        }

        public void SetLevel(int x, int y, double z)
        {
            SetLevel(x, y, HPoint.ToLevel(z));
        }
        public virtual void SetLevel(int x, int y, char level)
        {
            _rows[y] = _rows[y].Remove(x, 1)
                .Insert(x, level.ToString());

            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (_rows?.Length > 0)
            {
                _tiles.Clear();
                _tileRegions.Clear();
                float tileWidth = (ClientRectangle.Width -
                    (float)(_rows[0].Length - 1)) / _rows[0].Length;

                float tileHeight = (ClientRectangle.Height -
                    (float)(_rows.Length - 1)) / _rows.Length;

                for (int y = 0; y < _rows.Length; y++)
                {
                    string row = _rows[y];
                    for (int x = 0; x < row.Length; x++)
                    {
                        var tileRegion = new RectangleF(x * (tileWidth + (x != 0 ? 1 : 0)),
                            y * (tileHeight + (y != 0 ? 1 : 0)), tileWidth, tileHeight);

                        Color marker = _markers[row[x]];
                        if (_paints.ContainsKey(y) && _paints[y].ContainsKey(x))
                            marker = _paints[y][x];

                        if (BackColor != marker)
                        {
                            using (var brush = new SolidBrush(marker))
                                e.Graphics.FillRectangle(brush, tileRegion);
                        }

                        _tileRegions.Add(tileRegion);
                        _tiles.Add(tileRegion, new HPoint(x, y, row[x]));
                    }
                }
            }
            base.OnPaint(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            _enteredRegion = RectangleF.Empty;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_enteredRegion.IsEmpty)
            {
                _enteredRegion = GetTileRegion(e.X, e.Y);
                OnTileAction(_enteredRegion, OnTileEnter, e);
            }
            else if (!_enteredRegion.Contains(e.Location))
            {
                RectangleF region = GetTileRegion(e.X, e.Y);

                OnTileAction(_enteredRegion, OnTileLeave, e);
                OnTileAction(region, OnTileEnter, e);

                _enteredRegion = region;
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            OnTileAction(OnTileClick, e);
            base.OnMouseClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            OnTileAction(OnTileDoubleClick, e);
            base.OnMouseDoubleClick(e);
        }

        protected virtual void SetDefaultMarkers()
        {
            _markers['x'] = VoidColor;
            _markers['0'] = Color.FromArgb(0, 101, 255);
            _markers['1'] = Color.FromArgb(0, 145, 255);
            _markers['2'] = Color.FromArgb(0, 188, 255);
            _markers['3'] = Color.FromArgb(1, 232, 255);
            _markers['4'] = Color.FromArgb(1, 255, 234);
            _markers['5'] = Color.FromArgb(1, 255, 191);
            _markers['6'] = Color.FromArgb(1, 255, 147);
            _markers['7'] = Color.FromArgb(1, 255, 104);
            _markers['8'] = Color.FromArgb(1, 255, 61);
            _markers['9'] = Color.FromArgb(1, 255, 17);
            _markers['a'] = Color.FromArgb(25, 255, 0);
            _markers['b'] = Color.FromArgb(68, 255, 0);
            _markers['c'] = Color.FromArgb(68, 255, 0);
            _markers['d'] = Color.FromArgb(112, 255, 0);
            _markers['e'] = Color.FromArgb(155, 255, 0);
            _markers['f'] = Color.FromArgb(198, 255, 0);
            _markers['g'] = Color.FromArgb(242, 255, 0);
            _markers['h'] = Color.FromArgb(255, 224, 0);
            _markers['i'] = Color.FromArgb(255, 181, 0);
            _markers['j'] = Color.FromArgb(255, 137, 0);
            _markers['k'] = Color.FromArgb(255, 94, 0);
            _markers['l'] = Color.FromArgb(255, 50, 0);
            _markers['m'] = Color.FromArgb(255, 7, 0);
            _markers['n'] = Color.FromArgb(255, 0, 35);
            _markers['o'] = Color.FromArgb(255, 0, 79);
            _markers['p'] = Color.FromArgb(255, 0, 122);
            _markers['q'] = Color.FromArgb(255, 0, 165);
            _markers['r'] = Color.FromArgb(255, 0, 209);
            _markers['s'] = Color.FromArgb(214, 0, 255);
            _markers['t'] = Color.FromArgb(170, 0, 255);
        }
        protected virtual RectangleF GetTileRegion(int mouseX, int mouseY)
        {
            RectangleF[] tileRegions = _tileRegions.ToArray();
            foreach (RectangleF tileRegion in tileRegions)
            {
                if (tileRegion.Contains(mouseX, mouseY))
                    return tileRegion;
            }
            return RectangleF.Empty;
        }

        private void OnTileAction(Action<TileEventArgs> tileAction, MouseEventArgs e)
        {
            RectangleF region = GetTileRegion(e.X, e.Y);
            OnTileAction(region, tileAction, e);
        }
        protected virtual void OnTileAction(RectangleF region, Action<TileEventArgs> tileAction, MouseEventArgs e)
        {
            if (!region.IsEmpty)
            {
                HPoint tile = _tiles[region];

                tileAction(new TileEventArgs(
                    tile, e.Button, e.Clicks, e.X, e.Y, e.Delta));
            }
        }
    }
}