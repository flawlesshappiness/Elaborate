using Godot;

public partial class InteractableDialogueCharacter : Interactable
{
    [Export]
    public string Id { get; set; }

    [Export]
    public string DefaultNode { get; set; }

    private DialogueView DialogueView { get; set; }

    public override void _Ready()
    {
        base._Ready();
        DialogueView = View.Get<DialogueView>();
    }

    protected override void Interact()
    {
        base.Interact();

        Debug.Log($"InteractableDialogueCharacter.Interact: {Name}");
        Debug.Indent++;

        var character = DialogueController.Instance.GetOrCreateDialogueCharacterData(Id);
        if (character == null)
        {
            Debug.LogError($"Character was null");
            Debug.Indent--;
            return;
        }

        var node = string.IsNullOrEmpty(character.StartNode) ? DefaultNode : character.StartNode;
        DialogueView.SetDialogueNode(node);
        DialogueView.OnDialogueEnded += args =>
        {
            EndInteraction();
        };

        Debug.Indent--;
    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
        DialogueView.OnDialogueEnded = null;
    }
}
