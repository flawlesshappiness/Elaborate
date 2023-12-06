using System.Linq;

public partial class basement_001 : Scene
{
    protected override void OnInitialize()
    {
        base.OnInitialize();

        var dungeon = DungeonGenerator.GenerateDungeon(5, 5, 8);
        var resource = DungeonResource.Load(DungeonResource.Type.Basement);
        var args = new DungeonBuildArgs
        {
            Resource = resource,
            Info = dungeon,
            RoomAreaSize = 20f
        };

        DungeonBuilder.BuildDungeon(args);

        Debug.Log("");
        dungeon.Log();
        Debug.Log("");

        var player = Player.Instance as TopDownPlayer;
        player.GlobalPosition = dungeon.Rooms.First().Room.GlobalPosition;
    }
}
