public partial class landlord_001 : Scene
{
    public override void SaveData()
    {
        base.SaveData();

        SaveNode("items/Key4", "visible");
        SaveNode("items/Key5", "visible");
    }
}
