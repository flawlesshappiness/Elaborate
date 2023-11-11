using Godot;

public partial class FirstPersonPlayerMovement : PlayerMovement
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public bool IsOnFloor => Body.IsOnFloor();
    public Vector3 Velocity { get { return Body.Velocity; } set { Body.Velocity = value; } }

    [NodePath("../")]
    public CharacterBody3D Body;

    [NodeName("Neck")]
    public Node3D Neck;

    [NodeName("Camera3D")]
    public Camera3D Camera;

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
        Neck.RotateY(-motion.Relative.X * 0.001f);
        Camera.RotateX(-motion.Relative.Y * 0.001f);

        var x = Mathf.Clamp(Camera.Rotation.X, Mathf.DegToRad(-70), Mathf.DegToRad(70));
        Camera.Rotation = Rotation with { X = x };
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Body.Velocity;

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
        Vector3 direction = (Neck.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
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
        Body.MoveAndSlide();
    }
}
