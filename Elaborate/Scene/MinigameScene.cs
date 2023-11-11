public partial class MinigameScene : Scene
{
    public virtual void CompleteMinigame()
    {
        Goto(Save.Game.Scene);
        Player.Instance.LoadData();
    }
}
