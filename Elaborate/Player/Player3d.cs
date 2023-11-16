using Godot;

public partial class Player3D : CharacterBody3D, IPlayer
{
    public PlayerMovement Movement { get; private set; }

    public PlayerInteract Interact { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        NodeScript.FindNodesFromAttribute(this, GetType());
        Player.Instance = this;
        Movement = this.GetNodeInChildren<PlayerMovement>();
        Interact = this.GetNodeInChildren<PlayerInteract>();
    }

    protected void VerifySaveData()
    {
        Debug.Log("Player.VerifySaveData");

        if (Save.Game.Player == null)
        {
            Save.Game.Player = new PlayerData();
            Debug.Log($"  Created new {nameof(PlayerData)}");
        }
    }

    public virtual void SaveData()
    {
        Debug.Log("Player.SaveData");

        VerifySaveData();
    }

    public virtual void LoadData()
    {
        Debug.Log("Player.LoadData");

        if (Save.Game.Player == null)
        {
            Debug.Log("  PlayerData is null");
            return;
        }
    }

    public virtual void MoveToNode(Node node)
    {
        var n3 = node as Node3D;

        if (n3 == null)
        {
            Debug.Log($"Player3d.MoveToNode: Failed to start at node: {node.Name}");
            return;
        }

        GlobalPosition = n3.GlobalPosition;
    }

    public virtual void EquipItem(string id, EquipmentSlot slot)
    {
        Debug.Log($"Player3D.EquipItem({id}, {slot})");
    }

    public virtual void UnequipItem(EquipmentSlot slot)
    {
        Debug.Log($"Player3D.UnequipItem({slot})");
    }
}
