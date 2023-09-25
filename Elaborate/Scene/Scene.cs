using Godot;

public partial class Scene : Node
{
    private bool _initialized;

    public bool IsPaused => GetTree().Paused;

    public static SceneTree Tree { get; set; }
    public static Window Root { get; set; }
    public static MultiLock PauseLock { get; } = new();

    protected virtual void OnInitialize() { }
    protected virtual void OnDestroy() { }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!_initialized)
            Initialize();
    }

    private void Initialize()
    {
        _initialized = true;
        OnInitialize();
    }

    public static T CreateInstance<T>() where T : Scene =>
        Singleton.LoadInstance<T>($"Scenes/{typeof(T).Name}");

    public void Destroy() => Destroy(this);

    public static void Destroy(Scene scene)
    {
        scene.OnDestroy();
        scene.QueueFree();
    }
}
