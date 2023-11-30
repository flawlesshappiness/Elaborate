using Godot;

public partial class InteractableLock : Interactable
{
    [Export]
    public string ItemId { get; set; }

    [Export]
    public string LockedDialogueNode { get; set; }

    [Export]
    public string UnlockDialogueNode { get; set; }

    [Export]
    public bool Locked { get; set; } = true;

    private DialogueView _dialogue_view;

    public override void _Ready()
    {
        base._Ready();
        _dialogue_view = View.Get<DialogueView>();
    }

    protected override void Interact()
    {
        base.Interact();

        if (Locked)
        {
            StartLockedDialogue();
        }
        else
        {
            EndInteraction();
        }

    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }

    private void StartLockedDialogue()
    {
        _dialogue_view.SetDialogueNode(LockedDialogueNode);
        _dialogue_view.OnDialogueEnded += _ =>
        {
            _dialogue_view.OnDialogueEnded = null;
            ValidateEquipment();
        };
    }

    private void ValidateEquipment()
    {
        var has_item = PlayerEquipment.Instance.HasItem(ItemId);

        if (has_item)
        {
            var item = ItemData.Load(ItemId);
            DialogueController.Instance.SetOverwrite(Constants.DIALOGUE_OVERWRITE_USE_ITEM, item.ItemName);

            _dialogue_view.SetDialogueNode(UnlockDialogueNode);
            _dialogue_view.OnDialogueEnded += args =>
            {
                _dialogue_view.OnDialogueEnded = null;

                if (args.UrlClicked == Constants.DIALOGUE_URL_USE)
                {
                    UseItem();
                    Unlock();
                }
            };
        }
    }

    private void UseItem()
    {
        Debug.Indent++;

        Debug.Log("InteractableLock.UseItem");

        var slot = PlayerEquipment.Instance.GetItemSlot(ItemId);
        if (slot == null)
        {
            Debug.LogError($"InteractableLock.UseItem: Failed to get slot of item with id: {ItemId}");
            return;
        }

        PlayerEquipment.Instance.RemoveItem(new RemoveItemArguments
        {
            Slot = slot.Value,
            Animate = false
        });

        Debug.Indent--;
    }

    public void Unlock()
    {
        Locked = false;
    }

    public void Lock()
    {
        Locked = true;
    }
}
