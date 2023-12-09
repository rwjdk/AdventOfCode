using Utils;

namespace Year2023.Day19;

//https://adventofcode.com/2023/day/19
public class Day19
{
    private const string Day = "Day19";

    //TODO: Add Part 1 Description
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 0)]
    [InlineData($"{Day}\\Input.txt", 0)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        var inputLines = inputFile.ToLines();
        throw new NotImplementedException();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //TODO: Add Part 2 Description
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 0)]
    [InlineData($"{Day}\\Input.txt", 0)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        var inputLines = inputFile.ToLines();
        throw new NotImplementedException();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public static class Day19Extensions
{
}