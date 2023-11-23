using Godot;
using System.Collections.Generic;

public class NodeData
{
    public string Path { get; set; }

    public Dictionary<string, Variant> Properties { get; set; } = new();
}
