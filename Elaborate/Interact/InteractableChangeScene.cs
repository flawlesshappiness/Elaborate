using Godot;

public partial class InteractableChangeScene : Interactable
{
    [Export]
    public string SceneName { get; set; }

    [Export]
    public string StartNode { get; set; }

    protected override void Interact()
    {
        base.Interact();

        // Save
        Save.Game.Scene = SceneName;
        SaveDataController.Instance.Save<GameSaveData>();

        Scene.Goto(SceneName, StartNode);
        EndInteraction();
    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }
}
