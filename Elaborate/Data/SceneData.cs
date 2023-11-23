using System.Collections.Generic;
using System.Linq;

public class SceneData
{
    public string SceneName { get; set; }

    public List<NodeData> Nodes { get; set; } = new();

    public NodeData GetOrCreateNode(string node_path)
    {
        var data = Nodes.FirstOrDefault(d => d.Path == node_path);

        if (data == null)
        {
            data = new NodeData
            {
                Path = node_path
            };

            Nodes.Add(data);
        }

        return data;
    }
}
