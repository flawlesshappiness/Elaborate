using System.Linq;

public partial class basement_001 : Scene
{
    protected override void OnInitialize()
    {
        base.OnInitialize();

        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        var dungeon = DungeonGenerator.GenerateDungeon(5, 5, 16);
        var resource = DungeonResource.Load(DungeonResource.Type.Basement);
        var args = new DungeonBuildArgs
        {
            Resource = resource,
            Info = dungeon,
            RoomAreaSize = 10
        };

        DungeonBuilder.BuildDungeon(args);

        var player = Player.Instance as TopDownPlayer;
        player.GlobalPosition = dungeon.Rooms.First().Room.GlobalPosition;
    }
}
