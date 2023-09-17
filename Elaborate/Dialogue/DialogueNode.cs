using System.Text.Json.Serialization;

public class DialogueNode
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("next")]
    public string Next { get; set; }
}
