using Godot;

public static partial class NodeExtensions
{
    public static Coroutine TweenProperty(this Node control, NodeProperty property, float duration, Variant end) =>
        TweenEnumerator.TweenProperty(control, property.GetValue(), duration, end);

    public static Coroutine TweenProperty(this Node control, NodeProperty property, float duration, Variant start, Variant end) =>
        TweenEnumerator.TweenProperty(control, property.GetValue(), duration, start, end);
}

public enum NodeProperty
{
    [StringValue("position")]
    Position,

    [StringValue("global_position")]
    GlobalPosition,

    [StringValue("rotation_degrees")]
    Rotation,

    [StringValue("scale")]
    Scale,
}