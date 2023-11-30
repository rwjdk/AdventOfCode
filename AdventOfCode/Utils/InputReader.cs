namespace Utils;

public static class InputReader
{
    public static string[] ReadInputLines(string filename)
    {
        var directory = ExecutingDirectoryHelper.GetExecutingInputDataDirectory();
        var path = Path.Combine(directory, filename);
        var lines = File.ReadAllLines(path);
        return lines;
    }
}