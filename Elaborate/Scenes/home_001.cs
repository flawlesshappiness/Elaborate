using Godot;

public partial class home_001 : Scene
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                Tree.Quit();
            }
        }
    }
}
