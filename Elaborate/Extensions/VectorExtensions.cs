using Godot;

public static class VectorExtensions
{
    #region VECTOR3
    public static Vector3 Set(this Vector3 v, float? x = null, float? y = null, float? z = null)
    {
        v.X = x ?? v.X;
        v.Y = y ?? v.Y;
        v.Z = z ?? v.Z;
        return v;
    }

    public static Vector2 ToVector2(this Vector3 v)
    {
        return new Vector2 { X = v.X, Y = v.Y };
    }
    #endregion

    #region VECTOR2
    public static Vector2 Set(this Vector2 v, float? x = null, float? y = null)
    {
        v.X = x ?? v.X;
        v.Y = y ?? v.Y;
        return v;
    }

    public static Vector3 ToVector3(this Vector2 v)
    {
        return new Vector3 { X = v.X, Y = v.Y };
    }
    #endregion
}
