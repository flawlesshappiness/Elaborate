using Godot;

public partial class PlayerEquipment : Node
{
    public static PlayerEquipment Instance { get; private set; }

    public string LeftItemId { get; set; }

    public string RightItemId { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public void Equip(EquipItemArguments args)
    {
        if (HasItem(args.Slot))
        {
            Unequip(args.Slot);
        }

        LeftItemId = args.Slot == EquipmentSlot.LEFT ? args.ItemId : LeftItemId;
        RightItemId = args.Slot == EquipmentSlot.RIGHT ? args.ItemId : RightItemId;
        Player.Instance.EquipItem(args);
    }

    public void Unequip(EquipmentSlot slot)
    {
        LeftItemId = slot == EquipmentSlot.LEFT ? null : LeftItemId;
        RightItemId = slot == EquipmentSlot.RIGHT ? null : RightItemId;
        Player.Instance.UnequipItem(new UnequipItemArguments
        {
            Slot = slot,
            Animate = true,
        });
    }

    public bool HasItem(EquipmentSlot slot)
    {
        return GetItemId(slot) != null;
    }

    public string GetItemId(EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.LEFT: return LeftItemId;
            case EquipmentSlot.RIGHT: return RightItemId;
            default: return null;
        }
    }
}

public enum EquipmentSlot
{
    LEFT, RIGHT
}
