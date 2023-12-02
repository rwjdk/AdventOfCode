namespace Utils;

public static class ExecutingDirectoryHelper
{
    public static string GetExecutingInputDataDirectory()
    {
        return Path.Combine(GetExecutingDirectory(), "InputData");
    }

    private static string GetExecutingDirectory()
    {
        return Path.GetDirectoryName(typeof(ExecutingDirectoryHelper).Assembly.Location)!;
    }
}