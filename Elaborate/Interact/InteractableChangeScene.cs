using Godot;

public partial class InteractableChangeScene : Interactable
{
    [Export]
    public string SceneName { get; set; }

    protected override void Interact()
    {
        base.Interact();

        // Save
        Save.Game.Scene = SceneName;
        SaveDataController.Instance.Save<GameSaveData>();

        Scene.Goto(SceneName);
        EndInteraction();
    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }
}
