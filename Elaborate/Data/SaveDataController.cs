using Godot;
using System.Collections.Generic;
using System.Text.Json;

public partial class SaveDataController : Node
{
    public static SaveDataController Instance => Singleton.TryGet<SaveDataController>(out var instance) ? instance : Create();

    public static SaveDataController Create() =>
        Singleton.CreateSingleton<SaveDataController>($"Data/{nameof(SaveDataController)}");

    private Dictionary<System.Type, SaveData> data_objects = new Dictionary<System.Type, SaveData>();

    public T Get<T>() where T : SaveData, new()
    {
        if (data_objects.ContainsKey(typeof(T)))
        {
            return data_objects[typeof(T)] as T;
        }
        else
        {
            var filename = typeof(T).Name;
            var path = $"user://{filename}.save";
            EnsureFileExists(path);
            var json = FileAccess.GetFileAsString(path);
            T data = string.IsNullOrEmpty(json) ? new T() : JsonSerializer.Deserialize<T>(json);
            data_objects.Add(typeof(T), data);
            Save<T>();
            Debug.Log("json: " + json);
            return data;
        }
    }

    public void SaveAll()
    {
        foreach (var kvp in data_objects)
        {
            Save(kvp.Key);
        }
    }

    public void Save<T>() where T : SaveData, new()
    {
        var data = Get<T>();
        Save(typeof(T));
    }

    public void Save(System.Type type)
    {
        var data = data_objects[type];
        var json = JsonSerializer.Serialize(data, type);
        var filename = type.Name;
        var path = $"user://{filename}.save";
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        file.StoreLine(json);
    }

    private void EnsureFileExists(string path)
    {
        if (!FileAccess.FileExists(path))
        {
            Debug.Log($"Created file at path: {path}");
            using (FileAccess.Open(path, FileAccess.ModeFlags.Write)) { }
        }
    }
}
