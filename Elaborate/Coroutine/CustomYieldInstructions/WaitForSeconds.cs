using Godot;

public class WaitForSeconds : CustomYieldInstruction
{
    private float _time_end;

    private float CurrentTime => Time.GetTicksMsec();
    public override bool KeepWaiting => CurrentTime < _time_end;

    public WaitForSeconds(float seconds)
    {
        _time_end = CurrentTime + seconds * 1000;
    }
}
