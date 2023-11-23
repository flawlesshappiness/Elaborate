using Godot;

public partial class Item3D : Node3DScript, IItem
{
    private CollisionShape3D collider;
    private MeshInstance3D mesh;

    [NodeName("Grab")]
    public Node3D GrabNode;

    public bool CollisionEnabled { set { collider.Disabled = !value; } }

    public bool ShadowsEnabled { set { mesh.CastShadow = value ? GeometryInstance3D.ShadowCastingSetting.On : GeometryInstance3D.ShadowCastingSetting.Off; } }

    [Export]
    public string ItemDataId { get; set; }

    public Node ItemOwner => this;

    public override void _Ready()
    {
        base._Ready();

        collider = this.GetNodeInChildren<CollisionShape3D>();
        mesh = this.GetNodeInChildren<MeshInstance3D>();
    }
}
