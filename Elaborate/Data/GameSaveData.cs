using System.Collections.Generic;

public class GameSaveData : SaveData
{
    public string Scene { get; set; } = nameof(home_001);

    public float Worry { get; set; } = 1;

    public PlayerData Player { get; set; } = new PlayerData();

    public List<SceneData> Scenes { get; set; } = new();

    public Dictionary<string, bool> DialogueFlags { get; set; } = new();

    public Dictionary<string, DialogueCharacterData> DialogueCharacters { get; set; } = new();
}
