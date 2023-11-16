using Godot;

public partial class Item : NodeScript
{
    private CollisionShape3D collider;

    [Export]
    public string ItemDataId { get; set; }

    public bool CollisionEnabled { set { collider.Disabled = !value; } }

    public override void _Ready()
    {
        base._Ready();

        collider = this.GetNodeInChildren<CollisionShape3D>();
    }
}
