using Godot;

public interface IPlayer
{
    public void SaveData();

    public void LoadData();

    public void MoveToNode(Node node);

    public void EquipItem(string id, EquipmentSlot slot);

    public void UnequipItem(EquipmentSlot slot);
}
