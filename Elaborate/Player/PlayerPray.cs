using Godot;
using System.Collections;

public partial class PlayerPray : Node
{
    public static PlayerPray Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public void BeginPraying()
    {
        var view = View.Get<PraySelectView>();
        Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            Scene.PauseLock.AddLock("pray");
            view.Show();
            yield return view.AnimateShow();
            Scene.PauseLock.RemoveLock("pray");
        }
    }
}
