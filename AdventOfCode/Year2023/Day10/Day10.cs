using System.Diagnostics;
using Utils;

namespace Year2023.Day10;

//https://adventofcode.com/2023/day/10
public class Day10
{
    private const string Day = "Day10";

    //How many steps along the loop does it take to get from the starting position to the point farthest from the starting position?
    [Theory]
    //[InlineData($"{Day}\\Sample1.txt", 4)]
    //[InlineData($"{Day}\\Sample2.txt", 4)]
    [InlineData($"{Day}\\Sample3.txt", 8)]
    //[InlineData($"{Day}\\Sample4.txt", 8)]
    //[InlineData($"{Day}\\Input.txt", 0)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var maze = inputFile.ToLines().ToDay10Maze();
        var calculatedAnswer = maze.Start.GetStepsToFarthestPoint();
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

public static class Day10Extensions
{
    public static Maze ToDay10Maze(this string[] lines)
    {
        MazePart[,] mazeParts = new MazePart[lines[0].Length, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < lines[0].Length; x++)
            {
                int yAfterPaddingLater = y + 1;
                int xAfterPaddingLater = x + 1;
                mazeParts[x, y] = new MazePart(yAfterPaddingLater, xAfterPaddingLater, ToType(line[x]));
            }
        }

        mazeParts = mazeParts.PadArray(1, new MazePart(-1, -1, MazePartType.Ground));
        return new Maze(mazeParts).ExploreSurroundingsAndReturn();

        MazePartType ToType(char symbol)
        {
            return symbol switch
            {
                'S' => MazePartType.Start,
                '.' => MazePartType.Ground,
                '|' => MazePartType.VerticalPipe,
                '-' => MazePartType.HorizontalPipe,
                'L' => MazePartType.NorthAndEastBend,
                'J' => MazePartType.NorthAndWestBend,
                '7' => MazePartType.SouthAndWestBend,
                'F' => MazePartType.SouthAndEastBend,
                _ => throw new NotSupportedException($"Symbol '{symbol}' is not supported")
            };
        }
    }
}

public record Maze(MazePart[,] Parts)
{
    public MazePart Start { get; set; }

    public Maze ExploreSurroundingsAndReturn()
    {
        for (int y = 0; y < Parts.GetLength(1); y++)
        {
            for (int x = 0; x < Parts.GetLength(0); x++)
            {
                MazePart mazePart = Parts[x, y];
                mazePart.AttachNorthSouthEastAndWest(Parts);
                mazePart.DetermineStartAndValidConnections(this);
            }
        }

        return this;
    }
}

[DebuggerDisplay("{Type}")]
public class MazePart(int yCoordinate, int xCoordinate, MazePartType type)
{
    public string Id { get; } = $"{xCoordinate}_{yCoordinate}";
    public MazePart North { get; set; }
    public bool CanGoNorth { get; set; }
    public MazePart South { get; set; }
    public bool CanGoSouth { get; set; }
    public MazePart East { get; set; }
    public bool CanGoEast { get; set; }
    public MazePart West { get; set; }
    public bool CanGoWest { get; set; }
    public MazePart[] ValidConnections { get; set; } = Array.Empty<MazePart>();
    public int Y { get; init; } = yCoordinate;
    public int X { get; init; } = xCoordinate;
    public MazePartType Type { get; private set; } = type;

    public void AttachNorthSouthEastAndWest(MazePart[,] allMazeParts)
    {
        if (Y == -1 || X == -1)
        {
            //Padding area so no need to do anything
            return;
        }
        North = allMazeParts[X, Y - 1];
        South = allMazeParts[X, Y + 1];
        East = allMazeParts[X + 1, Y];
        West = allMazeParts[X - 1, Y];
    }


