using Godot;

public interface IPlayer
{
    public void SaveData();

    public void LoadData();

    public void MoveToNode(Node node);

    public void EquipItem(EquipItemArguments args);

    public void UnequipItem(UnequipItemArguments args);
}

public class EquipItemArguments
{
    public string ItemId { get; set; }

    public EquipmentSlot Slot { get; set; }

    public IItem WorldItem { get; set; }

    public bool Animate { get; set; }
}

public class UnequipItemArguments
{
    public EquipmentSlot Slot { get; set; }

    public bool Animate { get; set; }
}