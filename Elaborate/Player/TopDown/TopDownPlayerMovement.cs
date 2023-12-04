using Godot;
using System;

public partial class TopDownPlayerMovement : PlayerMovement
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public bool IsOnFloor => Body.IsOnFloor();
    public Vector3 Velocity { get { return Body.Velocity; } set { Body.Velocity = value; } }

    [NodePath("../")]
    public CharacterBody3D Body;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Body.Velocity;

        /*
        // Add the gravity.
        if (!IsOnFloor)
            velocity.Y -= gravity * (float)delta;
        */

        /*
        // Handle Jump.
        if (Input.IsActionJustPressed(PlayerControls.Jump) && IsOnFloor)
            velocity.Y = JumpVelocity;
        */

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector(PlayerControls.Left, PlayerControls.Right, PlayerControls.Forward, PlayerControls.Back);
        Vector3 direction = (new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;

            var ry = Convert.ToSingle(Mathf.LerpAngle(Body.Rotation.Y, Mathf.Atan2(velocity.X, velocity.Z), delta * 20));
            Body.Rotation = new Vector3(0, ry, 0);
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
