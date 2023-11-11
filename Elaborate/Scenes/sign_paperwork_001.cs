using Godot;
using System;
using System.Collections;

public partial class sign_paperwork_001 : MinigameScene
{
    private int _papers_to_sign;
    private TextureRect _current_paper;

    [NodeName("PivotPaperIn")]
    public Control PivotPaperIn;

    [NodeName("PivotPaperOut")]
    public Control PivotPaperOut;

    [NodeName("PaperTemplate")]
    public TextureRect PaperTemplate;

    public override void _Ready()
    {
        base._Ready();

        Player.Input.MouseVisibleLock.AddLock(nameof(sign_paperwork_001));

        _papers_to_sign = Random.Shared.Next(2, 3);

        PaperTemplate.Visible = false;

        AnimateNextPaper();
    }

    private void OnSign()
    {
        _papers_to_sign--;

        if (_papers_to_sign > 0)
        {
            AnimateNextPaper();
        }
        else
        {
            AnimateEnd();
        }
    }

    private void NextPaper()
    {
        if (_current_paper != null)
        {
            _current_paper.Visible = false;
            _current_paper.QueueFree();
        }

        _current_paper = PaperTemplate.Duplicate() as TextureRect;
        PaperTemplate.GetParent().AddChild(_current_paper);
        _current_paper.Visible = false;

        var draw = _current_paper.GetNodeInChildren<DrawController>();
        draw.DrawingEnabled = true;
        draw.OnDrawLine += _ => OnSign();
    }

    private Coroutine AnimateNextPaper()
    {
        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            if (_current_paper != null)
            {
                yield return AnimatePaperOut(_current_paper);
            }

            NextPaper();

            yield return AnimatePaperIn(_current_paper);
        }
    }

    private Coroutine AnimatePaperIn(TextureRect paper)
    {
        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            paper.Visible = true;
            paper.GlobalPosition = PivotPaperIn.GlobalPosition;
            var duration = 1.0f;
            yield return LerpEnumerator.Position(paper, duration, Vector2.Zero)
                .SetCurve(EasingFunctions.Ease.EaseOutQuad);
        }
    }

    private Coroutine AnimatePaperOut(TextureRect paper)
    {
        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            var end = PivotPaperOut.GlobalPosition;
            var duration = 1.0f;
            yield return LerpEnumerator.GlobalPosition(paper, duration, end)
                .SetCurve(EasingFunctions.Ease.EaseInQuad);
            paper.Visible = false;
        }
    }

    private Coroutine AnimateEnd()
    {
        return Coroutine.Start(Cr);
        IEnumerator Cr()
        {
            yield return AnimatePaperOut(_current_paper);

            Player.Input.MouseVisibleLock.RemoveLock(nameof(sign_paperwork_001));
            CompleteMinigame();
        }
    }
}
