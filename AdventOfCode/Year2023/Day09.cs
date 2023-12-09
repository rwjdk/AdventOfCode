using Utils;

namespace Year2023;

//https://adventofcode.com/2023/day/9
public class Day09
{
    private const string Day = "Day09";

    //What is the sum of these extrapolated values (Next Number)?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 114)]
    [InlineData($"{Day}_Input.txt", 1904165718)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var datasets = InputReader.ReadInputLines(inputFile).ToDay09Datasets();
        int calculatedAnswer = datasets.Sum(x =>x.GetNextNumberInSequence(x.Numbers));
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //What is the sum of these extrapolated values (Previous number)?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 2)]
    [InlineData($"{Day}_Input.txt", 964)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var datasets = InputReader.ReadInputLines(inputFile).ToDay09Datasets();
        int calculatedAnswer = datasets.Sum(x => x.GetPreviousNumberInSequence(x.Numbers));
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public record Day09Dataset(int[] Numbers)
{
    public int GetNextNumberInSequence(int[] numberSequence)
    {
        var differenceSequence = GetDifferenceSequence(numberSequence);
        if (differenceSequence.Any(x => x != 0))
        {
            return numberSequence.Last() + GetNextNumberInSequence(differenceSequence);
        }
        return numberSequence.Last();
    }

    public int GetPreviousNumberInSequence(int[] numberSequence)
    {
        var differenceSequence = GetDifferenceSequence(numberSequence);
        if (differenceSequence.Any(x => x != 0))
        {
            return numberSequence.First() - GetPreviousNumberInSequence(differenceSequence);
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

public static class Day09Extensions
{
    public static Day09Dataset[] ToDay09Datasets(this string[] inputLines)
    {
        return inputLines.Select(x => new Day09Dataset(x.SplitToNumbers(' '))).ToArray();
    }
}