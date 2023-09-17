using Godot;

public partial class View : Control
{
    public override void _Ready()
    {
        base._Ready();
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;
    }

    private static string GetPath<T>() where T : View
    {
        var type = typeof(T).Name;
        var path = $"UI/Views/{type}/{type}.tscn";
        return path;
    }

    public static T CreateInstance<T>() where T : View =>
        Singleton.CreateInstance<T>(GetPath<T>());

    public static T CreateSingleton<T>() where T : View =>
        Singleton.CreateSingleton<T>(GetPath<T>());

    public static T Get<T>() where T : View =>
        Singleton.Get<T>();
}
