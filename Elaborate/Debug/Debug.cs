using Godot;

public static class Debug
{
    public const bool PRINT_ENABLED = true;

    public static void Log(string message)
    {
        if (PRINT_ENABLED)
        {
            GD.Print(message);
        }
    }

    public static void Log(bool debug, string message)
    {
        if (debug)
        {
            Log(message);
        }
    }

    public static void LogError(string message)
    {
        if (PRINT_ENABLED)
        {
            GD.PrintErr(message);
        }
    }
}
