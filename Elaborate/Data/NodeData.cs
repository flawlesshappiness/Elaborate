using Godot;

public class NodeData
{
    public string Path { get; set; }

    public bool Visible { get; set; }

    public void Log()
    {
        Debug.Log($"  Path: {Path}");
        Debug.Log($"  Visible: {Visible}");
    }

    public void SaveNode3D(Node3D node)
    {
        Debug.Log("NodeData.SaveNode3D");

        Visible = node.Visible;

        SaveNode(node);
    }

    public void SaveNode2D(Node2D node)
    {
        Debug.Log("NodeData.SaveNode2D");

        Visible = node.Visible;

        SaveNode(node);
    }

    private void SaveNode(Node node)
    {
        Debug.Log("NodeData.SaveNode");

        var nodepath = node.GetPath();
        var path = nodepath.ToString().Replace(Scene.Current.Name, Scene.Current.Data.SceneName);
        Path = path;

        Log();
    }

    public void LoadNode3D(Node3D node)
    {
        node.Visible = Visible;

        LoadNode(node);
    }

    public void LoadNode2D(Node2D node)
    {
        node.Visible = Visible;

        LoadNode(node);
    }

    private void LoadNode(Node node)
    {
        Log();
    }
}
