using Godot;

public partial class DungeonRoom : Node3DScript
{
    [Export]
    public Vector2I Size;

    public Node3D CorridorNorth => GetNode<Node3D>("Corridor_N");
    public Node3D CorridorEast => GetNode<Node3D>("Corridor_E");
    public Node3D CorridorSouth => GetNode<Node3D>("Corridor_S");
    public Node3D CorridorWest => GetNode<Node3D>("Corridor_W");
    public Node3D WallNorth => GetNode<Node3D>("Wall_N");
    public Node3D WallEast => GetNode<Node3D>("Wall_E");
    public Node3D WallSouth => GetNode<Node3D>("Wall_S");
    public Node3D WallWest => GetNode<Node3D>("Wall_W");
    public bool WallNorthEnabled { set => SetWallEnabled(WallNorth, value); }
    public bool WallEastEnabled { set => SetWallEnabled(WallEast, value); }
    public bool WallSouthEnabled { set => SetWallEnabled(WallSouth, value); }
    public bool WallWestEnabled { set => SetWallEnabled(WallWest, value); }

    private void SetWallEnabled(Node3D node, bool enabled)
    {
        node.Visible = enabled;

        var collider = node.GetNodeInChildren<CollisionShape3D>();
        collider.Disabled = !enabled;
    }
}
