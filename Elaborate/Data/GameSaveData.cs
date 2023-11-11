public class GameSaveData : SaveData
{
    public string Scene { get; set; } = nameof(home_001);

    public float Worry { get; set; } = 1;

    public PlayerData Player { get; set; }
}
