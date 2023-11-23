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

    public void SaveData()
    {
        Debug.Log("PlayerEquipment.SaveData");

        Save.Game.Player.LeftItemId = LeftItemId;
        Save.Game.Player.RightItemId = RightItemId;

        Debug.Log($"  LeftItemId: {LeftItemId}");
        Debug.Log($"  RightItemId: {RightItemId}");
    }

    public void LoadData()
    {
        Debug.Log($"PlayerEquipment.LoadData");

        var data = Save.Game.Player;

        if (!string.IsNullOrEmpty(data.LeftItemId))
        {
            Debug.Log($"  LeftItemId: {data.LeftItemId}");

            Equip(new EquipItemArguments
            {
                ItemId = data.LeftItemId,
                Slot = EquipmentSlot.LEFT,
                Animate = false
            });
        }
        else
        {
            Unequip(new UnequipItemArguments
            {
                Slot = EquipmentSlot.LEFT,
                Animate = false
            });
        }

        if (!string.IsNullOrEmpty(data.RightItemId))
        {
            Debug.Log($"  RightItemId: {data.RightItemId}");

            Equip(new EquipItemArguments
            {
                ItemId = data.RightItemId,
                Slot = EquipmentSlot.RIGHT,
                Animate = false
            });
        }
        else
        {
            Unequip(new UnequipItemArguments
            {
                Slot = EquipmentSlot.RIGHT,
                Animate = false,
            });
        }
    }

    public void Equip(EquipItemArguments args)
    {
        if (HasItem(args.Slot))
        {
            Unequip(new UnequipItemArguments
            {
                Slot = args.Slot,
                Animate = args.Animate
            });
        }

        LeftItemId = args.Slot == EquipmentSlot.LEFT ? args.ItemId : LeftItemId;
        RightItemId = args.Slot == EquipmentSlot.RIGHT ? args.ItemId : RightItemId;
        SaveData();

        Player.Instance.EquipItem(args);
    }

    public void Unequip(UnequipItemArguments args)
    {
        LeftItemId = args.Slot == EquipmentSlot.LEFT ? null : LeftItemId;
        RightItemId = args.Slot == EquipmentSlot.RIGHT ? null : RightItemId;
        SaveData();

        Player.Instance.UnequipItem(args);
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
