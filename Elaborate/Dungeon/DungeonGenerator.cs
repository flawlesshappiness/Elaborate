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
                position = empty_positions.Random();
                empty_positions.RemoveAll(p => p.X == position.X && p.Y == position.Y);
            }

            var room = AddRoom(position.X, position.Y);

            var empty_neighbors = dungeon.Grid.GetEmptyNeighbourPositions(position.X, position.Y);
            empty_positions.AddRange(empty_neighbors.Values);

            rooms_to_place--;
        }

        AddStartRoom();
        AddEndRoom();

        return dungeon;

        DungeonRoomInfo AddRoom(int x, int y)
        {
            var room = new DungeonRoomInfo { X = x, Y = y };
            dungeon.Grid[x, y] = room;
            dungeon.Rooms.Add(room);
            return room;
        }

        void AddStartRoom()
        {
            var valid_rooms = dungeon.Rooms
                .OrderBy(room => dungeon.Grid.GetNeighbours(room.Position).Count(kvp => kvp.Value != null))
                .ToList();
            var room = valid_rooms.First();
            dungeon.StartRoom = room;
        }

        void AddEndRoom()
        {
            var room = dungeon.Rooms
                .Where(room => dungeon.Grid.GetNeighbours(room.Position).Count(kvp => kvp.Value != null) < 4 && room != dungeon.StartRoom)
                .OrderByDescending(room => room.Position.DistanceTo(dungeon.StartRoom.Position))
                .First();
            dungeon.EndRoom = room;
        }
    }
}

public class DungeonInfo
{
    public Grid<DungeonRoomInfo> Grid { get; set; }

    public List<DungeonRoomInfo> Rooms { get; set; } = new();

    public DungeonRoomInfo StartRoom { get; set; }

    public DungeonRoomInfo EndRoom { get; set; }

    public DungeonInfo(int width, int height)
    {
        Grid = new Grid<DungeonRoomInfo>(width, height);
    }

    public void Log()
    {
        Debug.Log($"Dungeon: {Grid.Width},{Grid.Height}");
        Debug.Log($"Rooms: {Rooms.Count}");

        for (int y = 0; y < Grid.Height; y++)
        {
            var s = "|";
            for (int x = 0; x < Grid.Width; x++)
            {
                var room = Grid[x, y];
                var neis = Grid.GetNeighbours(x, y).Select(kvp => kvp.Value).Where(n => n != null);
                var v = room == null ? "." :
                    room == StartRoom ? "S" :
                    room == EndRoom ? "E" :
                    neis.Count().ToString();
                s += v;
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
    public Vector2I Position => new Vector2I(X, Y);

    public DungeonRoom Room { get; set; }

    public override bool Equals(object obj)
    {
        var info = obj as DungeonRoomInfo;
        if (info == null) return false;
        return info.X == X && info.Y == Y;
    }
}
