using Godot;

public partial class Item3D : Node3DScript, IItem
{
    private CollisionShape3D collider;
    private MeshInstance3D mesh;

    [NodeName("Grab")]
    public Node3D GrabNode;

    public WorldItemData WorldItemData { get; set; }

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

    public void SavePositionInScene()
    {
        Debug.Log("Item3D.SavePositionInScene");
        Debug.Indent++;

        WorldItemData ??= new WorldItemData();
        WorldItemData.Save(this);
        Scene.Current.Data.WorldItems.Add(WorldItemData);

        Debug.Indent--;
    }

    public void UnsavePositionInScene()
    {
        Debug.Log("Item3D.UnsavePositionInScene");
        Debug.Indent++;

        if (WorldItemData == null)
        {
            Debug.LogError("Item did not have WorldItemData");
            Debug.Indent--;
            return;
        }

        Scene.Current.Data.WorldItems.Remove(WorldItemData);
        Debug.Indent--;
    }
}
