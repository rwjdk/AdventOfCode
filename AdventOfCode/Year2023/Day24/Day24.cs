using Utils;

namespace Year2023.Day24;

//https://adventofcode.com/2023/day/24
public class Day24
{
    private const string Day = "Day24";

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

public static class Day24Extensions
{
}