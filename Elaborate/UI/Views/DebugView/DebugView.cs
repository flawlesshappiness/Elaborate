using Godot;
using System;

public partial class DebugView : View
{
    [NodeName("Main")]
    public Control Main;

    [NodeName("Content")]
    public Control Content;

    [NodeName("ButtonPrefab")]
    public Button ButtonPrefab;

    public override void _Ready()
    {
        base._Ready();
        Visible = true;
        Content.Visible = false;
        ButtonPrefab.Visible = false;
        SetVisible(false);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("debug"))
            {
                ToggleVisible();
            }
        }
    }

    public void SetVisible(bool visible)
    {
        Main.Visible = visible;

        var parent = GetParent();
        var lock_name = nameof(DebugView);
        if (visible)
        {
            PlayerInput.Instance.MouseVisibleLock.AddLock(lock_name);
            Player.InteractLock.AddLock(lock_name);
            Scene.PauseLock.AddLock(lock_name);

            var idx_max = parent.GetChildCount() - 1;
            parent.MoveChild(this, idx_max);
        }
        else
        {
            PlayerInput.Instance.MouseVisibleLock.RemoveLock(lock_name);
            Player.InteractLock.RemoveLock(lock_name);
            Scene.PauseLock.RemoveLock(lock_name);

            parent.MoveChild(this, 0);
        }
    }

    private void ToggleVisible() =>
        SetVisible(!Main.Visible);

    private Button CreateActionButton()
    {
        var instance = ButtonPrefab.Duplicate() as Button;
        instance.SetParent(ButtonPrefab.GetParent());
        instance.Visible = true;
        return instance;
    }

    public void RegisterAction(string title, Action<DebugView> action)
    {
        var button = CreateActionButton();
        button.Text = title;
        button.Pressed += () => action(this);
    }
}
