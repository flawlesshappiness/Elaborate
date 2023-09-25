using Godot;
using System.Collections.Generic;

public static class NodeExtensions
{
    public static T GetNodeInChildren<T>(this Node node) where T : Node
    {
        if (node.TryGetNode<T>(out var result)) return result;

        foreach (var child in node.GetChildren())
        {
            T script = child.GetNodeInChildren<T>();
            if (script != null)
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

            current = current.GetParent();
        }

        return null;
    }

    public static List<T> GetNodesInParents<T>(this Node node) where T : Node
    {
        var list = new List<T>();
        var current = node;
        while (current != null)
        {
            if (current.TryGetNode(out T script))
            {
                list.Add(script);
            }

            current = current.GetParent();
        }

        return list;
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
