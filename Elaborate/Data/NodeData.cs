using Godot;

public class NodeData
{
    public string Path { get; set; }

    public bool Visible { get; set; }

    public void Log()
    {
        Debug.Log($"Path: {Path}");
        Debug.Log($"Visible: {Visible}");
    }

    public void SaveNode3D(Node3D node)
    {
        Debug.Log("NodeData.SaveNode3D");
        Debug.Indent++;

        Visible = node.Visible;

        SaveNode(node);

        Debug.Indent--;
    }

    public void SaveNode2D(Node2D node)
    {
        Debug.Log("NodeData.SaveNode2D");
        Debug.Indent++;

        Visible = node.Visible;

        SaveNode(node);

        Debug.Indent--;
    }

    private void SaveNode(Node node)
    {
        Debug.Log("NodeData.SaveNode");
        Debug.Indent++;

        var nodepath = node.GetPath();
        var path = nodepath.ToString().Replace(Scene.Current.Name, Scene.Current.Data.SceneName);
        Path = path;

        Log();

        Debug.Indent--;
    }

    public void LoadNode3D(Node3D node)
    {
        Debug.Log("NodeData.LoadNode3D");
        Debug.Indent++;

        node.Visible = Visible;

        LoadNode(node);

        Debug.Indent--;
    }

    public void LoadNode2D(Node2D node)
    {
        Debug.Log("NodeData.LoadNode2D");
        Debug.Indent++;

        node.Visible = Visible;

        LoadNode(node);

        Debug.Indent--;
    }

    private void LoadNode(Node node)
    {
        Log();
    }
}
