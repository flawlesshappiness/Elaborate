using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class DungeonCorridor : Node3DScript
{
    public Node3D WallNorth => GetNode<Node3D>("Wall_N");
    public Node3D WallEast => GetNode<Node3D>("Wall_E");
    public Node3D WallSouth => GetNode<Node3D>("Wall_S");
    public Node3D WallWest => GetNode<Node3D>("Wall_W");

    private List<DungeonCorridor> _neighbors;

    public void SetNeighbors(List<DungeonCorridor> neighbors)
    {
        _neighbors = neighbors;

        var positions = new List<Vector2I>();
        foreach (var nei in _neighbors)
        {
            var relative = nei.GlobalPosition - GlobalPosition;
            Debug.Log(relative);
            var horizontal = Mathf.Abs(relative.X) > Mathf.Abs(relative.Z);
            var x = horizontal ? (relative.X > 0 ? 1 : -1) : 0;
            var y = horizontal ? 0 : (relative.Z > 0 ? 1 : -1);
            positions.Add(new Vector2I(x, y));
        }

        WallNorth.Visible = positions.All(p => p.Y > -1);
        WallEast.Visible = positions.All(p => p.X < 1);
        WallSouth.Visible = positions.All(p => p.Y < 1);
        WallWest.Visible = positions.All(p => p.X > -1);
    }
}
