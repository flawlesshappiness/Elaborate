using Godot;
using System;

public partial class TopDownCameraFollow : Camera3D
{
    [Export]
    public float Speed;

    [Export]
    public Vector3 Offset;

    [Export]
    public Vector3 DebugOffset;

    private TopDownPlayer _player;

    private Vector3 _offset;
    private bool _zoomed_out;

    public override void _Ready()
    {
        base._Ready();
        _player = Player.Instance as TopDownPlayer;
        _offset = Offset;

        GlobalPosition = _player.GlobalPosition + _offset;

        Debug.RegisterAction(new DebugAction
        {
            Category = "Camera",
            Text = "Zoom out",
            Action = _ => ToggleZoomOut()
        });
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_player == null) return;

        var f = Convert.ToSingle(delta);

        var start = GlobalPosition;
        var end = _player.GlobalPosition + _offset;
        GlobalPosition = Lerp.Vector3(start, end, f * Speed);
    }

    private void ToggleZoomOut()
    {
        _zoomed_out = !_zoomed_out;
        _offset = _zoomed_out ? DebugOffset : Offset;
    }
}
