using Godot;

public partial class DrawController : Control
{
    [Export]
    public bool NewOnPointerDown { get; set; } = true;

    [Export]
    public bool DrawingEnabled { get; set; } = true;

    private bool _is_pointer_down;
    private Vector2? _last_position;

    private Line2D _line_prefab;
    private Line2D _current_line;

    private const float DIST_DRAG_POINT = 10;

    public event System.Action<InputEventMouseButton> OnPointerDown, OnPointerUp;

    public override void _Ready()
    {
        base._Ready();
        _line_prefab = this.GetNodeInChildren<Line2D>();
        _line_prefab.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (!DrawingEnabled) return;

        var emb = @event as InputEventMouseButton;
        if (emb != null)
            InputMouseButton(emb);

        var emm = @event as InputEventMouseMotion;
        if (emm != null)
            InputMouseMotion(emm);
    }

    private void InputMouseButton(InputEventMouseButton e)
    {
        if (e.ButtonIndex == MouseButton.Left)
        {
            if (e.IsPressed())
            {
                PointerDown(e);
            }
            else if (e.IsReleased())
            {
                PointerUp(e);
            }
        }
    }

    private void InputMouseMotion(InputEventMouseMotion e)
    {
        if (_current_line == null) return;
        if (_last_position == null) return;

        var p = e.Position - GlobalPosition;
        var dir = (p - _last_position).Value;
        var count = _current_line.GetPointCount();
        var rect = GetRect();

        if (rect.HasPoint(p))
        {
            _current_line.SetPointPosition(count - 1, p);

            if (dir.Length() > DIST_DRAG_POINT)
            {
                _current_line.AddPoint(p);
                _last_position = p;
            }
        }
    }

    protected virtual void PointerDown(InputEventMouseButton e)
    {
        if (NewOnPointerDown || _current_line == null)
        {
            NextLine();
        }

        var p = e.Position - GlobalPosition;

        _is_pointer_down = true;
        _last_position = p;
        _current_line.AddPoint(p);
        _current_line.AddPoint(p);

        OnPointerDown?.Invoke(e);
    }

    protected virtual void PointerUp(InputEventMouseButton e)
    {
        _is_pointer_down = false;
        _last_position = null;
        OnPointerUp?.Invoke(e);
    }

    private Line2D NextLine()
    {
        _current_line = CreateLine();
        return _current_line;
    }

    private Line2D CreateLine()
    {
        var line = _line_prefab.Duplicate() as Line2D;
        line.ClearPoints();
        AddChild(line);
        line.Visible = true;
        return line;
    }
}
