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

    public static List<int> SplitToNumbers(this string stringWithNumbers, char separator)
    {
        return stringWithNumbers.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x.Trim())).ToList();
    }

    public static List<long> SplitToLongNumbers(this string stringWithNumbers, char separator)
    {
        return stringWithNumbers.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt64(x.Trim())).ToList();
    }
}