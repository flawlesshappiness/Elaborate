using Godot;
using System;
using System.Linq;

public partial class Scene : NodeScript
{
    private bool _initialized;

    public bool IsPaused => GetTree().Paused;

    public SceneData Data { get; private set; }

    public static Scene Current { get; set; }
    public static SceneTree Tree { get; set; }
    public static Window Root { get; set; }
    public static MultiLock PauseLock { get; } = new();
    public static bool AutoSave { get; set; } = true;

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

    public virtual void SaveData()
    {
        Debug.Log("Scene.SaveData");
        Debug.Indent++;
        Debug.Indent--;
    }

    public virtual void LoadData()
    {
        Debug.Log("Scene.LoadData");
        Debug.Indent++;

        foreach (var data in Data.Nodes)
        {
            try
            {
                LoadNode(data);

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        Debug.Indent--;
    }

    private void LoadNode(NodeData data)
    {
        Debug.Log("Scene.LoadNode");
        Debug.Indent++;
        Debug.Log($"path: {data.Path}");

        var node = GetNode(data.Path);
        Debug.Log($"node: {node}");

        if (node is Node3D n3)
        {
            data.LoadNode3D(n3);
        }
        else if (node is Node2D n2)
        {
            data.LoadNode2D(n2);
        }

        Debug.Indent--;
    }

    public void SaveNode(string path) =>
        SaveNode(GetNode(path));

    public void SaveNode(Node node)
    {
        Debug.Log("Scene.SaveNode");
        Debug.Indent++;

        if (node == null)
        {
            Debug.LogError("Node was null");
            Debug.Indent--;
            return;
        }

        var path = node.GetPath();
        var data = Data.GetOrCreateNode(path);

        if (node is Node3D n3)
        {
            data.SaveNode3D(n3);
            Debug.Indent--;
            return;
        }

        if (node is Node2D n2)
        {
            data.SaveNode2D(n2);
            Debug.Indent--;
            return;
        }

        Debug.LogError($"Unhandled node type: {node}");
        Debug.Indent--;
    }

    public static T CreateInstance<T>(string path) where T : Scene =>
        Singleton.LoadInstance<T>(path);

    public static Scene Goto(string scene_name)
    {
        Debug.Log($"Scene.Goto: {scene_name}");
        Debug.Indent++;

        if (Current != null)
        {
            if (AutoSave)
            {
                Current.SaveData();
            }

            Current.QueueFree();
        }

        Current = CreateInstance<Scene>($"Scenes/{scene_name}");
        Current.Data = GetOrCreateSceneData(scene_name);
        Current.LoadData();
        Player.LoadData();

        Debug.Indent--;
        return Current;
    }

    public static T Goto<T>() where T : Scene =>
        Goto(typeof(T).Name) as T;

    public void Destroy() => Destroy(this);

    public static void Destroy(Scene scene)
    {
        scene.OnDestroy();
        scene.QueueFree();
    }

    private static SceneData GetOrCreateSceneData(string scene_name)
    {
        var data = Save.Game.Scenes.FirstOrDefault(d => d.SceneName == scene_name);

        if (data == null)
        {
            Debug.Log("New scene data created!");
            data = new SceneData
            {
                SceneName = scene_name,
            };
            Save.Game.Scenes.Add(data);
        }

        return data;
    }
}
