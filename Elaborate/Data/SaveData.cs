public abstract class SaveData
{
    public void Serialize()
    {
        var type = GetType();
        Debug.Log("## TYPE: " + type);
        SaveDataController.Instance.Save(type);
    }
}
