using Godot;

public static class DungeonBuilder
{
    public static void BuildDungeon(DungeonBuildArgs args)
    {
        Debug.Log("DungeonBuilder.BuildDungeon");
        Debug.Indent++;

        CreateRooms(args);
        //CreateCorridors(args);

        Debug.Indent--;
    }

    private static void CreateRooms(DungeonBuildArgs args)
    {
        //Debug.Log("Creating rooms");
        Debug.Indent++;

        args.Info.Rooms.ForEach(info => info.Room = CreateRoom(args, info.X, info.Y));

        Debug.Indent--;
    }

    private static DungeonRoom CreateRoom(DungeonBuildArgs args, int x, int y)
    {
        //Debug.Log($"Creating room: {x}, {y}");

        var resource = args.Resource;
        var paths = resource.GetRooms();
        var path = paths[args.Random.RandiRange(0, paths.Count - 1)];
        var prefab = GD.Load(path) as PackedScene;
        var node = prefab.Instantiate();

        Scene.Current.AddChild(node);
        var room = node.GetNodeInChildren<DungeonRoom>();
        var position = new Vector3(x * args.RoomAreaSize, 0, y * args.RoomAreaSize);
        room.GlobalPosition = position;

        var nei_N = args.Info.GetRoom(x, y - 1);
        var nei_E = args.Info.GetRoom(x + 1, y);
        var nei_S = args.Info.GetRoom(x, y + 1);
        var nei_W = args.Info.GetRoom(x - 1, y);

        room.Initialize();
        room.North.SetHasNeighbor(nei_N != null);
        room.East.SetHasNeighbor(nei_E != null);
        room.South.SetHasNeighbor(nei_S != null);
        room.West.SetHasNeighbor(nei_W != null);
        room.UpdateCorners();

        return room;
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