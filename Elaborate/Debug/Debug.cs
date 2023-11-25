using Godot;

public static class Debug
{
    public const bool PRINT_ENABLED = true;

    public static int Indent = 0;

    private static string IndentString => GetIndentString();

    public static void Log(object o)
    {
        var message = o == null ? "null" : o.ToString();
        Log(message);
    }

    public static void Log(bool debug, string message)
    {
        if (debug)
        {
            Log(message);
        }
    }

    public static void Log(string message)
    {
        if (PRINT_ENABLED)
        {
            string s = IndentString + message;
            GD.Print(s);
        }
    }

    public static void LogError(string message)
    {
        if (PRINT_ENABLED)
        {
            GD.PrintErr(message);
        }
    }

    public static void AddIndent() => Indent++;

    public static void RemoveIndent() => Indent--;

    private static string GetIndentString()
    {
        string s = "";

        for (int i = 0; i < Indent; i++)
        {
            s += "  ";
        }

        return s;
    }
}
