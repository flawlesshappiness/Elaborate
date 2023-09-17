using System.Collections;

public class CustomYieldInstruction : IEnumerator
{
    public object Current => this;

    public bool MoveNext() => !KeepWaiting;

    public void Reset()
    {
    }

    public virtual bool KeepWaiting { get; }
}
