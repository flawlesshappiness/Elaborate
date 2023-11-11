using Godot;

public abstract partial class Player : CharacterBody3D
{
    public static readonly MultiLock InteractLock = new MultiLock();

    public static readonly PlayerInput Input = new PlayerInput();

    public static Player Instance { get; private set; }

    public PlayerMovement Movement { get; private set; }

    public PlayerInteract Interact { get; private set; }

    public PlayerPray Pray { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        Movement = this.GetNodeInChildren<PlayerMovement>();
        Interact = this.GetNodeInChildren<PlayerInteract>();
        Pray = this.GetNodeInChildren<PlayerPray>();
    }

    // Override to implement another type of PlayerData
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
}
