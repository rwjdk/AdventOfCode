using Utils;

namespace Year2023.Day14;

//https://adventofcode.com/2023/day/14
public class Day14
{
    private const string Day = "Day14";

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

public static class Day14Extensions
{
}