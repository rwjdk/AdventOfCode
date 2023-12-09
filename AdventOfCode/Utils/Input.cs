namespace Utils;

public static class Input
{
    public static string[] ToLines(this string filename)
    {
        var directory = ExecutingDirectoryHelper.GetExecutingDirectory();
        var path = Path.Combine(directory, filename);
        var lines = File.ReadAllLines(path);
        return lines;
    }
}