using Godot;

public partial class FirstPersonPlayer : Player3D
{
    public static FirstPersonPlayer Instance { get; private set; }

    public FirstPersonPlayerMovement FirstPersonMovement { get { return Movement as FirstPersonPlayerMovement; } }

    [NodeName("EquipLeft")]
    public Node3D EquipLeftParent;

    [NodeName("EquipRight")]
    public Node3D EquipRightParent;

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

    public override void MoveToNode(Node node)
    {
        base.MoveToNode(node);

        var n3 = node as Node3D;

        if (n3 == null)
        {
            return;
        }

        var r = n3.GlobalRotation;
        FirstPersonMovement.Camera.Rotation = new Vector3(r.X, 0, 0);
        FirstPersonMovement.Neck.Rotation = new Vector3(0, r.Y, 0);
    }

    public override void EquipItem(string id, EquipmentSlot slot)
    {
        base.EquipItem(id, slot);

        Debug.Log($"  FirstPersonPlayer.EquipItem({id}, {slot})");

        var item_data = ItemData.Load(id);
        var parent = slot == EquipmentSlot.LEFT ? EquipLeftParent : EquipRightParent;
        var instance = GD.Load<PackedScene>(item_data.PathItem3D).Instantiate();
        parent.AddChild(instance);

        var item = instance as Item;
        item.CollisionEnabled = false;
    }
}
