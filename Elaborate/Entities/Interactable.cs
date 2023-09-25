using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Interactable : Node
{
    private List<InteractableCondition> _conditions = new();

    public event System.Action OnInteract;

    public bool CanInteract => _CanInteract();

    public override void _Ready()
    {
        base._Ready();
        _conditions = this.GetNodesInParents<InteractableCondition>();
    }

    public bool TryInteract()
    {
        if (!CanInteract)
        {
            Debug.Log($"Failed to interact with: {GetParent().Name}, Condition not met");
            return false;
        }

        Interact();
        return true;
    }

    protected virtual void Interact()
    {
        Debug.Log($"Interacted with: {GetParent().Name}");
        OnInteract?.Invoke();
    }

    private bool _CanInteract() =>
        _conditions.All(condition => condition.CanInteract);
}
