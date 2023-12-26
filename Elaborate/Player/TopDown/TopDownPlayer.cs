using Godot;

public partial class TopDownPlayer : Player3D
{
    [NodeName("EquipLeft")]
    public Node3D EquipLeftParent;

    [NodeName("EquipRight")]
    public Node3D EquipRightParent;

    private Item3D CreateItem(string id)
    {
        var item_data = ItemData.Load(id);
        var item = GD.Load<PackedScene>(item_data.PathItem3D).Instantiate() as Item3D;
        return item;
    }

    #region EQUIP
    public override void EquipItem(EquipItemArguments args)
    {
        base.EquipItem(args);

        Debug.Log($"FirstPersonPlayer.EquipItem({args.ItemId}, {args.Slot})");
        Debug.Indent++;

        var item = CreateItem(args.ItemId);
        var previous_item = args.WorldItem?.ItemOwner as Item3D;
        var parent = args.Slot == EquipmentSlot.LEFT ? EquipLeftParent : EquipRightParent;

        item.SetParent(Scene.Current);
        item.CollisionEnabled = false;
        item.ShadowsEnabled = false;

        SetEquippedItem(item, args.Slot);

        FinalizeEquipItem(item, parent, previous_item);

        Debug.Indent--;
    }

    private void FinalizeEquipItem(Item3D item, Node3D parent, Item3D previous_item)
    {
        item.SetParent(parent);
        item.Position = item.GrabNode.Position;
        item.Rotation = item.GrabNode.Rotation;

        if (previous_item != null)
        {
            previous_item.UnsavePositionInScene();
            previous_item.Visible = false;
        }
    }
    #endregion
}