    public void DetermineStartAndValidConnections(Maze maze)
    {
        if (Y == -1 || X == -1)
        {
            //Padding area so no need to do anything
            return;
        }

        switch (Type)
        {
            case MazePartType.Start:
                maze.Start = this;
                CanGoNorth = North.Type is MazePartType.VerticalPipe or MazePartType.SouthAndEastBend or MazePartType.SouthAndWestBend;
                CanGoSouth = South.Type is MazePartType.VerticalPipe or MazePartType.NorthAndEastBend or MazePartType.NorthAndWestBend;
                CanGoEast = East.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndWestBend or MazePartType.SouthAndWestBend;
                CanGoWest = West.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndEastBend or MazePartType.SouthAndEastBend;

                List<MazePartType> possibleStartTypes =
                [
                    MazePartType.VerticalPipe,
                    MazePartType.HorizontalPipe,
                    MazePartType.SouthAndWestBend,
                    MazePartType.NorthAndWestBend,
                    MazePartType.SouthAndEastBend,
                    MazePartType.NorthAndEastBend
                ];

                if (CanGoNorth)
                {
                    ValidConnections = ValidConnections.Append(North).ToArray();
                    possibleStartTypes.Remove(MazePartType.HorizontalPipe);
                    possibleStartTypes.Remove(MazePartType.SouthAndWestBend);
                    possibleStartTypes.Remove(MazePartType.SouthAndEastBend);
                }
                if (CanGoSouth)
                {
                    ValidConnections = ValidConnections.Append(South).ToArray();
                    possibleStartTypes.Remove(MazePartType.HorizontalPipe);
                    possibleStartTypes.Remove(MazePartType.NorthAndWestBend);
                    possibleStartTypes.Remove(MazePartType.NorthAndEastBend);
                }
                if (CanGoEast)
                {
                    ValidConnections = ValidConnections.Append(East).ToArray();
                    possibleStartTypes.Remove(MazePartType.VerticalPipe);
                    possibleStartTypes.Remove(MazePartType.NorthAndWestBend);
                    possibleStartTypes.Remove(MazePartType.SouthAndWestBend);
                }
                if (CanGoWest)
                {
                    ValidConnections = ValidConnections.Append(West).ToArray();
                    possibleStartTypes.Remove(MazePartType.VerticalPipe);
                    possibleStartTypes.Remove(MazePartType.NorthAndEastBend);
                    possibleStartTypes.Remove(MazePartType.SouthAndEastBend);
                }

                Type = possibleStartTypes.Single();
                break;
            case MazePartType.VerticalPipe:
                CanGoNorth = North.Type is MazePartType.VerticalPipe or MazePartType.SouthAndEastBend or MazePartType.SouthAndWestBend;
                CanGoSouth = South.Type is MazePartType.VerticalPipe or MazePartType.NorthAndEastBend or MazePartType.NorthAndWestBend;
                CanGoEast = false;
                CanGoWest = false;
                if (CanGoNorth && CanGoSouth)
                {
                    ValidConnections = [North, South];
                }
                break;
            case MazePartType.HorizontalPipe:
                CanGoNorth = false;
                CanGoSouth = false;
                CanGoEast = East.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndWestBend or MazePartType.SouthAndWestBend;
                CanGoWest = West.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndEastBend or MazePartType.SouthAndEastBend;
                if (CanGoEast && CanGoWest)
                {
                    ValidConnections = [East, West];
                }
                break;
            case MazePartType.NorthAndEastBend:
                CanGoNorth = North.Type is MazePartType.VerticalPipe or MazePartType.SouthAndEastBend or MazePartType.SouthAndWestBend;
                CanGoSouth = false;
                CanGoEast = East.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndWestBend or MazePartType.SouthAndWestBend;
                CanGoWest = false;
                if (CanGoNorth && CanGoEast)
                {
                    ValidConnections = [North, East];
                }
                break;
            case MazePartType.NorthAndWestBend:
                CanGoNorth = North.Type is MazePartType.VerticalPipe or MazePartType.SouthAndEastBend or MazePartType.SouthAndWestBend;
                CanGoSouth = false;
                CanGoEast = false;
                CanGoWest = West.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndEastBend or MazePartType.SouthAndEastBend;
                if (CanGoNorth && CanGoWest)
                {
                    ValidConnections = [North, West];
                }
                break;
            case MazePartType.SouthAndEastBend:
                CanGoNorth = false;
                CanGoSouth = South.Type is MazePartType.VerticalPipe or MazePartType.NorthAndEastBend or MazePartType.NorthAndWestBend;
                CanGoEast = East.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndWestBend or MazePartType.SouthAndWestBend;
                CanGoWest = false;
                if (CanGoSouth && CanGoEast)
                {
                    ValidConnections = [North, South];
                }
                break;
            case MazePartType.SouthAndWestBend:
                CanGoNorth = false;
                CanGoSouth = South.Type is MazePartType.VerticalPipe or MazePartType.NorthAndEastBend or MazePartType.NorthAndWestBend;
                CanGoEast = false;
                CanGoWest = West.Type is MazePartType.HorizontalPipe or MazePartType.NorthAndEastBend or MazePartType.SouthAndEastBend;
                if (CanGoSouth && CanGoWest)
                {
                    ValidConnections = [South, West];
                }
                break;
            case MazePartType.Ground:
                CanGoNorth = false;
                CanGoSouth = false;
                CanGoEast = false;
                CanGoWest = false;
                ValidConnections = Array.Empty<MazePart>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public int GetStepsToFarthestPoint()
    {
        int steps = 1;
        List<string> explored = [Id];
        MazePart direction1 = ValidConnections.First();
        MazePart direction2 = ValidConnections.Last();
        while (direction1.Id != direction2.Id)
        {
            explored.Add(direction1.Id);
            explored.Add(direction2.Id);
            direction1 = direction1.ValidConnections.Single(x => !explored.Contains(x.Id));
            direction2 = direction2.ValidConnections.Single(x => !explored.Contains(x.Id));
            steps++;
        }

        return steps;
    }
}

public enum MazePartType
{
    Start,
    VerticalPipe,
    HorizontalPipe,
    NorthAndEastBend,
    NorthAndWestBend,
    SouthAndEastBend,
    SouthAndWestBend,
    Ground,
    Invalid
}