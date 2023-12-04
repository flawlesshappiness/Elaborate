using Godot;

[GlobalClass]
public partial class DungeonResource : Resource
{
    [Export]
    public string[] RoomPrefabs { get; set; }
}
