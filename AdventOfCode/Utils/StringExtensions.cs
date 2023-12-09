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

    public static int[] SplitToNumbers(this string stringWithNumbers, char separator)
    {
        return stringWithNumbers.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToArray();
    }

    public static long[] SplitToLongNumbers(this string stringWithNumbers, char separator)
    {
        return stringWithNumbers.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x.Trim())).ToArray();
    }
}