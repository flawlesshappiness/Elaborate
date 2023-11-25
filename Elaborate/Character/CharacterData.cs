using Godot;
using System;

[GlobalClass]
public partial class CharacterData : Resource
{
    [Export]
    public AudioStream SfxDialogue { get; set; }

    [Export]
    public double SfxDialoguePlayDuration { get; set; }

    public static CharacterData Load(string name)
    {
        try
        {
            Debug.Log($"CharacterData.Load({name})");
            Debug.Indent++;
            var data = GD.Load<CharacterData>($"res://Resources/Characters/{name}.tres");
            Debug.Indent--;
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"CharacterData.Load: Failed to load CharacterData");
            Debug.LogError($"{e.Message}");
            Debug.Indent--;
            return null;
        }
    }
}
