using Godot;
using System.Collections;

public partial class DialogueView : View
{
    private RichTextLabel dialogue_label;
    private TextureButton dialogue_button;

    private Coroutine _cr_dialogue_text;

    private DialogueNode _current_node;

    private const ulong MSEC_PER_CHAR = 15;

    public bool IsAnimatingDialogue => !(_cr_dialogue_text?.HasEnded ?? false);

    public override void _Ready()
    {
        base._Ready();
        dialogue_label = FindChild("DialogueLabel") as RichTextLabel;
        dialogue_button = FindChild("DialogueButton") as TextureButton;

        dialogue_label.MetaClicked += MetaClicked;
        dialogue_button.ButtonUp += DialogueButtonUp;
    }

    private void DialogueButtonUp()
    {
        NextDialogueText();
    }

    private void MetaClicked(Variant meta)
    {
        var id = ((string)meta);
        SetDialogueNode(id);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        InputMouseButton(@event as InputEventMouseButton);
    }

    private void InputMouseButton(InputEventMouseButton e)
    {
        if (e == null) return;

        if (e.IsReleased() && e.ButtonIndex == MouseButton.Left)
        {
            if (Visible)
            {
                if (IsAnimatingDialogue)
                {
                    EndAnimateDialogueText();
                }
            }
            else
            {
                ShowDialogueBox();
                SetDialogueNode("DEBUG_001");
            }
        }
    }

    private void ShowDialogueBox()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Scene.PauseLock.AddLock(nameof(DialogueView));
        Visible = true;
    }

    private void HideDialogueBox()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Scene.PauseLock.RemoveLock(nameof(DialogueView));
        Visible = false;
    }

    private void ShowDialogueButton()
    {
        dialogue_button.Visible = true;
    }

    private void HideDialogueButton()
    {
        dialogue_button.Visible = false;
    }

    public void SetDialogueNode(string id) =>
        SetDialogueNode(DialogueController.Instance.GetNode(id));

    public void SetDialogueNode(DialogueNode node)
    {
        _current_node = node;

        if (node == null)
        {
            HideDialogueBox();
            return;
        }

        HideDialogueButton();
        SetDialogueText(node.Text);
        AnimateDialogueText(MSEC_PER_CHAR);
    }

    public void SetDialogueText(string text)
    {
        dialogue_label.Text = text;
        dialogue_label.VisibleCharacters = -1;
    }

    private void NextDialogueText() =>
        SetDialogueNode(_current_node.Next);

    private Coroutine AnimateDialogueText(ulong msec_per_char)
    {
        dialogue_label.VisibleCharacters = 0;

        Coroutine.Stop(_cr_dialogue_text);
        _cr_dialogue_text = Coroutine.Start(AnimateDialogueTextCr(msec_per_char));
        return _cr_dialogue_text;
    }

    private IEnumerator AnimateDialogueTextCr(ulong msec_per_char)
    {
        var i = 0;
        var max = dialogue_label.Text.GetLengthWithoutBBCode();
        var time_current = Time.GetTicksMsec();

        while (i < max)
        {
            yield return null;

            while (i < max && time_current < Time.GetTicksMsec())
            {
                i++;
                dialogue_label.VisibleCharacters = i;
                time_current += msec_per_char;
            }
        }

        OnAnimateDialogueTextEnd();
    }

    private void OnAnimateDialogueTextEnd()
    {
        dialogue_label.VisibleCharacters = -1;
        ShowDialogueButton();
    }

    private void EndAnimateDialogueText()
    {
        Coroutine.Stop(_cr_dialogue_text);
        OnAnimateDialogueTextEnd();
    }
}
