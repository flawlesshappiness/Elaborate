using System;
using System.Collections;

public class Coroutine
{
    public Guid Id { get; set; }

    public IEnumerator Enumerator { get; set; }

    public bool HasCompleted { get; set; }
    public bool HasEnded { get; set; }

    public Coroutine(IEnumerator enumerator)
    {
        Enumerator = enumerator;
    }

    public static Coroutine Start(IEnumerator enumerator)
    {
        var coroutine = new Coroutine(enumerator);
        CoroutineHandler.Instance.AddCoroutine(coroutine);
        return coroutine;
    }

    public static Coroutine Start(Func<IEnumerator> enumerator) =>
        Start(enumerator());

    public static bool Stop(Coroutine coroutine)
    {
        if (coroutine == null) return true;

        coroutine.HasEnded = true;
        CoroutineHandler.Instance.RemoveCoroutine(coroutine);
        return true;
    }

    public void UpdateFrame()
    {
        while (true)
        {
            var yield_instruction = Enumerator.Current as CustomYieldInstruction;
            if (yield_instruction != null && yield_instruction.KeepWaiting)
            {
                break;
            }

            if (!Enumerator.MoveNext())
            {
                HasEnded = true;
                HasCompleted = true;
                break;
            }

            if (Enumerator.Current == null)
            {
                break;
            }
        }
    }
}
