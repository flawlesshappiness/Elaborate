using Godot;

[GlobalClass]
public partial class DungeonRoomResource : Resource
{
    [Export(PropertyHint.File)]
    public string Path { get; set; }

    [Export]
    public float ChanceToSpawn { get; set; }
}
