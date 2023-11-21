using Godot;

public partial class Item3D : Node3DScript, IItem
{
    private CollisionShape3D collider;

    [NodeName("Grab")]
    public Node3D GrabNode;

    public bool CollisionEnabled { set { collider.Disabled = !value; } }

    [Export]
    public string ItemDataId { get; set; }

    public Node ItemOwner => this;

    public override void _Ready()
    {
        base._Ready();

        collider = this.GetNodeInChildren<CollisionShape3D>();
    }
}
