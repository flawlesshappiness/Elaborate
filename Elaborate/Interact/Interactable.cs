using System.Collections.Generic;
using System.Linq;

public partial class Interactable : NodeScript
{
    private List<InteractableCondition> _conditions = new();

    public event System.Action OnInteract;
    public event System.Action OnInteractEnd;

    public bool CanInteract => _CanInteract();

    public override void _Ready()
    {
        base._Ready();
        _conditions = this.GetNodesInParents<InteractableCondition>();
    }

    public bool TryInteract(System.Action onInteractEnd)
    {
        if (!CanInteract)
        {
            Debug.Log($"Failed to interact with: {GetParent().Name}, Condition not met");
            return false;
        }

        OnInteractEnd = onInteractEnd;

        Interact();
        return true;
    }

    protected virtual void Interact()
    {
        Debug.Log($"Interacted with: {GetParent().Name}");
        OnInteract?.Invoke();
    }

    protected virtual void EndInteraction()
    {
        Debug.Log($"Interaction ended");
        OnInteractEnd?.Invoke();
    }

    private bool _CanInteract() =>
        IsVisibleInTree() && _conditions.All(condition => condition.CanInteract);
}
