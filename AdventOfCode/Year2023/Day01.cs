using Utils;

namespace Year2023;

public class Day01
{
    //On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.
    //In Sample values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.
    //What is the sum of all of the calibration values?
    [Theory]
    [InlineData("Day01_Sample1.txt", 142)]
    [InlineData("Day01_Input.txt", 55712)]
    public void Part1(string inputFile, int expectedTotal)
    {
        Assert.Equal(expectedTotal, GetSum(inputFile, false));
    }

    //It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".
    //What is the sum of all of the calibration values with the "extra" special digits
    [Theory]
    [InlineData("Day01_Sample2.txt", 281)]
    [InlineData("Day01_Input.txt", 55413)]
    public void Part2(string inputFile, int expectedTotal)
    {
        Assert.Equal(expectedTotal, GetSum(inputFile, true));
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