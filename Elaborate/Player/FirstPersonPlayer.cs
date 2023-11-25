using Godot;
using System.Collections;

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
        Debug.Indent++;

        var data = Save.Game.Player;

        data.CameraRotation = FirstPersonMovement.Camera.Rotation;
        data.NeckRotation = FirstPersonMovement.Neck.Rotation;

        Debug.Log($"PlayerPosition: {data.Position}");
        Debug.Log($"CameraRotation: {data.CameraRotation}");
        Debug.Log($"NeckRotation: {data.NeckRotation}");
        Debug.Indent--;
    }

    public override void LoadData()
    {
        base.LoadData();

        Debug.Log("FirstPersonPlayer.LoadData");
        Debug.Indent++;

        var data = Save.Game.Player;

        FirstPersonMovement.Camera.Rotation = data.CameraRotation ?? FirstPersonMovement.Camera.Rotation;
        FirstPersonMovement.Neck.Rotation = data.NeckRotation ?? FirstPersonMovement.Neck.Rotation;

        Debug.Log($"CameraRotation: {data.CameraRotation}");
        Debug.Log($"NeckRotation: {data.NeckRotation}");
        Debug.Indent--;
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

    private Item3D CreateItem(string id)
    {
        var item_data = ItemData.Load(id);
        var item = GD.Load<PackedScene>(item_data.PathItem3D).Instantiate() as Item3D;
        return item;
    }

    public override void EquipItem(EquipItemArguments args)
    {
        base.EquipItem(args);

        Debug.Log($"FirstPersonPlayer.EquipItem({args.ItemId}, {args.Slot})");
        Debug.Indent++;

        var item = CreateItem(args.ItemId);
        var parent = args.Slot == EquipmentSlot.LEFT ? EquipLeftParent : EquipRightParent;

        item.SetParent(Scene.Current);
        item.CollisionEnabled = false;
        item.ShadowsEnabled = false;

        SetEquippedItem(item, args.Slot);

        if (args.Animate)
        {
            AnimateEquipItem(item, parent, args.WorldItem.ItemOwner as Node3D);
        }
        else
        {
            item.SetParent(parent);
            item.Position = item.GrabNode.Position;
            item.Rotation = item.GrabNode.Rotation;
        }

        Debug.Indent--;
    }

    public override void UnequipItem(UnequipItemArguments args)
    {
        base.UnequipItem(new UnequipItemArguments
        {
            Slot = args.Slot,
            Animate = true
        });

        Debug.Log($"FirstPersonPlayer.UnequipItem({args.Slot})");
        Debug.Indent++;

        var current_item = GetEquippedItem(args.Slot);
        if (current_item == null)
        {
            Debug.Log("Equipped item was null");
            Debug.Indent--;
            return;
        }

        var id = current_item.ItemDataId;
        var item = CreateItem(id);
        item.SetParent(Scene.Current);
        item.ShadowsEnabled = true;

        SetEquippedItem(null, args.Slot);

        if (args.Animate)
        {
            AnimateUnequipItem(item, current_item);
        }
        else
        {
            var position = item.GlobalPosition;
            item.GlobalPosition = GlobalPosition.Set(x: position.X, z: position.Z);
            item.GlobalRotation = FirstPersonMovement.Neck.Rotation;
        }

        Debug.Indent--;
    }

    private Coroutine AnimateEquipItem(Item3D item, Node3D parent, Node3D previous_item)
    {
        Debug.Log("FirstPersonPlayer.AnimateEquipItem");

        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            // Create temporary nodes
            var n3 = previous_item as Node3D;
            var start_node = new Node3D();
            var end_node = new Node3D();

            // Initialize temporary nodes
            start_node.SetParent(Scene.Current);
            end_node.SetParent(parent);

            start_node.GlobalPosition = n3.GlobalPosition;
            start_node.GlobalRotation = n3.GlobalRotation;

            end_node.Position = item.GrabNode.Position;
            end_node.Rotation = item.GrabNode.Rotation;

            n3.Visible = false;

            // Perform lerp
            item.Transform = start_node.Transform;
            yield return LerpEnumerator.Lerp01(0.25f, f =>
            {
                item.Transform = start_node.GlobalTransform.InterpolateWith(end_node.GlobalTransform, f);
            });

            // Finalize lerp
            item.SetParent(parent);
            item.Transform = end_node.Transform;

            // Remove temporary nodes
            start_node.QueueFree();
            end_node.QueueFree();
            previous_item.Visible = false;
        }
    }

    private Coroutine AnimateUnequipItem(Item3D item, Node3D previous_item)
    {
        Debug.Log("FirstPersonPlayer.AnimateEquipItem");

        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            // Create temporary nodes
            var n3 = previous_item as Node3D;
            var start_node = new Node3D();
            var end_node = new Node3D();
            var parent = previous_item.GetParent();

            // Initialize temporary nodes
            start_node.SetParent(parent);
            end_node.SetParent(Scene.Current);

            start_node.Position = item.GrabNode.Position;
            start_node.Rotation = item.GrabNode.Rotation;

            end_node.GlobalPosition = GlobalPosition.Set(x: start_node.GlobalPosition.X, z: start_node.GlobalPosition.Z);
            end_node.GlobalRotation = FirstPersonMovement.Neck.Rotation;

            n3.Visible = false;

            yield return LerpEnumerator.Lerp01(0.25f, f =>
            {
                item.Transform = start_node.GlobalTransform.InterpolateWith(end_node.GlobalTransform, f);
            });

            item.SetParent(Scene.Current);
            item.Position = end_node.Position;
            item.Rotation = end_node.Rotation;

            start_node.QueueFree();
            end_node.QueueFree();
            previous_item.QueueFree();
        }
    }
}