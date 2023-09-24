using Godot;

public partial class Player : CharacterBody3D
{
    public static readonly MultiLock InteractLock = new MultiLock();

    public static Player Instance { get; private set; }

    public PlayerMovement Movement { get; private set; }

    public PlayerInteract Interact { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        Movement = this.GetNodeInChildren<PlayerMovement>();
        Interact = this.GetNodeInChildren<PlayerInteract>();
    }
}
