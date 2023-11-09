using Godot;

public class WaitForSeconds : CustomYieldInstruction
{
    private double _time_end;

    private double CurrentTime => Time.GetTicksMsec();
    public override bool KeepWaiting => CurrentTime < _time_end;

    public WaitForSeconds(double seconds)
    {
        _time_end = CurrentTime + seconds * 1000;
    }
}
