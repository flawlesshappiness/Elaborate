using Godot;

public partial class DrawController : Line2D
{
    [Export]
    public bool ClearOnPointerUp { get; set; }

    private bool _is_pointer_down;
    private Vector2? _last_position;

    private event System.Action<InputEventMouseButton> OnPointerDown, OnPointerUp;

    private const float DIST_DRAG_POINT = 10;

    public override void _Ready()
    {
        base._Ready();
        ClearPoints();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

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
        if (_last_position == null) return;

        var p = e.Position;
        var dir = (p - _last_position).Value;

        SetPointPosition(GetPointCount() - 1, p);

        if (dir.Length() > DIST_DRAG_POINT)
        {
            AddPoint(p);
            _last_position = p;
        }
    }

    protected virtual void PointerDown(InputEventMouseButton e)
    {
        _is_pointer_down = true;
        _last_position = e.Position;
        AddPoint(e.Position);
        AddPoint(e.Position);

        OnPointerDown?.Invoke(e);
    }

    protected virtual void PointerUp(InputEventMouseButton e)
    {
        _is_pointer_down = false;
        _last_position = null;

        if (ClearOnPointerUp)
        {
            ClearPoints();
        }

        OnPointerUp?.Invoke(e);
    }
}
