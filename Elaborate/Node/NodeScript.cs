using Godot;
using System;
using System.Reflection;

public partial class NodeScript : Node
{
    public override void _Ready()
    {
        FindNodesFromAttribute();

        base._Ready();
    }

    protected void FindNodesFromAttribute()
    {
        var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            try
            {
                FindNodeFromAttribute(field);
            }
            catch (Exception e)
            {
                Debug.LogError("FindNodes_NodePathAttribute: Node not found: " + e.Message);
            }
        }
    }

    private Node FindNodeFromAttribute(FieldInfo field)
    {
        if (Attribute.GetCustomAttribute(field, typeof(NodePathAttribute)) as NodePathAttribute is var nodePathAtt && nodePathAtt != null)
        {
            var node = GetNode(nodePathAtt.Value);
            _ = node ?? throw new NullReferenceException("Node was null");

            field.SetValue(this, node);
            return node;
        }
        else if (Attribute.GetCustomAttribute(field, typeof(NodeNameAttribute)) as NodeNameAttribute is var nodeNameAtt && nodeNameAtt != null)
        {
            var node = this.GetNodeInChildren<Node>(nodeNameAtt.Value);
            _ = node ?? throw new NullReferenceException("Node was null");

            field.SetValue(this, node);
            return node;
        }

        throw new NullReferenceException("No valid attribute was found");
    }
}
