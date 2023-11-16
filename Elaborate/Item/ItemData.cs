using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export]
    public string PathItem3D { get; set; }

    [Export]
    public string PathItem2D { get; set; }

    public ItemData()
    {

    }

    public static ItemData Load(string name)
    {
        try
        {
            Debug.Log($"ItemData.Load({name})");
            var data = GD.Load<ItemData>($"res://Resources/Items/{name}.tres");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"  ItemData.Load: Failed to load ItemData");
            Debug.LogError($"  {e.Message}");
            return null;
        }
    }
}
