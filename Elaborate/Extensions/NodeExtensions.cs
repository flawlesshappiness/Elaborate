using Godot;

public static class NodeExtensions
{
    public static T GetNodeInChildren<T>(this Node node) where T : Node
    {
        if (TryGetNode<T>(node, out var result)) return result;

        var children = node.GetChildren();
        foreach (var child in children)
        {
            if (child.TryGetNode(out T script))
            {
                return script;
            }
        }

        return null;
    }

    public static T GetNodeInParents<T>(this Node node) where T : Node
    {
        var current = node;
        while (current != null)
        {
            if (current.TryGetNode(out T script))
            {
                return script;
            }

            current = current.GetNode("../");
        }

        return null;
    }

    public static bool TryGetNode<T>(this Node parent, out T script) where T : Node
    {
        script = null;

        try
        {
            script = parent.GetNode<T>(parent.GetPath());
            return true;
        }
        catch
        {
            return false;
        }
    }
}
