public partial class FirstPersonPlayer : Player
{
    public static FirstPersonPlayer Instance { get; private set; }

    public FirstPersonPlayerMovement FirstPersonMovement { get { return Movement as FirstPersonPlayerMovement; } }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public override void SaveData()
    {
        base.SaveData();

        Debug.Log("FirstPersonPlayer.SaveData");

        var data = Save.Game.Player;
        data.Position = Position;
        data.CameraRotation = FirstPersonMovement.Camera.Rotation;
        data.NeckRotation = FirstPersonMovement.Neck.Rotation;

        Debug.Log($"  PlayerPosition: {data.Position}");
        Debug.Log($"  CameraRotation: {data.CameraRotation}");
        Debug.Log($"  NeckRotation: {data.NeckRotation}");
    }

    public override void LoadData()
    {
        base.LoadData();

        Debug.Log("FirstPersonPlayer.LoadData");

        var data = Save.Game.Player;

        if (data == null)
        {
            Debug.Log("  PlayerData is null");
            return;
        }

        Position = data.Position;
        FirstPersonMovement.Camera.Rotation = data.CameraRotation;
        FirstPersonMovement.Neck.Rotation = data.NeckRotation;

        Debug.Log($"  PlayerPosition: {data.Position}");
        Debug.Log($"  CameraRotation: {data.CameraRotation}");
        Debug.Log($"  NeckRotation: {data.NeckRotation}");
    }
}
