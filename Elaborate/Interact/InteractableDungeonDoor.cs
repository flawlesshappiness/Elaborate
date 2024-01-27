using Godot;

public partial class InteractableDungeonDoor : Interactable
{
    [Export]
    public int FloorDelta { get; set; }

    [Export]
    public string DialogueNode { get; set; }

    private DialogueView DialogueView { get; set; }

    private DungeonScene DungeonScene { get; set; }

    public override void _Ready()
    {
        base._Ready();
        DialogueView = View.Get<DialogueView>();
        DungeonScene = this.GetNodeInParents<DungeonScene>();
    }

    protected override void Interact()
    {
        base.Interact();

        StartDialogue();
    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }

    private void StartDialogue()
    {
        Debug.Log("InteractableDungeonDoor.StartDialogue");
        Debug.Indent++;

        DialogueView.SetDialogueNode(DialogueNode);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_ENTER)
            {
                Debug.Log($"Clicked {Constants.DIALOGUE_URL_ENTER}");
                DungeonScene.SetFloor(DungeonScene.Floor + FloorDelta);
                EndInteraction();
            }
        };

        Debug.Indent--;
    }
}
