using Utils;

namespace Year2023.Day06;

//https://adventofcode.com/2023/day/6
public class Day06
{
    private const string Day = "Day06";

    //Determine the number of ways you could beat the record in each race.What do you get if you multiply these numbers together?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 288)]
    [InlineData($"{Day}\\Input.txt", 293046)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        var races = inputFile.ToLines().ToDay06RacesPart1();
        foreach (var race in races)
        {
            int numberOfWays = race.GetNumberOfWaysToBeatCurrentRecord();
            if (calculatedAnswer == 0)
            {
                calculatedAnswer = numberOfWays;
            }
            else
            {
                calculatedAnswer *= numberOfWays;
            }
        }

        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //How many ways can you beat the record in this one much longer race?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 71503)]
    [InlineData($"{Day}\\Input.txt", 35150181)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var race = inputFile.ToLines().ToDay06RacePart2();
        int numberOfWays = race.GetNumberOfWaysToBeatCurrentRecord();
        Assert.Equal(expectedAnswer, numberOfWays);
    }
}

public static class Day06Extensions
{
    public static List<Race> ToDay06RacesPart1(this string[] lines)
    {
        var result = new List<Race>();
        var durations = lines[0].RemovePrefix().SplitToLongs();
        var recordTimes = lines[1].RemovePrefix().SplitToLongs();
        for (int i = 0; i < durations.Length; i++)
        {
            result.Add(new Race(durations[i], recordTimes[i]));
        }

        return result;
    }

    public static Race ToDay06RacePart2(this string[] lines)
    {
        var duration = Convert.ToInt64(lines[0].RemovePrefix().Replace(" ", string.Empty));
        var recordTime = Convert.ToInt64(lines[1].RemovePrefix().Replace(" ", string.Empty));

        return new Race(duration, recordTime);
    }
}

public record Race(long TotalDuration, long RecordDistance)
{
    public int GetNumberOfWaysToBeatCurrentRecord()
    {
        int numberOfWaysToBeatCurrentRecord = 0;
        for (int durationToHoldButton = 1; durationToHoldButton < TotalDuration; durationToHoldButton++)
        {
            long remainingTimeForRace = TotalDuration - durationToHoldButton;
            if (durationToHoldButton * remainingTimeForRace > RecordDistance)
            {
                numberOfWaysToBeatCurrentRecord++;
            }
        }

        return numberOfWaysToBeatCurrentRecord;
    }
}