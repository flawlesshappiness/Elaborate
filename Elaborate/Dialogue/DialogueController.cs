using Godot;
using System.Collections.Generic;
using System.Text.Json;

public partial class DialogueController : Node
{
    public static DialogueController Instance => Singleton.TryGet<DialogueController>(out var instance) ? instance : Create();

    public static DialogueController Create() =>
        Singleton.CreateSingleton<DialogueController>($"Dialogue/{nameof(DialogueController)}");

    private DialogueNodeCollection _collection;

    public string SelectedUrlId { get; set; } = string.Empty;

    public override void _Ready()
    {
        base._Ready();
        DeserializeDialogue();
    }

    public DialogueNode GetNode(string id) => _collection.GetNode(id);

    private void DeserializeDialogue()
    {
        var path = "res://Resources/Dialogue/dialogue.json";
        var content = FileAccess.GetFileAsString(path);
        var nodes = JsonSerializer.Deserialize<IEnumerable<DialogueNode>>(content);
        UpdateNodes(nodes);
        _collection = new DialogueNodeCollection(nodes);
    }

    private void UpdateNodes(IEnumerable<DialogueNode> nodes)
    {
        foreach (var node in nodes)
        {
            node.Text = node.Text
                .Replace("[url", "[color=yellow][url")
                .Replace("[/url]", "[/url][/color]");
        }
    }

    public DialogueCharacterData GetOrCreateDialogueCharacterData(string id)
    {
        Debug.Log($"DialogueView.GetOrCreateDialogueCharacterdata: {id}");
        Debug.Indent++;

        if (string.IsNullOrEmpty(id))
        {
            Debug.Log("id was null or empty");
            Debug.Indent--;
            return null;
        }

        if (!Save.Game.DialogueCharacters.TryGetValue(id, out var data))
        {
            data = new DialogueCharacterData
            {
                Id = id
            };

            Save.Game.DialogueCharacters.Add(id, data);
        }

        Debug.Indent--;
        return data;
    }
}
