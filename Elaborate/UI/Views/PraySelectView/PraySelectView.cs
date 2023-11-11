using Godot;
using System.Collections;

public partial class PraySelectView : View
{
    private ColorRect _background;
    private Control _buttons;
    private Button _btn_prefab;

    public override void _Ready()
    {
        base._Ready();
        _background = GetNode<ColorRect>("Background");
        _buttons = GetNode("Buttons") as Control;
        _btn_prefab = GetNode("Buttons/ScrollContainer/MarginContainer/VBoxContainer/ButtonPrefab") as Button;

        Reset();
        CreateButtons();
    }

    private void Reset()
    {
        Visible = false;
        _background.Color = Color.Color8(0, 0, 0, 0);
        _buttons.Visible = false;
        _btn_prefab.Visible = false;

        Player.Input.MouseVisibleLock.RemoveLock(nameof(PraySelectView));
        Player.InteractLock.RemoveLock(nameof(PraySelectView));
    }

    public Coroutine AnimateShow()
    {
        Player.InteractLock.AddLock(nameof(PraySelectView));

        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            yield return AnimateShowBackground();
            _buttons.Visible = true;
            Player.Input.MouseVisibleLock.AddLock(nameof(PraySelectView));
        }
    }

    private Coroutine AnimateShowBackground() =>
        _background.TweenProperty(ColorRectProperty.Color, 0.5f, Color.Color8(0, 0, 0, 0), Colors.Black);

    private void CreateButtons()
    {
        CreateSceneButton("The State", "");
    }

    private void CreateSceneButton(string text, string scene)
    {
        var parent = _btn_prefab.GetParent();
        var btn = _btn_prefab.Duplicate() as Button;
        btn.Name = $"Button ({text})";
        btn.Visible = true;
        btn.Text = text;
        btn.Pressed += Pressed;
        parent.AddChild(btn);

        void Pressed()
        {
            // Reset UI
            Reset();

            // Save player data
            var player = Player.Instance;
            if (player != null)
            {
                player.SaveData();
            }

            // Load scene
            Scene.Goto<sign_paperwork_001>();
        }
    }
}
