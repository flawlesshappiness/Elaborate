public static class Player
{
    public static readonly MultiLock InteractLock = new MultiLock();

    public static IPlayer Instance { get; set; }

    public static bool Is3D => Instance as Player3d != null;

    public static bool Is2D => Instance as Player2d != null;
}
