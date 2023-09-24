using Godot;

public partial class Interactable : Node
{
    public event System.Action OnInteract;

    public virtual void Interact()
    {
        Debug.Log($"Interacted with: {GetParent().Name}");
        OnInteract?.Invoke();
    }
}
