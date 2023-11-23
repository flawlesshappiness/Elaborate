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
        Save.Game.Player.Position = null;
        Save.Game.Player.CameraRotation = null;
        Save.Game.Player.NeckRotation = null;
        Save.Game.Player.StartNode = StartNode;
        SaveDataController.Instance.Save<GameSaveData>();

        Scene.Goto(SceneName);
        EndInteraction();
    }

    protected override void EndInteraction()
    {
        base.EndInteraction();
    }
}
