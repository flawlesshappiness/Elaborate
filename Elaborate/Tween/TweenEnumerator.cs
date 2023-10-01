using Godot;
using System.Collections;

public static class TweenEnumerator
{
    private static Node _unscaled_node;

    private static Node UnscaledNode => _unscaled_node ?? (_unscaled_node = CreateUnscaledNode());

    private static Node CreateUnscaledNode()
    {
        var node = new Node();
        node.Name = "UnscaledNode (TweenEnumerator)";
        node.ProcessMode = Node.ProcessModeEnum.Always;
        Scene.Root.AddChild(node);
        return node;
    }

    public static Coroutine TweenProperty(GodotObject go, string property, float duration, Variant end)
    {
        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            var tween = UnscaledNode.CreateTween();
            tween.TweenProperty(go, property, end, duration);
            yield return new WaitForSeconds(duration);
        }
    }

    public static Coroutine TweenProperty(GodotObject go, string property, float duration, Variant start, Variant end)
    {
        go.Set(property, start);
        return TweenProperty(go, property, duration, end);
    }
}
