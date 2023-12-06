using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class DungeonResource : Resource
{
    [Export]
    public string DungeonPath { get; set; }

    private const string PREFAB_EXTENSION = ".tscn";
    private const string ROOMS_DIR = "Rooms/";
    private const string CORRIDORS_DIR = "Corridors/";

    public enum Type { Basement }

    public List<string> GetRooms()
    {
        var path = $"{DungeonPath}/{ROOMS_DIR}";
        return GetPrefabs(path);
    }

    public List<string> GetCorridors()
    {
        var path = $"{DungeonPath}/{CORRIDORS_DIR}";
        return GetPrefabs(path);
    }

    private List<string> GetPrefabs(string path)
    {
        return DirAccess.GetFilesAt(path)
            .Where(file => file.EndsWith(PREFAB_EXTENSION))
            .Select(file => path + file)
            .ToList();
    }

    public static DungeonResource Load(Type type)
    {
        try
        {
            var name = type.ToString();
            Debug.Log($"DungeonResource.Load({name})");
            Debug.Indent++;
            var data = GD.Load<DungeonResource>($"res://Resources/Dungeons/{name}.tres");
            Debug.Indent--;
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"DungeonResource.Load: Failed to load resource");
            Debug.LogError($"{e.Message}");
            Debug.Indent--;
            return null;
        }
    }
}
