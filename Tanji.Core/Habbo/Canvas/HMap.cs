namespace Tanji.Core.Habbo.Canvas;

public class HMap
{
    private readonly string[] _rows;

    public int RowCount => _rows.Length;
    public int TileCount { get; }

    public string this[int y]
    {
        get => _rows[y];
        set => _rows[y] = value[.._rows[y].Length];
    }
    public char this[int x, int y]
    {
        get => _rows[y][x];
        set => _rows[y] = _rows[y].Remove(x, 1).Insert(x, value.ToString());
    }

    public HMap(string map)
    {
        map = Adjust(map);

        _rows = map.Split('\r', StringSplitOptions.RemoveEmptyEntries);

        TileCount = map.Length - RowCount;
    }

    public bool IsWalkable(HPoint tile) => IsWalkable(tile.X, tile.Y);
    public bool IsWalkable(int x, int y) => this[x, y] != 'x';

    public static string Adjust(string map)
    {
        return map?.ToLowerInvariant().Trim()
            .Replace("\n", string.Empty)
            .Replace("[13]", "\r") ?? string.Empty;
    }

    public override string ToString()
        => string.Join("\r", _rows).Trim();
}