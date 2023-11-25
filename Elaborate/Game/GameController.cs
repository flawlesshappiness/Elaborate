using Godot;

public partial class GameController : Node
{
    public static GameController Instance => Singleton.TryGet<GameController>(out var instance) ? instance : Create();

    public static GameController Create() =>
        Singleton.CreateSingleton<GameController>($"Game/{nameof(GameController)}");

    public static bool AutoSave { get; set; } = true;

    public System.Action OnQuit;

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == NotificationWMCloseRequest)
        {
            OnWindowClose();
        }
    }

    private void SaveAll()
    {
        // Scene
        Scene.Current.SaveData();

        // Player
        Player.SaveData();

        // Serialize data
        SaveDataController.Instance.SaveAll();
    }

    private void OnWindowClose()
    {
        Debug.Log("GameController.OnWindowClose");
        Debug.Indent++;

        // Serialize save data
        if (AutoSave)
        {
            SaveAll();
        }

        // Event
        OnQuit?.Invoke();

        // Quit
        Debug.Log("Quit");
        Debug.Indent--;
    }

    private void CloseWindow()
    {
        OnWindowClose();
        GetTree().Quit();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                CloseWindow();
            }
        }
    }
}
