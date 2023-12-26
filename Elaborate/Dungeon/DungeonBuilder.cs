using Godot;

public static class DungeonBuilder
{
    public static void BuildDungeon(DungeonBuildArgs args)
    {
        Debug.Log("DungeonBuilder.BuildDungeon");
        Debug.Indent++;

        CreateRooms(args);

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

        var resource = args.RoomRandomizer.Random();
        var instance = GD.Load<PackedScene>(resource.Path).Instantiate();
        var room = instance as DungeonRoom;
        room.SetParent(Scene.Current);
        room.Visible = true;

        var position = new Vector3(x * args.Resource.RoomSize, 0, y * args.Resource.RoomSize);
        room.GlobalPosition = position;

        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.North, out var nei_N);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.East, out var nei_E);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.South, out var nei_S);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.West, out var nei_W);

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

    public WeightedRandom<DungeonRoomResource> RoomRandomizer { get; set; } = new();

    public float TileSize = 2f;

    public DungeonBuildArgs(DungeonResource resource, DungeonInfo info)
    {
        this.Resource = resource;
        this.Info = info;

        foreach (var path in Resource.RoomPaths)
        {
            var room = GD.Load<DungeonRoomResource>(path);
            RoomRandomizer.AddElement(room, room.ChanceToSpawn);
        }
    }
}