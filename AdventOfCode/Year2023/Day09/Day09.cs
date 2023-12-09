using Utils;

namespace Year2023.Day09;

//https://adventofcode.com/2023/day/9
public class Day09
{
    private const string Day = "Day09";

    //What is the sum of these extrapolated values (Next Number)?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 114)]
    [InlineData($"{Day}\\Input.txt", 1904165718)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var datasets = inputFile.ToLines().Select(x => x.SplitToIntegers());
        int calculatedAnswer = datasets.Sum(x => x.GetNextNumberInSequence());
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //What is the sum of these extrapolated values (Previous number)?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 2)]
    [InlineData($"{Day}\\Input.txt", 964)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var datasets = inputFile.ToLines().Select(x=> x.SplitToIntegers());
        int calculatedAnswer = datasets.Sum(x => x.GetPreviousNumberInSequence());
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public static class Day09Extensions
{
    public static int GetNextNumberInSequence(this int[] numberSequence)
    {
        var differenceSequence = GetDifferenceSequence(numberSequence);
        if (differenceSequence.Any(x => x != 0))
        {
            return numberSequence.Last() + differenceSequence.GetNextNumberInSequence();
        }

        return numberSequence.Last();
    }

    public static int GetPreviousNumberInSequence(this int[] numberSequence)
    {
        var differenceSequence = GetDifferenceSequence(numberSequence);
        if (differenceSequence.Any(x => x != 0))
        {
            return numberSequence.First() - differenceSequence.GetPreviousNumberInSequence();
        }

        return numberSequence.First();
    }

    private static int[] GetDifferenceSequence(int[] numberSequence)
    {
        List<int> differenceSequence = [];
        for (var i = 0; i < numberSequence.Length - 1; i++)
        {
            var number1 = numberSequence[i];
            var number2 = numberSequence[i + 1];
            differenceSequence.Add(number2 - number1);
        }

        return differenceSequence.ToArray();
    }
}