using System.Collections.Generic;
using System.Linq;

public class GameSaveData : SaveData
{
    public string Scene { get; set; } = nameof(home_001);

    public float Worry { get; set; } = 1;

    public PlayerData Player { get; set; } = new PlayerData();

    public List<SceneData> Scenes { get; set; } = new();

    public SceneData GetOrCreateSceneData(string scene_name)
    {
        var data = Scenes.FirstOrDefault(d => d.SceneName == scene_name);

        if (data == null)
        {
            Debug.Log("New scene data created!");
            data = new SceneData
            {
                SceneName = scene_name,
            };
            Scenes.Add(data);
        }

        return data;
    }
}
