public partial class MinigameScene : Scene
{
    public virtual void CompleteMinigame()
    {
        Scene.Goto(Save.Game.Scene);
    }
}
