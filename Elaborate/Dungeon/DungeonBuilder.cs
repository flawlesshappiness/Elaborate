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

        args.Info.Rooms.ForEach(info => info.Room = CreateRoom(args, info));

        Debug.Indent--;
    }

    private static DungeonRoom CreateRoom(DungeonBuildArgs args, DungeonRoomInfo info)
    {
        //Debug.Log($"Creating room: {x}, {y}");

        var x = info.X;
        var y = info.Y;

        var is_start = info == args.Info.StartRoom;
        var is_end = info == args.Info.EndRoom;

        var resource = GetRoomResource(args, info);
        var instance = GD.Load<PackedScene>(resource.Path).Instantiate();
        var room = instance as DungeonRoom;
        room.SetParent(args.Root);
        room.Visible = true;

        var position = new Vector3(x * args.Resource.RoomSize, 0, y * args.Resource.RoomSize);
        room.GlobalPosition = position;

        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.North, out var nei_N);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.East, out var nei_E);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.South, out var nei_S);
        args.Info.Grid.TryGetNeighbour(x, y, Grid.Direction.West, out var nei_W);

        room.Initialize();
        room.SetNeighbours(nei_N, nei_E, nei_S, nei_W);
        room.SetStartEnabled(is_start);
        room.SetEndEnabled(is_end);
        room.UpdateCorners();

        return room;
    }

    private static DungeonRoomResource GetRoomResource(DungeonBuildArgs args, DungeonRoomInfo info)
    {
        if (info == args.Info.StartRoom)
        {
            return args.StartRoom;
        }
        else if (info == args.Info.EndRoom)
        {
            return args.EndRoom;
        }
        else
        {
            return args.RoomRandomizer.Random();
        }
    }
}

public class DungeonBuildArgs
{
    public Node3D Root { get; set; }

    public DungeonResource Resource { get; set; }

    public DungeonInfo Info { get; set; }

    public RandomNumberGenerator Random { get; set; } = new();

    public DungeonRoomResource StartRoom { get; set; }

    public DungeonRoomResource EndRoom { get; set; }

    public WeightedRandom<DungeonRoomResource> RoomRandomizer { get; set; } = new();

    public float TileSize = 2f;

    public DungeonBuildArgs(DungeonResource resource, DungeonInfo info)
    {
        this.Resource = resource;
        this.Info = info;

        CreateRoot();

        StartRoom = GD.Load<DungeonRoomResource>(Resource.StartRoom);
        EndRoom = GD.Load<DungeonRoomResource>(Resource.EndRoom);

        foreach (var path in Resource.RoomPaths)
        {
            var room = GD.Load<DungeonRoomResource>(path);
            RoomRandomizer.AddElement(room, room.ChanceToSpawn);
        }
    }

    private void CreateRoot()
    {
        if (Root != null) return;

        Root = new Node3D();
        Root.SetParent(Scene.Current);
    }
}