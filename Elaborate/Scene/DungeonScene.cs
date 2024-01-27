using Godot;

public partial class DungeonScene : Scene
{
    [Export]
    public string ExitSceneName { get; set; }

    [Export]
    public string ExitStartNode { get; set; }

    [Export]
    public DungeonResource.Type DungeonType { get; set; }

    public int Floor { get; set; }

    private DungeonBuildArgs CurrentBuild { get; set; }

    private int _previous_floor = 0;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        SetFloor(1);
    }

    public virtual void GenerateDungeon()
    {
        var dungeon = DungeonGenerator.GenerateDungeon(8, 8, 16);
        var resource = DungeonResource.Load(DungeonType);
        CurrentBuild = new DungeonBuildArgs(resource, dungeon);

        DungeonBuilder.BuildDungeon(CurrentBuild);

        SpawnPlayer();

        dungeon.Log();
    }

    public virtual void RegenerateDungeon()
    {
        ClearDungeon();
        GenerateDungeon();
    }

    public virtual void ClearDungeon()
    {
        if (CurrentBuild == null) return;

        CurrentBuild.Root.QueueFree();
    }

    public void SetFloor(int floor)
    {
        _previous_floor = Floor;
        Floor = floor;
        Debug.Log($"Current floor: {floor}");

        if (floor == 0)
        {
            ExitSaveData();
            Goto(ExitSceneName);
        }
        else
        {
            RegenerateDungeon();
        }
    }

    private void SpawnPlayer()
    {
        var player = Player.Instance as TopDownPlayer;
        var going_up = _previous_floor > Floor;
        var info = going_up ? CurrentBuild.Info.EndRoom : CurrentBuild.Info.StartRoom;
        player.GlobalPosition = info.Room.StartPosition.GlobalPosition;
    }

    private void ExitSaveData()
    {
        Save.Game.Scene = ExitSceneName;
        Save.Game.Player.Position = null;
        Save.Game.Player.CameraRotation = null;
        Save.Game.Player.NeckRotation = null;
        Save.Game.Player.StartNode = ExitStartNode;
        Save.Game.Serialize();
    }
}
