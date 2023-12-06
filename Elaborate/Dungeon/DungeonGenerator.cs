using Godot;
using System.Collections.Generic;
using System.Linq;

public static class DungeonGenerator
{
    public static DungeonInfo GenerateDungeon(int width, int height, int room_count)
    {
        var random = new RandomNumberGenerator();
        var dungeon = new DungeonInfo(width, height);

        var empty_positions = new List<Vector2I>();

        var rooms_to_place = room_count;
        while (rooms_to_place > 0)
        {
            Vector2I position = new Vector2I();

            if (dungeon.Rooms.Count == 0)
            {
                var x = (int)((float)width / 2);
                var y = (int)((float)height / 2);
                position = new Vector2I(x, y);
            }
            else
            {
                position = empty_positions[random.RandiRange(0, empty_positions.Count - 1)];
                empty_positions.Where(p => p.X == position.X && p.Y == position.Y)
                    .ToList().ForEach(p => empty_positions.Remove(p));
            }

            var room = new DungeonRoomInfo { X = position.X, Y = position.Y };
            dungeon.Grid[position.X, position.Y] = room;
            dungeon.Rooms.Add(room);

            var empty_neighbors = dungeon.GetNeighborPositions(position.X, position.Y)
                .Where(p => dungeon.IsValidRoom(p.X, p.Y) && dungeon.Grid[p.X, p.Y] == null).ToList();

            empty_positions.AddRange(empty_neighbors);

            rooms_to_place--;
        }

        return dungeon;
    }
}

public class DungeonInfo
{
    public DungeonRoomInfo[,] Grid { get; set; }

    public List<DungeonRoomInfo> Rooms { get; set; } = new();

    public int Width => Grid.GetLength(0);

    public int Height => Grid.GetLength(1);

    private readonly List<Vector2I> _neighbor_offsets = new()
    {
        new Vector2I(0, 1), // N
        new Vector2I(1, 0), // E
        new Vector2I(0, -1), // S
        new Vector2I(-1, 0) // W
    };

    public DungeonInfo(int width, int height)
    {
        Grid = new DungeonRoomInfo[width, height];
    }

    public List<Vector2I> GetNeighborPositions(int x, int y)
    {
        return _neighbor_offsets.Select(p => new Vector2I(p.X + x, p.Y + y)).ToList();
    }

    public bool IsValidRoom(int x, int y) =>
        x >= 0 && x < Grid.GetLength(0) &&
        y >= 0 && y < Grid.GetLength(1);

    public bool HasRoom(int x, int y) =>
        IsValidRoom(x, y) && Grid[x, y] != null;

    public DungeonRoomInfo GetRoom(int x, int y) =>
        HasRoom(x, y) ? Grid[x, y] : null;

    public List<DungeonRoomInfo> GetRoomNeighbors(int x, int y)
    {
        var neighbors = new List<DungeonRoomInfo>();

        foreach (var offset in _neighbor_offsets)
        {
            var nx = x + (int)offset.X;
            var ny = y + (int)offset.Y;

            if (!IsValidRoom(nx, ny)) continue;

            var room = Grid[nx, ny];

            if (room != null)
            {
                neighbors.Add(room);
            }
        }

        return neighbors;
    }

    public void Log()
    {
        for (int y = 0; y < Grid.GetLength(1); y++)
        {
            var s = "|";
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                var room = Grid[x, y];
                var neis = GetRoomNeighbors(x, y);
                s += room == null ? "." : neis.Count;
            }

            s += "|";

            Debug.Log(s);
        }
    }
}

public class DungeonRoomInfo
{
    public int X { get; set; }
    public int Y { get; set; }

    public DungeonRoom Room { get; set; }
}
