using Godot;

public partial class WorryController : Node
{
    public static WorryController Instance => Singleton.TryGet<WorryController>(out var instance) ? instance : Create();

    public static WorryController Create() =>
        Singleton.CreateSingleton<WorryController>($"Worry/{nameof(WorryController)}");

    public event System.Action<float> OnWorryChanged;

    public float WorryValue => Save.Game.Worry;

    public void SetWorry(float value)
    {
        // Set value
        Save.Game.Worry = Mathf.Clamp(value, 0f, 1f);

        // Save
        SaveDataController.Instance.Save<GameSaveData>();

        // Event
        OnWorryChanged?.Invoke(value);
    }

    public void AddWorry(float value) =>
        SetWorry(WorryValue + value);
}
