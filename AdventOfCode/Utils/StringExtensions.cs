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
        KeyValuePair<Digit, int> lowestIndexDigit = new(new Digit(0), int.MaxValue);
        foreach (var digit in digits)
        {
            var indexOf = input.IndexOf(digit.StringRepresentation, StringComparison.OrdinalIgnoreCase);
            if (indexOf != -1 && indexOf < lowestIndexDigit.Value)
            {
                lowestIndexDigit = new(digit, indexOf);
            }
        }
        return lowestIndexDigit.Key.Value != 0 ? lowestIndexDigit.Key.Value : throw new ArgumentException("No digits in string");
    }

    public static int GetLastDigitInString(this string input, bool includeSpelledOutDigits)
    {
        var digits = includeSpelledOutDigits ? NormalAndSpelledOutDigits : NormalDigits;
        KeyValuePair<Digit, int> highestIndexDigit = new(new Digit(0), int.MinValue);
        foreach (var digit in digits)
        {
            var indexOf = input.LastIndexOf(digit.StringRepresentation, StringComparison.OrdinalIgnoreCase);
            if (indexOf != -1 && indexOf > highestIndexDigit.Value)
            {
                highestIndexDigit = new(digit, indexOf);
            }
        }
        return highestIndexDigit.Key.Value != 0 ? highestIndexDigit.Key.Value : throw new ArgumentException("No digits in string");
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