using Godot;
using System.Linq;

public partial class PlayerEquipment : Node
{
    public static PlayerEquipment Instance { get; private set; }

    public string LeftItemId { get; set; }

    public string RightItemId { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;

        RegisterDebugActions();
    }

    private void RegisterDebugActions()
    {
        var category = "EQUIPMENT";

        Debug.RegisterAction(new DebugAction
        {
            Text = "Equip item",
            Category = category,
            Action = v =>
            {
                v.ContentSearch.Title = "Equip item";

                var ext = ".tres";
                var files = DirAccess.GetFilesAt("res://Resources/Items")
                    .Where(file => file.EndsWith(ext));

                foreach (var file in files)
                {
                    var name = System.IO.Path.GetFileName(file).Replace(ext, "");
                    v.ContentSearch.AddItem(name, () => Equip(new EquipItemArguments { ItemId = name, Animate = false, Slot = EquipmentSlot.LEFT }));
                }

                v.Content.Visible = true;
                v.ContentSearch.Visible = true;
            }
        });

        Debug.RegisterAction(new DebugAction
        {
            Text = "Remove left",
            Category = category,
            Action = v => RemoveItem(new RemoveItemArguments { Slot = EquipmentSlot.LEFT, Animate = false })
        });

        Debug.RegisterAction(new DebugAction
        {
            Text = "Remove right",
            Category = category,
            Action = v => RemoveItem(new RemoveItemArguments { Slot = EquipmentSlot.RIGHT, Animate = false })
        });

        Debug.RegisterAction(new DebugAction
        {
            Text = "Unequip left",
            Category = category,
            Action = v => Unequip(new UnequipItemArguments { Slot = EquipmentSlot.LEFT, Animate = false })
        });

        Debug.RegisterAction(new DebugAction
        {
            Text = "Unequip right",
            Category = category,
            Action = v => Unequip(new UnequipItemArguments { Slot = EquipmentSlot.RIGHT, Animate = false })
        });
    }

    public void LoadData()
    {
        Debug.Log($"PlayerEquipment.LoadData");
        Debug.Indent++;

        var data = Save.Game.Player;
        Debug.Log($"Left: {data.LeftItemId}");
        Debug.Log($"Right: {data.RightItemId}");

        if (!string.IsNullOrEmpty(data.LeftItemId))
        {
            Debug.Log($"LeftItemId: {data.LeftItemId}");

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
            Debug.Log($"RightItemId: {data.RightItemId}");

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

        Debug.Indent--;
    }

    public void SaveData(EquipmentSlot slot)
    {
        Save.Game.Player.LeftItemId = slot == EquipmentSlot.LEFT ? LeftItemId : Save.Game.Player.LeftItemId;
        Save.Game.Player.RightItemId = slot == EquipmentSlot.RIGHT ? RightItemId : Save.Game.Player.RightItemId;
    }

    private void SetItemId(EquipmentSlot slot, string item_id)
    {
        LeftItemId = slot == EquipmentSlot.LEFT ? item_id : LeftItemId;
        RightItemId = slot == EquipmentSlot.RIGHT ? item_id : RightItemId;
    }

    public void Equip(EquipItemArguments args)
    {
        Debug.Log($"PlayerEquipment.Equip: {args.Slot}");
        Debug.Indent++;

        if (HasItemInSlot(args.Slot))
        {
            Unequip(new UnequipItemArguments
            {
                Slot = args.Slot,
                Animate = args.Animate
            });
        }

        SetItemId(args.Slot, args.ItemId);
        SaveData(args.Slot);
        Player.Instance.EquipItem(args);

        Debug.Indent--;
    }

    public void Unequip(UnequipItemArguments args)
    {
        Debug.Log($"PlayerEquipment.Unequip: {args.Slot}");
        Debug.Indent++;

        SetItemId(args.Slot, null);
        SaveData(args.Slot);
        Player.Instance.UnequipItem(args);

        Debug.Indent--;
    }

    public void RemoveItem(RemoveItemArguments args)
    {
        Debug.Log($"PlayerEquipment.RemoveItem: {args.Slot}");
        Debug.Indent++;

        SetItemId(args.Slot, null);
        SaveData(args.Slot);
        Player.Instance.RemoveItem(args);

        Debug.Indent--;
    }

    public bool HasItemInSlot(EquipmentSlot slot)
    {
        return GetItemId(slot) != null;
    }

    public bool HasItem(string item_id)
    {
        return !string.IsNullOrEmpty(item_id) && LeftItemId == item_id || RightItemId == item_id;
    }

    public EquipmentSlot? GetItemSlot(string item_id)
    {
        if (LeftItemId == item_id) return EquipmentSlot.LEFT;
        if (RightItemId == item_id) return EquipmentSlot.RIGHT;
        return null;
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
