using Godot;
using System;

[GlobalClass]
public partial class DungeonResource : Resource
{
    [Export]
    public float RoomSize { get; set; }

    [Export(PropertyHint.File)]
    public string StartRoom { get; set; }

    [Export(PropertyHint.File)]
    public string EndRoom { get; set; }

    [Export(PropertyHint.File, ".tres")]
    public string[] RoomPaths { get; set; }

    public enum Type { Basement }

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