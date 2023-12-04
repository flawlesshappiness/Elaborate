using Godot;
using System;

public partial class TopDownCameraFollow : Camera3D
{
    [Export]
    public float Speed;

    [Export]
    public Vector3 Offset;

    private TopDownPlayer _player;

    public override void _Ready()
    {
        base._Ready();
        _player = Player.Instance as TopDownPlayer;

        GlobalPosition = _player.GlobalPosition + Offset;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_player == null) return;

        var f = Convert.ToSingle(delta);

        var start = GlobalPosition;
        var end = _player.GlobalPosition + Offset;
        GlobalPosition = Lerp.Vector3(start, end, f * Speed);
    }
}
