using Godot;

public partial class InteractableUrlCondition : InteractableCondition
{
    [Export]
    public string Id { get; set; }

    public override bool CanInteract => Validate();

    private bool Validate()
    {
        return DialogueController.Instance.SelectedUrlId == Id;
    }
}
