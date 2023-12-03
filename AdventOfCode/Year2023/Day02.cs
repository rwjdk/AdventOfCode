using System.Diagnostics;
using Utils;

namespace Year2023;

//https://adventofcode.com/2023/day/2
public class Day02
{
    private const string Day = "Day02";

    //Determine which games would have been possible if the bag had been loaded with only 12 red cubes, 13 green cubes, and 14 blue cubes.
    //What is the sum of the IDs of those games?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 5, 3, 8)]
    [InlineData($"{Day}_Input.txt", 100, 37, 1853)]
    public void Part1(string inputFile, int expectedTotalNumberOfGames, int expectedValidGames, int expectedSumOfValidGameIds)
    {
        List<Day02Game> games = InputReader.ReadInputLines(inputFile).Select(x => x.ToGame()).ToList();
        Assert.Equal(expectedTotalNumberOfGames, games.Count);

        var validGames = games.Where(x => x.ObeyMaxColorRules(maxBlue: 14, maxRed: 12, maxGreen: 13)).ToList();
        Assert.Equal(expectedValidGames, validGames.Count);
        var calculatedAnswer = validGames.Sum(x => x.GameNumber);
        Assert.Equal(expectedSumOfValidGameIds, calculatedAnswer);
    }

    //The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together
    //For each game, find the minimum set of cubes that must have been present.
    //What is the sum of the power of these sets?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 2286)]
    [InlineData($"{Day}_Input.txt", 72706)]
    public void Part2(string inputFile, int expectedPowerOfTheSets)
    {
        int calculatedAnswer = 0;
        List<Day02Game> games = InputReader.ReadInputLines(inputFile).Select(x => x.ToGame()).ToList();
        foreach (Day02Game game in games)
        {
            var (neededGreen, neededBlue, neededRed) = game.GetMaxNeededCubes();
            int power = neededGreen * neededBlue * neededRed;
            calculatedAnswer += power;
        }
        Assert.Equal(expectedPowerOfTheSets, calculatedAnswer);
    }
}

[DebuggerDisplay("Game: {GameNumber} Sets; {Sets.Length}")]
public record Day02Game(int GameNumber, params Day02GameSet[] Sets)
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
            UpdateIfBigger(set.NumberOfBlue, ref neededBlue);
            UpdateIfBigger(set.NumberOfRed, ref neededRed);
            UpdateIfBigger(set.NumberOfGreen, ref neededGreen);
        }

        return (neededBlue, neededRed, neededGreen);

        void UpdateIfBigger(int current, ref int maxSoFar)
        {
            if (current > maxSoFar)
            {
                maxSoFar = current;
            }
        }
    }
}

[DebuggerDisplay("Blue: {NumberOfBlue}, Red: {NumberOfRed}, Green: {NumberOfGreen}")]
public record Day02GameSet(int NumberOfBlue, int NumberOfRed, int NumberOfGreen);

public static class Day02Extensions
{
    public static Day02Game ToGame(this string input)
    {
        var indexOfColon = input.IndexOf(':', StringComparison.Ordinal);
        int gameNumber = Convert.ToInt32(input.Substring(5, indexOfColon - 5));

        List<Day02GameSet> sets = new List<Day02GameSet>();
        string setPartOfLine = input.Substring(indexOfColon + 1).Trim();
        string[] setsRaw = setPartOfLine.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var setRaw in setsRaw)
        {
            int numberOfBlue = 0;
            int numberOfRed = 0;
            int numberOfGreen = 0;
            string[] setRawCubeDistributions = setRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var setRawCubeDistribution in setRawCubeDistributions)
            {
                if (TryGetNumberOfAColor(setRawCubeDistribution, "blue", out int valueBlue))
                {
                    numberOfBlue = valueBlue;
                }
                else if (TryGetNumberOfAColor(setRawCubeDistribution, "red", out int valueRed))
                {
                    numberOfRed = valueRed;
                }
                else if (TryGetNumberOfAColor(setRawCubeDistribution, "green", out int valueGreen))
                {
                    numberOfGreen = valueGreen;
                }
            }

            sets.Add(new Day02GameSet(numberOfBlue, numberOfRed, numberOfGreen));
        }

        return new Day02Game(gameNumber, sets.ToArray());

        bool TryGetNumberOfAColor(string setRawCubeDistribution, string colorName, out int valueOfColor)
        {
            if (setRawCubeDistribution.Contains(colorName))
            {
                setRawCubeDistribution = setRawCubeDistribution.Trim();
                var indexOfSpace = setRawCubeDistribution.IndexOf(' ');
                valueOfColor = Convert.ToInt32(setRawCubeDistribution.Substring(0, indexOfSpace));
                return true;
            }

            valueOfColor = 0;
            return false;
        }
    }
}