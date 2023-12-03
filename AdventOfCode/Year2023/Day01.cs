using System.Diagnostics;
using Utils;

namespace Year2023;

//https://adventofcode.com/2023/day/1
public class Day01
{
    private const string Day = "Day01"; //TODO add day

    //On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.
    //In Sample values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.
    //What is the sum of all of the calibration values?
    [Theory]
    [InlineData($"{Day}_Sample1.txt", 142)]
    [InlineData($"{Day}_Input.txt", 55712)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var calculatedAnswer = GetSum(inputFile, false);
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".
    //What is the sum of all of the calibration values with the "extra" special digits
    [Theory]
    [InlineData($"{Day}_Sample2.txt", 281)]
    [InlineData($"{Day}_Input.txt", 55413)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var calculatedAnswer = GetSum(inputFile, true);
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    private int GetSum(string inputFile, bool includeSpelledOutDigits)
    {
        var inputLines = InputReader.ReadInputLines(inputFile);
        int sum = 0;
        foreach (var inputLine in inputLines)
        {
            var first = inputLine.GetFirstDigitInString(includeSpelledOutDigits).ToString();
            var last = inputLine.GetLastDigitInString(includeSpelledOutDigits).ToString();
            var sumAsString = string.Concat(first, last);
            sum += sumAsString.ToInteger();
        }

        return sum;
    }
}

public static class Day01Extensions
{
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

                if (highestIndex == inputLength - 1)
                {
                    break; //short-circuit if second to last (can't get higher so no need to proceed further)
                }
            }
        }

        return lastDigit?.Value ?? throw new ArgumentException("No digits in string");
    }

    private static readonly Digit[] NormalDigits =
    {
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

    private static readonly Digit[] NormalAndSpelledOutDigits =
    {
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
    private class Digit(string stringRepresentation, int value)
    {
        public string StringRepresentation { get; } = stringRepresentation;
        public int Value { get; } = value;

        public Digit(int value) : this(value.ToString(), value)
        {
            //Empty
        }
    }
}