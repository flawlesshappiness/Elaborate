using Godot;

public partial class Player3d : CharacterBody3D, IPlayer
{
    public PlayerMovement Movement { get; private set; }

    public PlayerInteract Interact { get; private set; }


    public override void _Ready()
    {
        base._Ready();
        Player.Instance = this;
        Movement = this.GetNodeInChildren<PlayerMovement>();
        Interact = this.GetNodeInChildren<PlayerInteract>();
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
