using Godot;

public partial class ProteanClouds : ColorRect
{
    [Export]
    public float Speed { get; set; }

    private ShaderMaterial _mat;
    private double _time_start;

    public override void _Ready()
    {
        base._Ready();
        _mat = Material as ShaderMaterial;
        _time_start = Time.GetUnixTimeFromSystem();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_mat == null) return;

        var time = (Time.GetUnixTimeFromSystem() - _time_start) * Speed;
        _mat.SetShaderParameter("iTime", time);
    }
}
