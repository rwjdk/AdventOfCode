namespace Utils;

public static class StringExtensions
{
    public static int ToInteger(this string input)
    {
        return Convert.ToInt32(input);
    }

    public static bool IsNullOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static SequenceOfIntegers[] ToSequenceOfIntegers(this string[] inputLines, char separator = ' ')
    {
        return inputLines.Select(x => new SequenceOfIntegers(x.SplitToIntegers(separator))).ToArray();
    }

    public static SequenceOfLongs[] ToSequenceOfLongs(this string[] inputLines, char separator = ' ')
    {
        return inputLines.Select(x => new SequenceOfLongs(x.SplitToLongs(separator))).ToArray();
    }

    public static int[] SplitToIntegers(this string inputString, char separator)
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToArray();
    }

    public static long[] SplitToLongs(this string inputString, char separator)
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x.Trim())).ToArray();
    }
}