public static class Player
{
    public static readonly MultiLock InteractLock = new MultiLock();

    public static IPlayer Instance { get; set; }

    public static bool Is3D => Instance as Player3D != null;

    public static bool Is2D => Instance as Player2D != null;

    public static void SaveData()
    {
        if (Instance != null)
        {
            Instance.SaveData();
        }
    }

    public static void LoadData()
    {
        if (Instance != null)
        {
            Instance.LoadData();
        }

        PlayerEquipment.Instance.LoadData();
    }
}
