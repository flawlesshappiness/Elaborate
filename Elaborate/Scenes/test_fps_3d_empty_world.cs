using Godot;

public partial class test_fps_3d_empty_world : Scene
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
                /*
                TogglePause();
                Input.MouseMode = IsPaused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
                View.Get<PauseView>().Visible = IsPaused;
                */

                Tree.Quit();
            }
        }
    }
}
