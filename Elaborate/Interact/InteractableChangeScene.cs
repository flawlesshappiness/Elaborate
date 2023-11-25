using Godot;

public partial class InteractableChangeScene : Interactable
{
    [Export]
    public string DialogueNode { get; set; }

    [Export]
    public string SceneName { get; set; }

    [Export]
    public string StartNode { get; set; }

    private DialogueView DialogueView { get; set; }

    public override void _Ready()
    {
        base._Ready();
        DialogueView = View.Get<DialogueView>();
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
        Debug.Log("InteractableChangeScene.StartDialogue");
        Debug.Indent++;

        DialogueView.SetDialogueNode(DialogueNode);
        DialogueView.OnDialogueEnded = args =>
        {
            if (args.UrlClicked == Constants.DIALOGUE_URL_ENTER)
            {
                Debug.Log($"Clicked {Constants.DIALOGUE_URL_ENTER}");
                SaveData();
                ChangeScene();
                EndInteraction();
            }
        };

        Debug.Indent--;
    }

    private void SaveData()
    {
        Save.Game.Scene = SceneName;
        Save.Game.Player.Position = null;
        Save.Game.Player.CameraRotation = null;
        Save.Game.Player.NeckRotation = null;
        Save.Game.Player.StartNode = StartNode;
        Save.Game.Serialize();
    }

    private void ChangeScene()
    {
        Scene.Goto(SceneName);
    }
}
