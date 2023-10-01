using Godot;

public static class ColorRectExtensions
{
    public static Coroutine TweenProperty(this ColorRect control, ColorRectProperty property, float duration, Variant end) =>
        TweenEnumerator.TweenProperty(control, property.GetValue(), duration, end);

    public static Coroutine TweenProperty(this ColorRect control, ColorRectProperty property, float duration, Variant start, Variant end) =>
        TweenEnumerator.TweenProperty(control, property.GetValue(), duration, start, end);
}

public enum ColorRectProperty
{
    [StringValue("color")]
    Color
}