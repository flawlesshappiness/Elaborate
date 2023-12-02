using Godot;

public class SceneData
{
    public string SceneName { get; set; }

    public DataList<NodeData, Node> Nodes { get; set; } = new();

    public DataList<LockData, InteractableLock> Locks { get; set; } = new();

    public DataList<WorldItemData, IItem> WorldItems { get; set; } = new();
}
