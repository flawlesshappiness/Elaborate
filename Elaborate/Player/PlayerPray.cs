using Godot;
using System.Collections;

public partial class PlayerPray : Node
{
    private PraySelectView _view;

    public override void _Ready()
    {
        base._Ready();
        _view = View.Get<PraySelectView>();
    }

    public void BeginPraying()
    {
        Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            Scene.PauseLock.AddLock(nameof(PlayerPray));
            _view.Show();
            yield return _view.AnimateShow();
            Scene.PauseLock.RemoveLock(nameof(PlayerPray));
        }
    }
}
