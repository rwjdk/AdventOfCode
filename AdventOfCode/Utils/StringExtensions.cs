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

    public static int[] SplitToIntegers(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt32(x.Trim())).ToArray();
    }
    
    public static long[] SplitToLongs(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x.Trim())).ToArray();
    }
    
    public static string[] SplitToStrings(this string inputString, char separator = ' ')
    {
        return inputString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
    }
    
    public static List<TOutputType[]> SplitTwice<TOutputType>(this string inputString, char split1Separator, char split2Separator)
    {
        var firstSplitValues = inputString.SplitToStrings(split1Separator);
        if (typeof(TOutputType) == typeof(string))
        {
            return firstSplitValues.Select(x => x.SplitToStrings(split2Separator).Cast<TOutputType>().ToArray()).ToList();
        }
        if (typeof(TOutputType) == typeof(int))
        {
            return firstSplitValues.Select(x => x.SplitToIntegers(split2Separator).Cast<TOutputType>().ToArray()).ToList();
        }
        if (typeof(TOutputType) == typeof(long))
        {
            return firstSplitValues.Select(x => x.SplitToLongs(split2Separator).Cast<TOutputType>().ToArray()).ToList();
        }

        throw new NotSupportedException("Your T type is not supported");
    }

    public static string[] RemoveTheseChars(this string[] input, params char[] chars)
    {
        var result = new List<string>();
        foreach (var inputLine in input)
        {
            result.Add(inputLine.RemoveTheseChars(chars));
        }
        return result.ToArray();
    }

    public static string RemoveTheseChars(this string input, params char[] chars)
    {
        foreach (var c in chars)
        {
            input = input.Replace(c.ToString(), string.Empty);
        }

        return input;
    }
    
    public static PrefixWithInteger GetPrefixWithInteger(this string input, char separator = ':')
    {
        Prefix prefix = input.GetPrefix(separator);
        var lastIndexOfSpace = prefix.PrefixText.LastIndexOf(' ');
        var number = Convert.ToInt32(prefix.PrefixText.Substring(lastIndexOfSpace));
        return new PrefixWithInteger(prefix.PrefixText, number, prefix.RestOfString);
    }
    
    public static Prefix GetPrefix(this string input, char separator = ':')
    {
        var indexOfSeparator = input.IndexOf(separator);
        string prefix = input.Substring(0, indexOfSeparator).Trim();
        return new Prefix(prefix, input.Substring(indexOfSeparator + 1).Trim());
    }

    /// <summary>
    /// Remove Prefix from a string. Example: 'Game1: abc;def;123' >> 'abc;def;123'
    /// </summary>
    /// <param name="input">Input</param>
    /// <param name="separator">Separator of prefix and data</param>
    /// <returns>Rest of String</returns>
    public static string RemovePrefix(this string input, char separator = ':')
    {
        var indexOfSeparator = input.IndexOf(separator);
        return input.Substring(indexOfSeparator + 1).Trim();
    }

    public static KeyValuePair<string, int> GetIntegerAndIdentifier(this string input, char separator = ' ', ValueAndIdentiferOrder order = ValueAndIdentiferOrder.ValueIdentifer)
    {
        var parts = input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        switch (order)
        {
            case ValueAndIdentiferOrder.ValueIdentifer:
                return new KeyValuePair<string, int>(parts[1], parts[0].ToInteger());
            case ValueAndIdentiferOrder.IdentifierValue:
                return new KeyValuePair<string, int>(parts[0], parts[1].ToInteger());
            default:
                throw new ArgumentOutOfRangeException(nameof(order), order, null);
        }
    }
}