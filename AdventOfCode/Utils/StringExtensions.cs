using System.Diagnostics;

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

    public static int GetFirstDigitInString(this string input, bool includeSpelledOutDigits)
    {
        var digits = includeSpelledOutDigits ? NormalAndSpelledOutDigits : NormalDigits;
        int lowestIndex = int.MaxValue;
        Digit? firstDigit = null;
        foreach (var digit in digits)
        {
            var index = input.IndexOf(digit.StringRepresentation, StringComparison.OrdinalIgnoreCase);
            if (index != -1 && index < lowestIndex)
            {
                lowestIndex = index;
                firstDigit = digit;
                
                if (lowestIndex == 0)
                {
                    break; //short-circuit if lowestIndex is 0 (can't get lower so no need to proceed further)
                }
            }
        }
        return firstDigit?.Value ?? throw new ArgumentException("No digits in string");
    }

    public static int GetLastDigitInString(this string input, bool includeSpelledOutDigits)
    {
        var inputLength = input.Length;
        var digits = includeSpelledOutDigits ? NormalAndSpelledOutDigits : NormalDigits;
        int highestIndex = int.MinValue;
        Digit? lastDigit = null;
        foreach (var digit in digits)
        {
            var index = input.LastIndexOf(digit.StringRepresentation, StringComparison.OrdinalIgnoreCase);
            if (index != -1 && index > highestIndex)
            {
                highestIndex = index;
                lastDigit = digit;
                
                if (highestIndex == inputLength-1)
                {
                    break; //short-circuit if second to last (can't get higher so no need to proceed further)
                }
            }
        }
        return lastDigit?.Value ?? throw new ArgumentException("No digits in string");
    }

    private static readonly Digit[] NormalDigits = {
        new(1),
        new(2),
        new(3),
        new(4),
        new(5),
        new(6),
        new(7),
        new(8),
        new(9),
    };

    private static readonly Digit[] NormalAndSpelledOutDigits = {
        new(1),
        new(2),
        new(3),
        new(4),
        new(5),
        new(6),
        new(7),
        new(8),
        new(9),
        new("one", 1),
        new("two", 2),
        new("three", 3),
        new("four", 4),
        new("five", 5),
        new("six", 6),
        new("seven", 7),
        new("eight", 8),
        new("nine", 9)
    };

    [DebuggerDisplay("{Value} '{StringRepresentation}'")]
    private class Digit
    {
        public string StringRepresentation { get; }
        public int Value { get; }

        public Digit(int value)
        {
            StringRepresentation = value.ToString();
            Value = value;
        }

        public Digit(string stringRepresentation, int value)
        {
            StringRepresentation = stringRepresentation;
            Value = value;
        }
    }
}