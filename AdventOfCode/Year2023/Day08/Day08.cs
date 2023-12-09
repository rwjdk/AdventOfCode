using Utils;

namespace Year2023.Day08;

//https://adventofcode.com/2023/day/8
public class Day08
{
    private const string Day = "Day08";

    //How many steps are required to reach ZZZ?
    [Theory]
    [InlineData($"{Day}\\Sample1.txt", 2)]
    [InlineData($"{Day}\\Sample2.txt", 6)]
    [InlineData($"{Day}\\Input.txt", 15_517)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var map = inputFile.ToLines().Today08Map();
        int calculatedAnswer = map.GetNumberOfStepsToReachGoal("AAA", "Z");
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //How many steps does it take before you're only on nodes that end with Z?
    [Theory]
    [InlineData($"{Day}\\Sample3.txt", 6)]
    [InlineData($"{Day}\\Input.txt", 14_935_034_899_483)] //aka almost 15 trillion
    public void Part2(string inputFile, long expectedAnswer)
    {
        var map = inputFile.ToLines().Today08Map();
        //long calculatedAnswer = map.GetNumberOfGhostStepsToReachGoalBruteForce();
        long calculatedAnswer = map.GetNumberOfGhostStepsToReachGoalFancyMath();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public record Map(Direction[] Directions, Dictionary<string, Dictionary<Direction, string>> Elements)
{
    public int GetNumberOfStepsToReachGoal(string currentLocation, string goal)
    {
        int steps = 0;
        int instructionIndex = 0;
        string end = string.Empty;
        while (!end.EndsWith(goal))
        {
            if (instructionIndex == Directions.Length)
            {
                instructionIndex = 0;
            }
            Direction directionToGo = Directions[instructionIndex];
            end = Elements[currentLocation][directionToGo];
            currentLocation = end;
            instructionIndex++;
            steps++;
        }
        return steps;
    }

    public long GetNumberOfGhostStepsToReachGoalBruteForce() //Take days to run so not used
    {
        const string destination = "Z";
        long steps = 0;
        var currentLocations = Elements.Keys.Where(x => x.EndsWith("A")).ToArray();
        int instructionIndex = 0;
        while (true)
        {
            if (instructionIndex == Directions.Length)
            {
                instructionIndex = 0;
            }
            Direction directionToGo = Directions[instructionIndex];
            for (var i = 0; i < currentLocations.Length; i++)
            {
                currentLocations[i] = Elements[currentLocations[i]][directionToGo];
            }
            instructionIndex++;
            steps++;
            if (currentLocations.All(x => x.EndsWith(destination)))
            {
                break;
            }
        }
        return steps;
    }

    public long GetNumberOfGhostStepsToReachGoalFancyMath()
    {
        const string goal = "Z";
        var currentLocations = Elements.Keys.Where(x => x.EndsWith("A")).ToArray();
        List<long> stepToReachGoalForEachLocation = [];
        foreach (var currentLocation in currentLocations)
        {
            var numberOfStepsToReachGoal = GetNumberOfStepsToReachGoal(currentLocation, goal);
            stepToReachGoalForEachLocation.Add(numberOfStepsToReachGoal);
        }

        var result = LeastCommonMultipleForList(stepToReachGoalForEachLocation.ToArray());

        return result;

        long LeastCommonMultipleForList(long[] numbers)
        {
            return numbers.Aggregate(LeastCommonMultiple);
        }
        long LeastCommonMultiple(long a, long b)
        {
            return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
        }
        long GreatestCommonDivisor(long a, long b)
        {
            return b == 0 ? a : GreatestCommonDivisor(b, a % b);
        }
    }
}

public enum Direction
{
    Left,
    Right
}

public static class Day08Extensions
{
    public static Map Today08Map(this string[] inputLines)
    {
        Direction[] directions = ParseDirection();
        Dictionary<string, Dictionary<Direction, string>> mapElements = [];
        for (int i = 2; i < inputLines.Length; i++)
        {
            var element = inputLines[i];
            var elementParts = element.Split("=", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var source = elementParts[0];
            var destinations = elementParts[1].Replace("(", string.Empty).Replace(")", string.Empty);
            var destinationParts = destinations.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            mapElements.Add(source, new Dictionary<Direction, string>
            {
                { Direction.Left, destinationParts[0]},
                { Direction.Right, destinationParts[1]},
            });
        }
        return new Map(directions, mapElements);

        Direction[] ParseDirection()
        {
            List<Direction> result = [];
            foreach (var direction in inputLines[0])
            {
                switch (direction)
                {
                    case 'R':
                        result.Add(Direction.Right);
                        break;
                    case 'L':
                        result.Add(Direction.Left);
                        break;
                    default: throw new ArgumentException("Invalid direction input");
                }
            }

            return result.ToArray();
        }
    }
}