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
        if (CurrentInteractable == null) return;

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
        if (CurrentInteractable == null) return;
        if (e == null) return;
        if (e.ButtonIndex == MouseButton.Left && e.IsReleased())
        {
            CurrentInteractable.Interact();
        }
    }

    private void ProcessRaycast()
    {
        if (IsColliding())
        {
            var collider = GetCollider();
            SetCollider(collider);
        }
        else
        {
            SetCollider(null);
        }
    }

    private void SetCollider(GodotObject go)
    {
        try
        {
            var node = go as Node3D;
            var parent = node.GetParent();
            var interactable = parent.GetNodeInChildren<Interactable>();

            if (interactable == CurrentInteractable) return;

            if (CurrentInteractable != null)
            {
                Debug.Log(DEBUG, $"Interactable Exit: {CurrentInteractable.Name} ({CurrentInteractable.GetParent().Name})");
                OnInteractableExit?.Invoke(CurrentInteractable);
            }

            CurrentInteractable = interactable;

            if (CurrentInteractable != null)
            {
                Debug.Log(DEBUG, $"Interactable Enter: {CurrentInteractable.Name} ({parent.Name})");
                OnInteractableEnter?.Invoke(CurrentInteractable);
            }
        }
        catch (Exception e)
        {
            ClearInteractable();
        }
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
