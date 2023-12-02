using Godot;

public class WorldItemData : IDataListItem<IItem>
{
    public string Id { get; set; }

    public string ItemId { get; set; }

    public bool Is3D { get; set; }

    public Vector3 GlobalPosition { get; set; }

    public Vector3 GlobalRotation { get; set; }

    public void Load()
    {
        Debug.Log("WorldItemData.Load");
        Debug.Indent++;

        if (Is3D)
        {
            Load3D();
        }
        else
        {
            Load2D();
        }

        Debug.Indent--;
    }

    private void Load3D()
    {
        Debug.Log("WorldItemData.Load3D");
        Debug.Indent++;

        var res = ItemData.Load(ItemId);
        var item = GD.Load<PackedScene>(res.PathItem3D).Instantiate() as Item3D;
        item.SetParent(Scene.Current);
        item.GlobalPosition = GlobalPosition;
        item.GlobalRotation = GlobalRotation;
        item.WorldItemData = this;

        Debug.Log($"Position: {item.GlobalPosition}");
        Debug.Log($"Rotation: {item.GlobalRotation}");
        Debug.Log($"WorldItemData: {item.WorldItemData}");

        Debug.Indent--;
    }

    private void Load2D()
    {
        Debug.LogError("LOAD2D IS NOT IMPLEMENTED");
        /*
        var res = ItemData.Load(ItemId);
        var item = GD.Load<PackedScene>(res.PathItem2D).Instantiate() as Item2D;
        item.SetParent(Scene.Current);
        item.GlobalPosition = GlobalPosition;
        item.GlobalRotation = GlobalRotation;
        */
    }

    public void Save(IItem reference)
    {
        Debug.Log("WorldItemData.Save");
        Debug.Indent++;

        ItemId = reference.ItemDataId;

        var node = reference.ItemOwner;

        var n3 = node as Node3D;
        if (n3 != null)
        {
            Save3D(n3);
            Debug.Indent--;
            return;
        }

        var n2 = node as Node2D;
        if (n2 != null)
        {
            Save2D(n2);
            Debug.Indent--;
            return;
        }

        Debug.LogError("Node type not supported");
        Debug.Indent--;
    }

    private void Save3D(Node3D node)
    {
        Debug.Log("WorldItemData.Save3D");
        Debug.Indent++;

        Is3D = true;
        GlobalPosition = node.GlobalPosition;
        GlobalRotation = node.GlobalRotation;

        Debug.Indent--;
    }

    private void Save2D(Node2D node)
    {
        Debug.Log("WorldItemData.Save2D");
        Debug.Indent++;

        Is3D = false;
        GlobalPosition = node.GlobalPosition.ToVector3();
        GlobalRotation = new Vector3 { Z = node.GlobalRotation };

        Debug.Indent--;
    }
}
