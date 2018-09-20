﻿using System;

namespace Sulakore.Habbo
{
    public class HMap
    {
        private readonly string[] _rows;

        public int RowCount { get; }
        public int TileCount { get; }

        public string this[int y]
        {
            get => _rows[y];
            set => _rows[y] = value.Substring(0, _rows[y].Length);
        }
        public char this[int x, int y]
        {
            get => _rows[y][x];
            set => _rows[y] = _rows[y].Remove(x, 1).Insert(x, value.ToString());
        }

        public HMap(string map)
        {
            map = Adjust(map);

            _rows = map.Split(new[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            RowCount = _rows.Length;
            TileCount = map.Length - RowCount;
        }

        public bool IsWalkable(HPoint tile)
        {
            return IsWalkable(tile.X, tile.Y);
        }
        public bool IsWalkable(int x, int y)
        {
            return this[x, y] != 'x';
        }

        public static string Adjust(string map)
        {
            map = map?.ToLower().Trim()
                .Replace("\n", string.Empty)
                .Replace("[13]", "\r") ?? string.Empty;

            return map;
        }

        public override string ToString()
        {
            return string.Join("\r", _rows).Trim();
        }
    }
}