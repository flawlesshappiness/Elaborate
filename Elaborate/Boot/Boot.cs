using Godot;

public partial class Boot : Node
{
    private bool _initialized;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!_initialized)
            Initialize();
    }

    private void Initialize()
    {
        _initialized = true;
        InitializeScene();
        GameController.Create();
        DialogueController.Create();
        View.LoadSingleton<PauseView>();
        View.LoadSingleton<DialogueView>();
        View.LoadSingleton<PraySelectView>();
        LoadScene();
        LoadPlayer();

        SaveDataController.Instance.SaveAll();
    }

    private void InitializeScene()
    {
        Scene.Tree = GetTree();
        Scene.Root = Scene.Tree.Root;
        Scene.PauseLock.OnLocked += () => Scene.Tree.Paused = true;
        Scene.PauseLock.OnFree += () => Scene.Tree.Paused = false;
    }

    private void LoadScene()
    {
        Scene.Goto(Save.Game.Scene);
    }

    private void LoadPlayer()
    {
        var player = Player.Instance;
        if (player != null)
        {
            player.LoadData();
        }
    }
}
