using Godot;

public partial class InteractableDialogue : Interactable
{
    [Export]
    private string dialogue_id;

    public override void _Ready()
    {
        base._Ready();

        if (string.IsNullOrEmpty(dialogue_id))
        {
            GD.PrintErr($"InteractableDialogue ({GetParent().Name}): id is empty");
        }
    }

    protected override void Interact()
    {
        base.Interact();
        var view = View.Get<DialogueView>();
        view.SetDialogueNode(dialogue_id);
    }
}
