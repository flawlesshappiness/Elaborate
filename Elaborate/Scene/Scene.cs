using Godot;

public partial class Scene : NodeScript
{
    private bool _initialized;

    public bool IsPaused => GetTree().Paused;

    public static Scene Current { get; set; }
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

    public static T CreateInstance<T>(string path) where T : Scene =>
        Singleton.LoadInstance<T>(path);

    public static Scene Goto(string scenename)
    {
        Debug.Log($"Scene.Goto: {scenename}");

        if (Current != null)
        {
            Current.QueueFree();
        }

        var scene = CreateInstance<Scene>($"Scenes/{scenename}");
        Current = scene;
        return scene;
    }

    public static T Goto<T>() where T : Scene =>
        Goto(typeof(T).Name) as T;

    public void Destroy() => Destroy(this);

    public static void Destroy(Scene scene)
    {
        scene.OnDestroy();
        scene.QueueFree();
    }
}
