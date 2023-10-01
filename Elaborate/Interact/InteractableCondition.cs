using Godot;

public abstract partial class InteractableCondition : Node
{
    public abstract bool CanInteract { get; }
}
