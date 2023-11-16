using Godot;

public partial class PlayerEquipment : Node
{
    public static PlayerEquipment Instance { get; private set; }

    public string LeftItemId { get; set; }

    public string RightItemId { get; set; }

    public bool HasLeftItem => LeftItemId != null;

    public bool HasRightItem => RightItemId != null;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public void Equip(string id, EquipmentSlot slot)
    {
        LeftItemId = slot == EquipmentSlot.LEFT ? id : LeftItemId;
        RightItemId = slot == EquipmentSlot.RIGHT ? id : RightItemId;
        Player.Instance.EquipItem(id, slot);
    }

    public void Unequip(EquipmentSlot slot)
    {
        LeftItemId = slot == EquipmentSlot.LEFT ? null : LeftItemId;
        RightItemId = slot == EquipmentSlot.RIGHT ? null : RightItemId;
        Player.Instance.UnequipItem(slot);
    }
}

public enum EquipmentSlot
{
    LEFT, RIGHT
}
