using Godot;

public partial class Player3D : CharacterBody3D, IPlayer
{
    public PlayerMovement Movement { get; private set; }

    public PlayerInteract Interact { get; private set; }

    public Item3D EquippedItemLeft { get; protected set; }

    public Item3D EquippedItemRight { get; protected set; }

    public override void _Ready()
    {
        base._Ready();
        NodeScript.FindNodesFromAttribute(this, GetType());
        Player.Instance = this;
        Movement = this.GetNodeInChildren<PlayerMovement>();
        Interact = this.GetNodeInChildren<PlayerInteract>();
    }

    public virtual void SaveData()
    {
        Debug.Log("Player.SaveData");
        Debug.Indent++;

        var data = Save.Game.Player;
        data.Position = GlobalPosition;

        Debug.Indent--;
    }

    public virtual void LoadData()
    {
        Debug.Log("Player.LoadData");
        Debug.Indent++;

        LoadStartPosition();

        Debug.Indent--;
    }

    private void LoadStartPosition()
    {
        Debug.Log("Player3D.LoadStartPosition");
        Debug.Indent++;

        var data = Save.Game.Player;
        var position = data.Position;

        if (data.Position != null)
        {
            GlobalPosition = position.Value;

            Debug.Indent--;
            return;
        }

        if (data.StartNode != null)
        {
            Debug.Log($"Finding node: {data.StartNode}");
            var node = Scene.Current.GetNodeInChildren<Node>(data.StartNode);
            if (node != null)
            {
                MoveToNode(node);

                Debug.Indent--;
                return;
            }
        }

        Debug.LogError("Failed to load position");
        Debug.Indent--;
    }

    public virtual void MoveToNode(Node node)
    {
        var n3 = node as Node3D;

        if (n3 == null)
        {
            Debug.Log($"Player3d.MoveToNode: Failed to start at node: {node?.Name}");
            return;
        }

        GlobalPosition = n3.GlobalPosition;
    }

    public virtual void EquipItem(EquipItemArguments args)
    {
        Debug.Log($"Player3D.EquipItem({args.ItemId}, {args.Slot})");
    }

    public virtual void UnequipItem(UnequipItemArguments args)
    {
        Debug.Log($"Player3D.UnequipItem({args.Slot})");
        Debug.Indent++;

        if (args.Slot == EquipmentSlot.LEFT)
        {
            Save.Game.Player.LeftItemId = null;
        }
        else if (args.Slot == EquipmentSlot.RIGHT)
        {
            Save.Game.Player.RightItemId = null;
        }

        Debug.Indent--;
    }

    protected void SetEquippedItem(Item3D item, EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.LEFT:
                EquippedItemLeft = item;
                break;

            case EquipmentSlot.RIGHT:
                EquippedItemRight = item;
                break;
        }
    }

    public Item3D GetEquippedItem(EquipmentSlot slot)
    {
        switch (slot)
        {
            case EquipmentSlot.LEFT: return EquippedItemLeft;
            case EquipmentSlot.RIGHT: return EquippedItemRight;
            default: return null;
        }
    }
}