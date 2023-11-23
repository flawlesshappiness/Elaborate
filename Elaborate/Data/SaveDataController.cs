using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class SaveDataController : Node
{
    public static SaveDataController Instance => Singleton.TryGet<SaveDataController>(out var instance) ? instance : Create();

    public static SaveDataController Create() =>
        Singleton.CreateSingleton<SaveDataController>($"Data/{nameof(SaveDataController)}");

    private Dictionary<System.Type, SaveData> data_objects = new Dictionary<System.Type, SaveData>();

    public override void _Ready()
    {
        base._Ready();
        RegisterDebugActions();
    }

    private void RegisterDebugActions()
    {
        var view = View.Get<DebugView>();
        view.RegisterAction("Clear ALL save data", v => ClearAllSaveData());
        view.RegisterAction("Clear current scene data", v => ClearCurrentSceneData());
        view.RegisterAction("Clear player data", v => ClearPlayerData());
        view.RegisterAction("Clear player equipment data", v => ClearPlayerEquipmentData());
    }

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
            Debug.Log("json: " + json);

            T data = string.IsNullOrEmpty(json) ? new T() : JsonConvert.DeserializeObject<T>(json);
            data_objects.Add(typeof(T), data);

            Save<T>();

            return data;
        }
    }

    public void SaveAll()
    {
        Debug.Log("SaveDataController.SaveAll");

        foreach (var kvp in data_objects)
        {
            Debug.Log($"  {kvp.Key}");
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
        var json = JsonConvert.SerializeObject(data);
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

    private void ClearSaveData(Type type)
    {
        var data = Activator.CreateInstance(type) as SaveData;
        data_objects[type] = data;
        Save(type);
    }

    private void ClearAllSaveData()
    {
        foreach (var kvp in data_objects)
        {
            ClearSaveData(kvp.Key);
        }

        GetTree().Quit();
    }

    private void ClearCurrentSceneData()
    {
        var scene_data = Scene.Current.Data;
        var data = Get<GameSaveData>();
        data.Scenes.Remove(scene_data);
        Save<GameSaveData>();

        // Reload
        Scene.AutoSave = false;
        Scene.Goto(scene_data.SceneName);
        Scene.AutoSave = true;
    }

    private void ClearPlayerData()
    {
        var data = Get<GameSaveData>();
        data.Player = new();
        Scene.Goto(data.Scene);
    }

    private void ClearPlayerEquipmentData()
    {
        var data = Get<GameSaveData>();
        data.Player.LeftItemId = null;
        data.Player.RightItemId = null;
        Scene.Goto(data.Scene);
    }
}
