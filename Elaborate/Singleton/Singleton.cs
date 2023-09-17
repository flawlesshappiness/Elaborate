using Godot;
using System.Collections.Generic;

public static class Singleton
{
    private static Dictionary<string, Node> _singletons = new();

    public static T CreateInstance<T>(string path) where T : Node
    {
        path = "res://" + path;
        var scene = GD.Load(path) as PackedScene;
        var packed_scene = scene.Instantiate();
        Scene.Root.AddChild(packed_scene);
        var script = packed_scene.GetNodeInChildren<T>();
        return script;
    }

    public static T CreateSingleton<T>(string path) where T : Node
    {
        var type = typeof(T).Name;
        if (_singletons.ContainsKey(type))
        {
            return _singletons[type] as T;
        }

        var instance = CreateInstance<T>(path);
        _singletons.Add(type, instance);
        return instance;
    }

    public static T Get<T>() where T : Node
    {
        var type = typeof(T).Name;
        return _singletons.ContainsKey(type) ? _singletons[type] as T : throw new System.Exception($"Failed to get singleton with type: {type}");
    }

    public static bool TryGet<T>(out T result) where T : Node
    {
        try
        {
            result = Get<T>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}
