using Godot;
using System;

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

    public static Scene Goto(string scene_name, string start_node)
    {
        Debug.Log($"Scene.Goto: {scene_name}, {start_node}");

        var scene = Goto(scene_name);

        StartPlayerAtNode(start_node);

        return scene;
    }

    public static Scene Goto(string scene_name)
    {
        Debug.Log($"Scene.Goto: {scene_name}");

        if (Current != null)
        {
            Current.QueueFree();
        }

        var scene = CreateInstance<Scene>($"Scenes/{scene_name}");
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

    public static void StartPlayerAtNode(string start_node)
    {
        try
        {
            Debug.Log($"Scene.StartPlayerAtNode: {start_node}");

            if (Player.Is3D)
            {
                var p3d = Player.Instance as Player3d;
                var n3d = Current.GetNodeInChildren<Node3D>(start_node);
                _ = n3d ?? throw new NullReferenceException("start_node was null");
                p3d.GlobalPosition = n3d.GlobalPosition;
            }
            else if (Player.Is2D)
            {
                var p2d = Player.Instance as Player2d;
                var n2d = Current.GetNodeInChildren<Node2D>(start_node);
                _ = n2d ?? throw new NullReferenceException("start_node was null");
                p2d.GlobalPosition = n2d.GlobalPosition;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"  Failed to start player at node: {e.Message}");
        }
    }
}
