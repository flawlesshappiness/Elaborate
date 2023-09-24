public partial class FirstPersonPlayer : Player
{
    public static FirstPersonPlayer Instance { get; private set; }

    public FirstPersonPlayerMovement FirstPersonMovement { get { return Movement as FirstPersonPlayerMovement; } }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}
