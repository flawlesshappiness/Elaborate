using Godot;
using System.Collections.Generic;

public abstract class Grid
{
    public enum Direction { North, East, South, West }

    public static readonly Vector2I North = new Vector2I(0, -1);
    public static readonly Vector2I East = new Vector2I(1, 0);
    public static readonly Vector2I South = new Vector2I(0, 1);
    public static readonly Vector2I West = new Vector2I(-1, 0);

    public static readonly Dictionary<Direction, Vector2I> Directions = new()
    {
        { Direction.North, North },
        { Direction.East, East },
        { Direction.South, South },
        { Direction.West, West },
    };
}

public class Grid<T> : Grid where T : class
{
    private readonly T[,] _inner;

    public int Width => _inner.GetLength(0);

    public int Height => _inner.GetLength(1);

    public Grid(int width, int height)
    {
        _inner = new T[width, height];
    }

    public T this[int x, int y]
    {
        get { TryGet(x, y, out var v); return v; }
        set { TrySet(x, y, value); }
    }

    public T this[Vector2I p]
    {
        get { TryGet(p, out var v); return v; }
        set { TrySet(p, value); }
    }

    public bool TryGet(Vector2I p, out T v) => TryGet(p.X, p.Y, out v);
    public bool TryGet(int x, int y, out T v)
    {
        if (IsValid(x, y))
        {
            v = _inner[x, y];
            return true;
        }

        v = null;
        return false;
    }

    public bool TrySet(Vector2I p, T v) => TrySet(p.X, p.Y, v);
    public bool TrySet(int x, int y, T v)
    {
        if (IsValid(x, y))
        {
            _inner[x, y] = v;
            return true;
        }

        return false;
    }

    public bool IsValid(Vector2I p) => IsValid(p.X, p.Y);
    public bool IsValid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    public Vector2I GetNeighbourPosition(Vector2I p, Direction d) => GetNeighbourPosition(p.X, p.Y, d);
    public Vector2I GetNeighbourPosition(int x, int y, Direction d)
    {
        var p = Directions[d];
        x += p.X;
        y += p.Y;
        return new Vector2I(x, y);
    }

    public bool TryGetNeighbour(Vector2I p, Direction d, out T v) => TryGetNeighbour(p.X, p.Y, d, out v);
    public bool TryGetNeighbour(int x, int y, Direction d, out T v)
    {
        var p = GetNeighbourPosition(x, y, d);
        return TryGet(p, out v);
    }

    public Dictionary<Direction, T> GetNeighbours(Vector2I p) => GetNeighbours(p.X, p.Y);
    public Dictionary<Direction, T> GetNeighbours(int x, int y)
    {
        var neighbours = new Dictionary<Direction, T>();

        foreach (var kvp in Directions)
        {
            var d = kvp.Key;
            if (!TryGetNeighbour(x, y, d, out var v)) continue;
            neighbours.Add(d, v);
        }

        return neighbours;
    }

    public Dictionary<Direction, Vector2I> GetEmptyNeighbourPositions(Vector2I p) => GetEmptyNeighbourPositions(p.X, p.Y);
    public Dictionary<Direction, Vector2I> GetEmptyNeighbourPositions(int x, int y)
    {
        var positions = new Dictionary<Direction, Vector2I>();
        var neighbours = GetNeighbours(x, y);

        foreach (var n in neighbours)
        {
            if (n.Value == null)
            {
                var d = n.Key;
                var p = GetNeighbourPosition(x, y, d);
                positions.Add(d, p);
            }
        }

        return positions;
    }
}
