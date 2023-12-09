namespace Utils;

public static class ExecutingDirectoryHelper
{
    public static string GetExecutingDirectory()
    {
        return Path.GetDirectoryName(typeof(ExecutingDirectoryHelper).Assembly.Location)!;
    }
}