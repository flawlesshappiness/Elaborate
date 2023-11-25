using Godot;
using System.Collections;
using System.Linq;

public partial class DialogueView : View
{
    private const bool DEBUG = true;

    private bool _first_frame;

    private RichTextLabel dialogue_label;
    private TextureButton dialogue_button;

    private Coroutine _cr_dialogue_text;

    private DialogueNode _current_node;

    private const ulong MSEC_PER_CHAR = 15;

    public bool IsAnimatingDialogue => !(_cr_dialogue_text?.HasEnded ?? false);

    public System.Action<string> OnDialogueStarted;
    public System.Action<DialogueEndedArguments> OnDialogueEnded;

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
        var text = (string)meta;
        var parts = text.Split('-');

        if (parts.Length == 2)
        {
            switch (parts[0])
            {
                case Constants.DIALOGUE_URL_ACTION:
                    ParseAction(parts[1]);
                    break;

                case Constants.DIALOGUE_URL_ID:
                    ClickUrlId(parts[1]);
                    break;

                default:
                    EndDialogue(new DialogueEndedArguments
                    {
                        Node = _current_node,
                        UrlClicked = text,
                    });
                    break;
            }
        }
        else
        {
            var node = DialogueController.Instance.GetNode(text);
            if (node != null)
            {
                SetDialogueNode(node);
            }
            else
            {
                EndDialogue(new DialogueEndedArguments
                {
                    Node = _current_node,
                    UrlClicked = text
                });
            }
        }
    }

    private void ParseAction(string action)
    {
        var args = new DialogueEndedArguments
        {
            Node = _current_node,
            UrlClicked = $"{Constants.DIALOGUE_URL_ACTION}-{action}"
        };

        switch (action)
        {
            case Constants.DIALOGUE_URL_ACTION_PRAY:
                PlayerPray.Instance.BeginPraying();
                EndDialogue(args);
                break;

            default:
                EndDialogue(args);
                break;
        }
    }

    private void ClickUrlId(string text)
    {
        Debug.Log($"DialogueView.ClickUrlId: {text}");
        Debug.Indent++;

        DialogueController.Instance.SelectedUrlId = text;
        EndDialogue(new DialogueEndedArguments
        {
            Node = _current_node,
            UrlClicked = $"{Constants.DIALOGUE_URL_ID}-{text}"
        });

        Debug.Indent--;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        InputMouseButton(@event as InputEventMouseButton);
        InputAction();
    }

    private void InputMouseButton(InputEventMouseButton e)
    {
        if (e == null) return;
        if (!Visible) return;

        if (e.IsReleased() && e.ButtonIndex == MouseButton.Left)
        {
            if (IsAnimatingDialogue)
            {
                EndAnimateDialogueText();
            }
        }
    }

    private void InputAction()
    {
        if (!Visible) return;

        if (Input.IsActionJustPressed(PlayerControls.UI_Accept))
        {
            if (IsAnimatingDialogue)
            {
                EndAnimateDialogueText();
            }
            else
            {
                NextDialogueText();
            }
        }
    }

    private void ShowDialogueBox()
    {
        PlayerInput.Instance.MouseVisibleLock.AddLock(nameof(PraySelectView));
        Scene.PauseLock.AddLock(nameof(DialogueView));
        Player.InteractLock.AddLock(nameof(DialogueView));
        Visible = true;
    }

    private void HideDialogueBox()
    {
        PlayerInput.Instance.MouseVisibleLock.RemoveLock(nameof(PraySelectView));
        Scene.PauseLock.RemoveLock(nameof(DialogueView));
        Player.InteractLock.RemoveLock(nameof(DialogueView));
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
        Debug.Log(DEBUG, $"DialogueView.SetDialogueNode({node})");
        Debug.Indent++;

        if (_current_node == null && node != null)
        {
            StartDialogue(node);
        }

        var previous_node = _current_node;
        _current_node = node;

        if (_current_node == null)
        {
            EndDialogue(new DialogueEndedArguments
            {
                Node = previous_node
            });

            Debug.Indent--;
            return;
        }

        Debug.Log(DEBUG, $"Dialogue node: {_current_node.Id}");

        var text = new DialogueText(node);

        ParseDialogueNode(node);
        HideDialogueButton();
        SetDialogueText(text);
        AnimateDialogueText(text, MSEC_PER_CHAR);

        Debug.Indent--;
    }

    private void ParseDialogueNode(DialogueNode node)
    {
        Debug.Log("DialogueView.ParseDialogueNode");
        Debug.Indent++;

        if (node == null)
        {
            Debug.LogError("  Node was null");
            Debug.Indent--;
            return;
        }

        if (!string.IsNullOrEmpty(node.Start))
        {
            Debug.Log($"node.Start: {node.Start}");
            var character = DialogueController.Instance.GetOrCreateDialogueCharacterData(node.Character);
            if (character == null)
            {
                Debug.LogError("Character was not found");
            }
            else
            {
                character.StartNode = node.Start;
            }
        }
    }

    private void StartDialogue(DialogueNode node)
    {
        Debug.Log(DEBUG, $"Dialogue started: {node.Id}");
        Debug.Indent++;

        OnDialogueStarted?.Invoke(node.Id);
        ShowDialogueBox();

        Debug.Indent--;
    }

    private void EndDialogue(DialogueEndedArguments args)
    {
        _current_node = null;
        HideDialogueBox();

        if (args != null)
        {
            Debug.Log(DEBUG, $"Dialogue ended: {args.Node.Id}");
            Debug.Indent++;

            OnDialogueEnded?.Invoke(args);

            Debug.Indent--;
        }
    }

    public void SetDialogueText(DialogueText text)
    {
        dialogue_label.Text = text.Text;
        dialogue_label.VisibleCharacters = -1;
    }

    private void NextDialogueText() =>
        SetDialogueNode(_current_node.Next);

    private Coroutine AnimateDialogueText(DialogueText text, ulong msec_per_char)
    {
        dialogue_label.VisibleCharacters = 0;
        _first_frame = true;

        Coroutine.Stop(_cr_dialogue_text);
        _cr_dialogue_text = Coroutine.Start(AnimateDialogueTextCr(text, msec_per_char));
        return _cr_dialogue_text;
    }

    private IEnumerator AnimateDialogueTextCr(DialogueText text, ulong msec_per_char)
    {
        var i = 0;
        var max = text.TextLength;
        var time_current = Time.GetTicksMsec();

        while (i < max)
        {
            yield return null;

            while (i < max && time_current < Time.GetTicksMsec())
            {
                i++;
                dialogue_label.VisibleCharacters = i;
                time_current += msec_per_char;

                // Animation
                var animation = text.Animations.FirstOrDefault(a => a.Index == i);
                if (animation != null)
                {
                    var pause = animation as DialogueText.PauseIndexAnimation;
                    if (pause != null)
                    {
                        time_current += (ulong)pause.DurationInMs;
                        yield return new WaitForSeconds((float)pause.DurationInMs / 1000);
                    }
                }
            }

            _first_frame = false;
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
        if (_first_frame) return;
        Coroutine.Stop(_cr_dialogue_text);
        OnAnimateDialogueTextEnd();
    }
}

public class DialogueEndedArguments
{
    public DialogueNode Node { get; set; }

    public string UrlClicked { get; set; } = string.Empty;
}