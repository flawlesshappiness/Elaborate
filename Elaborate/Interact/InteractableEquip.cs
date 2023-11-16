public partial class InteractableEquip : Interactable
{
    private Item Item { get; set; }

    private DialogueView DialogueView { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Item = this.GetNodeInParents<Item>();
        DialogueView = View.Get<DialogueView>();
    }

    protected override void Interact()
    {
        base.Interact();

        DialogueEquipLeft();

    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }

    private void DialogueEquipLeft()
    {
        Debug.Log("InteractableEquip.DialogueEquipLeft");

        var id = PlayerEquipment.Instance.HasLeftItem ? "EQUIP_LEFT_OCCUPIED" : "EQUIP_LEFT_OPEN";
        DialogueView.SetDialogueNode(id);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_EQUIP_LEFT)
            {
                Debug.Log($"  Clicked {Constants.DIALOGUE_URL_EQUIP_LEFT}");
                PlayerEquipment.Instance.Equip(Item.ItemDataId, EquipmentSlot.LEFT);
                Item.QueueFree();
                EndInteraction();
            }
            else
            {
                DialogueEquipRight();
            }
        };
    }

    private void DialogueEquipRight()
    {
        Debug.Log("InteractableEquip.DialogueEquipRight");

        var id = PlayerEquipment.Instance.HasRightItem ? "EQUIP_RIGHT_OCCUPIED" : "EQUIP_RIGHT_OPEN";
        DialogueView.SetDialogueNode(id);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_EQUIP_RIGHT)
            {
                Debug.Log($"  Clicked {Constants.DIALOGUE_URL_EQUIP_RIGHT}");
                PlayerEquipment.Instance.Equip(Item.ItemDataId, EquipmentSlot.RIGHT);
                Item.QueueFree();
                EndInteraction();
            }
            else
            {
                EndInteraction();
            }
        };
    }
}
