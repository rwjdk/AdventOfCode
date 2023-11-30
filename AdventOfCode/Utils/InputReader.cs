namespace Utils;

public static class InputReader
{
    public static string[] ReadInputLines(string filename)
    {
        var directory = ExecutingDirectoryHelper.GetExecutingDirectory();
        var path = Path.Combine(directory, "InputData", filename);
        var lines = File.ReadAllLines(path);
        return lines;
    }
}