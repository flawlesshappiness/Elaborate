using Godot;

public partial class DungeonRoom : Node3DScript
{
    public DungeonRoomSection North { get; set; }
    public DungeonRoomSection East { get; set; }
    public DungeonRoomSection South { get; set; }
    public DungeonRoomSection West { get; set; }

    public DungeonRoomSection NorthWest { get; set; }
    public DungeonRoomSection NorthEast { get; set; }
    public DungeonRoomSection SouthWest { get; set; }
    public DungeonRoomSection SouthEast { get; set; }

    public void Initialize()
    {
        North = new DungeonRoomSection(this, "N");
        East = new DungeonRoomSection(this, "E");
        South = new DungeonRoomSection(this, "S");
        West = new DungeonRoomSection(this, "W");

        NorthWest = new DungeonRoomSection(this, "NW");
        NorthEast = new DungeonRoomSection(this, "NE");
        SouthWest = new DungeonRoomSection(this, "SW");
        SouthEast = new DungeonRoomSection(this, "SE");
    }

    public void UpdateCorners()
    {
        NorthWest.SetDoorEnabled(North.HasNeighbor && West.HasNeighbor);
        NorthEast.SetDoorEnabled(North.HasNeighbor && East.HasNeighbor);
        SouthWest.SetDoorEnabled(South.HasNeighbor && West.HasNeighbor);
        SouthEast.SetDoorEnabled(South.HasNeighbor && East.HasNeighbor);

        NorthWest.SetWallEnabled(!North.HasNeighbor && !West.HasNeighbor);
        NorthEast.SetWallEnabled(!North.HasNeighbor && !East.HasNeighbor);
        SouthWest.SetWallEnabled(!South.HasNeighbor && !West.HasNeighbor);
        SouthEast.SetWallEnabled(!South.HasNeighbor && !East.HasNeighbor);
    }
}

public class DungeonRoomSection
{
    public DungeonRoom Room { get; set; }

    public string Prefix { get; set; }

    public bool HasNeighbor { get; set; }

    public Node3D Wall;

    public Node3D Door;

    public Node3D Start;

    public DungeonRoomSection(DungeonRoom room, string prefix)
    {
        Room = room;
        Prefix = prefix;

        room.TryGetNode($"{prefix}/Wall", out Wall);
        room.TryGetNode($"{prefix}/Door", out Door);
        room.TryGetNode($"{prefix}/Start", out Start);
    }

    public void SetHasNeighbor(bool has_neighbor)
    {
        HasNeighbor = has_neighbor;
        SetWallEnabled(!has_neighbor);
        SetDoorEnabled(has_neighbor);
    }

    public void SetWallEnabled(bool enabled)
    {
        if (Wall != null)
        {
            Wall.Visible = enabled;
            Wall.GetNodesInChildren<CollisionShape3D>()
                .ForEach(c => c.Disabled = !enabled);
        }
    }

    public void SetDoorEnabled(bool enabled)
    {
        if (Door != null)
        {
            Door.Visible = enabled;
            Door.GetNodesInChildren<CollisionShape3D>()
                .ForEach(c => c.Disabled = !enabled);
        }
    }
}
