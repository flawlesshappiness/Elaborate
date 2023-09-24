using Godot;

public partial class sign_paperwork_001 : MinigameScene
{
    private DrawController _draw;

    public override void _Ready()
    {
        base._Ready();
        _draw = this.GetNodeInChildren<DrawController>();
        _draw.OnPointerUp += OnDraw;
    }

    private void OnDraw(InputEventMouseButton e)
    {
        CompleteMinigame();
    }
}
