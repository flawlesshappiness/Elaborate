using Godot;
using System.Collections.Generic;
using System.Linq;

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

    public Node3D StartPosition { get; set; }

    public List<DungeonRoomSection> Sections => new List<DungeonRoomSection> { North, East, South, West };

    public List<DungeonRoomSection> Corners => new List<DungeonRoomSection> { NorthWest, NorthEast, SouthWest, SouthEast };

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

        StartPosition = this;
    }

    public void SetNeighbours(DungeonRoomInfo north, DungeonRoomInfo east, DungeonRoomInfo south, DungeonRoomInfo west)
    {
        North.SetHasNeighbor(north != null);
        East.SetHasNeighbor(east != null);
        South.SetHasNeighbor(south != null);
        West.SetHasNeighbor(west != null);
    }

    public void SetStartEnabled(bool is_start)
    {
        North.SetStartEnabled(false);
        East.SetStartEnabled(false);
        South.SetStartEnabled(false);
        West.SetStartEnabled(false);

        if (is_start)
        {
            var section = Sections
                .Where(s => !s.HasNeighbor)
                .ToList()
                .Random();
            section.SetStartEnabled(true);
            section.SetWallEnabled(false);

            if (section.Start.TryGetNode<Node3D>("Position", out var position))
            {
                StartPosition = position;
            }
        }
    }

    public void SetEndEnabled(bool is_end)
    {
        if (is_end)
        {
            if (this.TryGetNode<Node3D>("Position", out var position))
            {
                StartPosition = position;
            }
        }
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

    private void SetNodeEnabled(Node3D node, bool enabled)
    {
        if (node != null)
        {
            node.Visible = enabled;
            node.GetNodesInChildren<CollisionShape3D>()
                .ForEach(c => c.Disabled = !enabled);
        }
    }

    public void SetWallEnabled(bool enabled)
    {
        SetNodeEnabled(Wall, enabled);
    }

    public void SetDoorEnabled(bool enabled)
    {
        SetNodeEnabled(Door, enabled);
    }

    public void SetStartEnabled(bool enabled)
    {
        SetNodeEnabled(Start, enabled);
    }
}
