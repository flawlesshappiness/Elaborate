using Godot;
using System;

public partial class PlayerInteract : RayCast3D
{
    private const bool DEBUG = true;

    public Interactable CurrentInteractable { get; private set; }

    public event Action<Interactable> OnInteractableEnter, OnInteractableExit;

    public override void _Process(double delta)
    {
        base._Process(delta);
        ProcessRaycast();
        ProcessInteract();
    }

    private void ProcessInteract()
    {
        if (Player.InteractLock.IsLocked) return;
        if (CurrentInteractable == null) return;

        // Currently not using a key as interact input
        /*
        if (Input.IsActionJustPressed(PlayerControls.Interact))
        {
            CurrentInteractable.Interact();
        }
        */
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        InputMouse(@event as InputEventMouseButton);
    }

    private void InputMouse(InputEventMouseButton e)
    {
        if (Player.InteractLock.IsLocked) return;
        if (CurrentInteractable == null) return;
        if (e == null) return;
        if (e.ButtonIndex == MouseButton.Left && e.IsReleased())
        {
            CurrentInteractable.TryInteract(OnInteractEnd);
        }
    }

    private void ProcessRaycast()
    {
        if (IsColliding())
        {
            var collider = GetCollider();
            SetInteractable(collider);
        }
        else
        {
            SetInteractable(null);
        }
    }

    private void OnInteractEnd()
    {
        Debug.Log($"PlayerInteract.OnInteractEnd");
        Debug.Indent++;

        var interactable = GetValidInteractableFromNode(CurrentInteractable, true);

        Debug.Log($"Interactable: {interactable}");

        if (interactable == null)
        {
            Debug.Indent--;
            return;
        }

        if (interactable == CurrentInteractable)
        {
            Debug.Log($"Same as current interactable: {CurrentInteractable}");
            Debug.Indent--;
            return;
        }

        Debug.Indent--;

        CurrentInteractable = interactable;
        CurrentInteractable.TryInteract(OnInteractEnd);
    }

    private void SetInteractable(GodotObject go)
    {
        try
        {
            var node = go as Node3D;
            var owner = node.SceneFilePath == string.Empty ? node.Owner : node;
            var interactable = GetValidInteractableFromNode(owner);

            if (interactable == CurrentInteractable)
            {
                return;
            }

            if (CurrentInteractable != null)
            {
                Debug.Log(DEBUG, $"Interactable Exit: {CurrentInteractable.Name} ({CurrentInteractable.GetNodeInParents<MeshInstance3D>().Name})");
                OnInteractableExit?.Invoke(CurrentInteractable);
            }

            CurrentInteractable = interactable;

            if (CurrentInteractable != null)
            {
                Debug.Log(DEBUG, $"Interactable Enter: {CurrentInteractable.Name} ({owner.Name})");
                OnInteractableEnter?.Invoke(CurrentInteractable);
            }
        }
        catch
        {
            ClearInteractable();
        }
    }

    private Interactable GetValidInteractableFromNode(Node node, bool exclude_self = false)
    {
        if (node.TryGetNode<InteractableCondition>(out var condition) && !condition.CanInteract)
        {
            return null;
        }

        if (node.TryGetNode<Interactable>(out var interactable) && !exclude_self)
        {
            return interactable;
        }

        foreach (var child in node.GetChildren())
        {
            interactable = GetValidInteractableFromNode(child);
            if (interactable != null)
            {
                return interactable;
            }
        }

        return null;
    }

    private void ClearInteractable()
    {
        if (CurrentInteractable != null)
        {
            OnInteractableExit?.Invoke(CurrentInteractable);
        }

        CurrentInteractable = null;
    }
}
