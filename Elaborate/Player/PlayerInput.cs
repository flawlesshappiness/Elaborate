using Godot;

public partial class PlayerInput : Node
{
    public MultiLock MouseVisibleLock = new MultiLock();

    public static PlayerInput Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        MouseVisibleLock.OnLocked += OnMouseVisibleLocked;
        MouseVisibleLock.OnFree += OnMouseVisibleFree;

        Instance = this;
    }

    public PlayerInput()
    {
        MouseVisibleLock.OnLocked += OnMouseVisibleLocked;
        MouseVisibleLock.OnFree += OnMouseVisibleFree;
    }

    private void OnMouseVisibleFree()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void OnMouseVisibleLocked()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }
}
