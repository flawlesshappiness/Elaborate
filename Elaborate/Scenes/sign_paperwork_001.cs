public partial class sign_paperwork_001 : MinigameScene
{
    private DrawController _draw;

    public override void _Ready()
    {
        base._Ready();

        Player.Input.MouseVisibleLock.AddLock(nameof(sign_paperwork_001));

        _draw = this.GetNodeInChildren<DrawController>();
        _draw.OnDrawLine += _ => OnDraw();
    }

    private void OnDraw()
    {
        Player.Input.MouseVisibleLock.RemoveLock(nameof(sign_paperwork_001));
        CompleteMinigame();
    }
}
