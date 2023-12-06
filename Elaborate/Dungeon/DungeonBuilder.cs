using Godot;
using System.Collections.Generic;

public static class DungeonBuilder
{
    public static void BuildDungeon(DungeonBuildArgs args)
    {
        Debug.Log("DungeonBuilder.BuildDungeon");
        Debug.Indent++;

        CreateRooms(args);
        CreateCorridors(args);

        Debug.Indent--;
    }

    private static void CreateRooms(DungeonBuildArgs args)
    {
        Debug.Log("Creating rooms");
        Debug.Indent++;

        args.Info.Rooms.ForEach(info => info.Room = CreateRoom(args, info.X, info.Y));

        Debug.Indent--;
    }

    private static DungeonRoom CreateRoom(DungeonBuildArgs args, int x, int y)
    {
        Debug.Log($"Creating room: {x}, {y}");
        Debug.Indent++;

        var resource = args.Resource;
        var paths = resource.GetRooms();
        var path = paths[args.Random.RandiRange(0, paths.Count - 1)];
        var prefab = GD.Load(path) as PackedScene;
        var node = prefab.Instantiate();

        Scene.Current.AddChild(node);
        var room = node.GetNodeInChildren<DungeonRoom>();
        var position = new Vector3(x * args.RoomAreaSize, 0, y * args.RoomAreaSize);
        var offset_max_x = (int)Mathf.Max(0, (args.RoomAreaSize - room.Size.X) * 0.5f - args.TileSize * 2);
        var offset_max_y = (int)Mathf.Max(0, (args.RoomAreaSize - room.Size.Y) * 0.5f - args.TileSize * 2);
        var offset_x = args.Random.RandiRange(-offset_max_x, offset_max_x) * args.TileSize;
        var offset_y = args.Random.RandiRange(-offset_max_y, offset_max_y) * args.TileSize;
        var offset = new Vector3(offset_x, 0, offset_y);
        room.GlobalPosition = position + offset;

        var nei_N = args.Info.GetRoom(x, y - 1);
        var nei_E = args.Info.GetRoom(x + 1, y);
        var nei_S = args.Info.GetRoom(x, y + 1);
        var nei_W = args.Info.GetRoom(x - 1, y);

        room.WallNorthEnabled = nei_N == null;
        room.WallEastEnabled = nei_E == null;
        room.WallSouthEnabled = nei_S == null;
        room.WallWestEnabled = nei_W == null;

        Debug.Indent--;
        return room;
    }

    private static void CreateCorridors(DungeonBuildArgs args)
    {
        foreach (var info in args.Info.Rooms)
        {
            var north_room = args.Info.GetRoom(info.X, info.Y - 1);
            var east_room = args.Info.GetRoom(info.X + 1, info.Y);
            var rooms = new List<DungeonRoomInfo> { north_room, east_room };
            foreach (var room in rooms)
            {
                if (room == null) continue;
                BuildCorridor(args, info, room);
            }
        }
    }

    private static void BuildCorridor(DungeonBuildArgs args, DungeonRoomInfo room1, DungeonRoomInfo room2)
    {
        var x_diff = Mathf.Abs(room1.X - room2.X);
        var y_diff = Mathf.Abs(room1.Y - room2.Y);
        var horizontal = x_diff > y_diff;
        var lower_room = horizontal ? (room1.X < room2.X ? room1 : room2) : (room1.Y > room2.Y ? room1 : room2);
        var upper_room = lower_room == room1 ? room2 : room1;
        var start_corridor = horizontal ? lower_room.Room.CorridorEast : lower_room.Room.CorridorNorth;
        var end_corridor = horizontal ? upper_room.Room.CorridorWest : upper_room.Room.CorridorSouth;

        var start_pos = start_corridor.GlobalPosition;
        var end_pos = end_corridor.GlobalPosition;
        var dir = end_pos - start_pos;
        var dir_towards = horizontal ? new Vector3(1, 0, 0) : new Vector3(0, 0, -1);
        var sign_align = horizontal ? Mathf.Sign(dir.Z) : Mathf.Sign(dir.X);
        var dir_align = horizontal ? new Vector3(0, 0, sign_align) : new Vector3(sign_align, 0, 0);

        var dist_towards = Mathf.Abs(horizontal ? dir.X : dir.Z);
        var dist_align = Mathf.Abs(horizontal ? dir.Z : dir.X);
        var towards_steps = (int)(dist_towards / args.TileSize) + 1;
        var align_steps = (int)(dist_align / args.TileSize);
        var align_step = args.Random.RandiRange(2, towards_steps - 2);

        for (int i_towards = 0; i_towards < towards_steps; i_towards++)
        {
            var corridor = CreateCorridor(args);
            var offset = i_towards < align_step ? Vector3.Zero : (dir_align * align_steps * args.TileSize);
            corridor.GlobalPosition = start_pos + offset + dir_towards * i_towards * args.TileSize;
        }

        for (int i_align = 0; i_align < align_steps; i_align++)
        {
            var corridor = CreateCorridor(args);
            corridor.GlobalPosition = start_pos + (dir_towards * align_step * args.TileSize) + (dir_align * i_align * args.TileSize);
        }
    }

    private static DungeonCorridor CreateCorridor(DungeonBuildArgs args)
    {
        var resource = args.Resource;
        var paths = resource.GetCorridors();
        var path = paths[args.Random.RandiRange(0, paths.Count - 1)];
        var prefab = GD.Load(path) as PackedScene;
        var node = prefab.Instantiate();

        Scene.Current.AddChild(node);
        var corridor = node.GetNodeInChildren<DungeonCorridor>();

        return corridor;
    }
}

public class DungeonBuildArgs
{
    public DungeonResource Resource { get; set; }

    public DungeonInfo Info { get; set; }

    public RandomNumberGenerator Random { get; set; } = new();

    public float TileSize = 2f;

    public float RoomAreaSize = 16f;
}