using Godot;

public partial class InteractableDialogue : Interactable
{
    [Export]
    private string dialogue_id;

    private bool _active;
    private DialogueView _view;

    public override void _Ready()
    {
        base._Ready();

        _view = View.Get<DialogueView>();
        _view.OnDialogueEnded += OnDialogueEnded;

        if (string.IsNullOrEmpty(dialogue_id))
        {
            GD.PrintErr($"InteractableDialogue ({GetParent().Name}): id is empty");
        }
    }

    protected override void Interact()
    {
        base.Interact();
        _active = true;
        var view = View.Get<DialogueView>();
        view.SetDialogueNode(dialogue_id);
    }

    private void OnDialogueEnded(DialogueEndedArguments args)
    {
        if (!_active) return;
        _active = false;

        EndInteraction();
    }
}
