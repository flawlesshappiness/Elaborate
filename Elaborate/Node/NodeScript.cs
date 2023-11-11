using Godot;
using System;
using System.Linq;
using System.Reflection;

public partial class NodeScript : Node
{
    public override void _Ready()
    {
        FindNodesFromAttribute(this, GetType());

        base._Ready();
    }

    public static void FindNodesFromAttribute(Node root, Type type)
    {
        Debug.Log($"FindNodes_NodePathAttribute ({root.Name})");

        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            if (field.CustomAttributes.Count() == 0) continue;

            try
            {
                FindNodeFromAttribute(root, field);
                Debug.Log($"  Found value for field: {field.Name}");
            }
            catch (Exception e)
            {
                Debug.LogError($"  {field.Name} field value not found: " + e.Message);
            }
        }
    }

    private static Node FindNodeFromAttribute(Node root, FieldInfo field)
    {
        if (Attribute.GetCustomAttribute(field, typeof(NodePathAttribute)) as NodePathAttribute is var nodePathAtt && nodePathAtt != null)
        {
            var node = root.GetNode(nodePathAtt.Value);
            _ = node ?? throw new NullReferenceException("Node was null");

            field.SetValue(root, node);
            return node;
        }
        else if (Attribute.GetCustomAttribute(field, typeof(NodeNameAttribute)) as NodeNameAttribute is var nodeNameAtt && nodeNameAtt != null)
        {
            var node = root.GetNodeInChildren<Node>(nodeNameAtt.Value);
            _ = node ?? throw new NullReferenceException("Node was null");

            field.SetValue(root, node);
            return node;
        }

        throw new NullReferenceException($"No valid attribute was found");
    }
}
