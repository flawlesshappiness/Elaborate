public class LockData : IDataListItem<InteractableLock>
{
    public string Id { get; set; }

    public string Path { get => Id; set => Id = value; }

    public bool Locked { get; set; }

    public void Log()
    {
        Debug.Log($"Path: {Path}");
        Debug.Log($"Locked: {Locked}");
    }

    public void Load()
    {
        Debug.Log($"LockData.Load");
        Debug.Indent++;

        Debug.Log($"Path: {Path}");

        var node = Scene.Current.GetNode(Path) as InteractableLock;
        Debug.Log($"Lock: {node}");

        if (node != null)
        {
            node.Locked = Locked;
            Debug.Log($"Locked: {node.Locked}");
        }

        Debug.Indent--;
    }

    public void Save(InteractableLock reference)
    {
        Debug.Log($"LockData.Save");
        Debug.Indent++;

        Locked = reference.Locked;

        Log();

        Debug.Indent--;
    }
}
