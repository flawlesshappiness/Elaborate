using Godot;

public interface IItem
{
    public string ItemDataId { get; set; }

    public Node ItemOwner { get; }
}
