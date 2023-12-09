using System.Diagnostics;
using Utils;

namespace Year2023.Day02;

//https://adventofcode.com/2023/day/2
public class Day02
{
    private const string Day = "Day02";

    //Determine which games would have been possible if the bag had been loaded with only 12 red cubes, 13 green cubes, and 14 blue cubes.
    //What is the sum of the IDs of those games?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 5, 3, 8)]
    [InlineData($"{Day}\\Input.txt", 100, 37, 1853)]
    public void Part1(string inputFile, int expectedTotalNumberOfGames, int expectedValidGames, int expectedSumOfValidGameIds)
    {
        Game[] games = inputFile.ToLines().ToGames();
        Assert.Equal(expectedTotalNumberOfGames, games.Length);

        var validGames = games.Where(x => x.ObeyMaxColorRules(maxBlue: 14, maxRed: 12, maxGreen: 13)).ToList();
        Assert.Equal(expectedValidGames, validGames.Count);
        var calculatedAnswer = validGames.Sum(x => x.GameNumber);
        Assert.Equal(expectedSumOfValidGameIds, calculatedAnswer);
    }

    //The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together
    //For each game, find the minimum set of cubes that must have been present.
    //What is the sum of the power of these sets?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 2286)]
    [InlineData($"{Day}\\Input.txt", 72706)]
    public void Part2(string inputFile, int expectedPowerOfTheSets)
    {
        int calculatedAnswer = 0;
        Game[] games = inputFile.ToLines().ToGames();
        foreach (Game game in games)
        {
            var (neededGreen, neededBlue, neededRed) = game.GetMaxNeededCubes();
            int power = neededGreen * neededBlue * neededRed;
            calculatedAnswer += power;
        }

        Assert.Equal(expectedPowerOfTheSets, calculatedAnswer);
    }
}

[DebuggerDisplay("Game: {GameNumber} Sets; {Sets.Length}")]
public record Game(int GameNumber, params GameSet[] Sets)
{
    public bool ObeyMaxColorRules(int maxBlue, int maxRed, int maxGreen)
    {
        return Sets.All(x => x.NumberOfBlue <= maxBlue && x.NumberOfRed <= maxRed && x.NumberOfGreen <= maxGreen);
    }

    public (int Blue, int Red, int Green) GetMaxNeededCubes()
    {
        int neededGreen = 0;
        int neededBlue = 0;
        int neededRed = 0;

        foreach (var set in Sets)
        {
            neededBlue = Math.Max(neededBlue, set.NumberOfBlue);
            neededRed = Math.Max(neededRed, set.NumberOfRed);
            neededGreen = Math.Max(neededGreen, set.NumberOfGreen);
        }

        return (neededBlue, neededRed, neededGreen);
    }
}

[DebuggerDisplay("Blue: {NumberOfBlue}, Red: {NumberOfRed}, Green: {NumberOfGreen}")]
public record GameSet(int NumberOfBlue, int NumberOfRed, int NumberOfGreen);

public static class Day02Extensions
{
    public static Game[] ToGames(this string[] inputLines)
    {
        List<Game> result = [];
        foreach (var input in inputLines)
        {
            var (_, number, restOfString) = input.GetPrefixWithInteger();
            List<GameSet> sets = [];

            var setsAndDistributions = restOfString.SplitTwice<string>(split1Separator: ';', split2Separator: ',');
            foreach (var set in setsAndDistributions)
            {
                int numberOfBlue = 0;
                int numberOfRed = 0;
                int numberOfGreen = 0;
                foreach (var distribution in set)
                {
                    var (color, value) = distribution.GetIntegerAndIdentifier();
                    switch (color)
                    {
                        case "blue":
                            numberOfBlue = value;
                            break;
                        case "red":
                            numberOfRed = value;
                            break;
                        case "green":
                            numberOfGreen = value;
                            break;
                    }
                }

                sets.Add(new GameSet(numberOfBlue, numberOfRed, numberOfGreen));
            }

            result.Add(new Game(number, sets.ToArray()));
        }

        return result.ToArray();
    }
}