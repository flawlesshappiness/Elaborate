public partial class InteractableEquip : Interactable
{
    private IItem Item { get; set; }

    private DialogueView DialogueView { get; set; }

    public override void _Ready()
    {
        base._Ready();
        Item = Owner as IItem;
        DialogueView = View.Get<DialogueView>();
    }

    protected override void Interact()
    {
        base.Interact();

        DialogueEquipLeft();

    }

    protected override void EndInteraction()
    {
        DialogueView.OnDialogueEnded = null;
        base.EndInteraction();
    }

    private void DialogueEquipLeft()
    {
        Debug.Log("InteractableEquip.DialogueEquipLeft");

        var slot = EquipmentSlot.LEFT;
        var id = PlayerEquipment.Instance.HasItem(slot) ? "EQUIP_LEFT_OCCUPIED" : "EQUIP_LEFT_OPEN";
        DialogueView.SetDialogueNode(id);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_EQUIP_LEFT)
            {
                Debug.Log($"  Clicked {Constants.DIALOGUE_URL_EQUIP_LEFT}");
                PlayerEquipment.Instance.Equip(new EquipItemArguments
                {
                    ItemId = Item.ItemDataId,
                    Slot = slot,
                    WorldItem = Item,
                    Animate = true,
                });
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

        var slot = EquipmentSlot.RIGHT;
        var id = PlayerEquipment.Instance.HasItem(slot) ? "EQUIP_RIGHT_OCCUPIED" : "EQUIP_RIGHT_OPEN";
        DialogueView.SetDialogueNode(id);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_EQUIP_RIGHT)
            {
                Debug.Log($"  Clicked {Constants.DIALOGUE_URL_EQUIP_RIGHT}");
                PlayerEquipment.Instance.Equip(new EquipItemArguments
                {
                    ItemId = Item.ItemDataId,
                    Slot = slot,
                    WorldItem = Item,
                    Animate = true,
                });
                EndInteraction();
            }
            else
            {
                EndInteraction();
            }
        };
    }
}
