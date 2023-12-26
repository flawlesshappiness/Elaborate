public partial class basement_001 : Scene
{
    protected override void OnInitialize()
    {
        base.OnInitialize();

        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        var dungeon = DungeonGenerator.GenerateDungeon(8, 8, 25);
        var resource = DungeonResource.Load(DungeonResource.Type.Basement);
        var args = new DungeonBuildArgs(resource, dungeon);

        DungeonBuilder.BuildDungeon(args);

        var player = Player.Instance as TopDownPlayer;
        player.GlobalPosition = dungeon.StartRoom.Room.GlobalPosition;

        dungeon.Log();
    }
}
