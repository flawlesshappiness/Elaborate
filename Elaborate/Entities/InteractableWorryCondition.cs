using Godot;

public partial class InteractableWorryCondition : InteractableCondition
{
    [Export]
    public float GreaterThan { get; set; } = 0f;

    [Export]
    public float LessThan { get; set; } = 1f;

    public override bool CanInteract => Validate();

    private bool Validate()
    {
        var v = WorryController.Instance.WorryValue;
        return v > GreaterThan && v < LessThan;
    }
}
