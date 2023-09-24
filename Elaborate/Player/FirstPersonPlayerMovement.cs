using Godot;

public partial class FirstPersonPlayerMovement : PlayerMovement
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public bool IsOnFloor => body.IsOnFloor();
    public Vector3 Velocity { get { return body.Velocity; } set { body.Velocity = value; } }

    private CharacterBody3D body;
    private Node3D neck;
    private Camera3D camera;

    public override void _Ready()
    {
        base._Ready();
        body = GetNode<CharacterBody3D>("../");
        neck = GetNode<Node3D>("Neck");
        camera = GetNode<Camera3D>("Neck/Camera3D");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        RotateView(@event);
    }

    private void RotateView(InputEvent e)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;
        if (e is not InputEventMouseMotion) return;

        var motion = e as InputEventMouseMotion;
        neck.RotateY(-motion.Relative.X * 0.001f);
        camera.RotateX(-motion.Relative.Y * 0.001f);

        var x = Mathf.Clamp(camera.Rotation.X, Mathf.DegToRad(-70), Mathf.DegToRad(70));
        camera.Rotation = Rotation with { X = x };
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = body.Velocity;

        // Add the gravity.
        if (!IsOnFloor)
            velocity.Y -= gravity * (float)delta;

        /*
        // Handle Jump.
        if (Input.IsActionJustPressed(PlayerControls.Jump) && IsOnFloor)
            velocity.Y = JumpVelocity;
        */

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector(PlayerControls.Left, PlayerControls.Right, PlayerControls.Forward, PlayerControls.Back);
        Vector3 direction = (neck.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        body.MoveAndSlide();
    }
}
