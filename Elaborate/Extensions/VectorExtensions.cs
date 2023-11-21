using Godot;

public static class VectorExtensions
{
    public static Vector3 Set(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v.X = x ?? v.X;
        v.Y = y ?? v.Y;
        v.Z = z ?? v.Z;
        return v;
    }

    public static Vector2 Set(this Vector2 v, float? x = null, float? y = null)
    {
        v.X = x ?? v.X;
        v.Y = y ?? v.Y;
        return v;
    }
}
