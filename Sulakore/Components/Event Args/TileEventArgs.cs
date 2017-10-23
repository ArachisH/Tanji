using System.Windows.Forms;

using Sulakore.Habbo;

namespace Sulakore.Components
{
    public class TileEventArgs : MouseEventArgs
    {
        public HPoint Tile { get; }

        public TileEventArgs(HPoint tile, MouseButtons button,
            int clicks, int x, int y, int delta)
            : base(button, clicks, x, y, delta)
        {
            Tile = tile;
        }
    }
}