using System.Diagnostics;
using System.Text;
using Utils;

namespace Year2023.Day10;

//https://adventofcode.com/2023/day/10
public class Day10
{
    private const string Day = "Day10";

    //How many steps along the loop does it take to get from the starting position to the point farthest from the starting position?
    [Theory]
    [InlineData($"{Day}\\Sample1.txt", 4)]
    [InlineData($"{Day}\\Sample2.txt", 4)]
    [InlineData($"{Day}\\Sample3.txt", 8)]
    [InlineData($"{Day}\\Sample4.txt", 8)]
    [InlineData($"{Day}\\Input.txt", 6682)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var maze = inputFile.ToLines().ToDay10Maze();
        //var visualize = maze.Visualize();
        var calculatedAnswer = maze.Start!.GetStepsToFarthestPoint();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //How many tiles are enclosed by the loop?
    [Theory]
    //[InlineData($"{Day}\\Sample5.txt", 4)]
    //[InlineData($"{Day}\\Sample6.txt", 4)]
    //[InlineData($"{Day}\\Sample7.txt", 8)]
    [InlineData($"{Day}\\Sample8.txt", 10)]
    //[InlineData($"{Day}\\Input.txt", 0)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var maze = inputFile.ToLines().ToDay10Maze();
        //var visualize = maze.Visualize();
        maze.RemoveNonLoopJunkPipes();

        var visualize1 = maze.Visualize();
        //todo - enlarge maze so pipe gaps are own tile
        maze.FloodFillTileFromEdgesWithEscapeGround();
        var visualize2 = maze.Visualize();
        var enclosedTiles = maze.GetRemainingGroundTiles().Length;
        Assert.Equal(expectedAnswer, enclosedTiles);
    }
}

public static class Day10Extensions
{
    public static Maze ToDay10Maze(this string[] lines)
    {
        MazeTile[,] mazeTile = new MazeTile[lines[0].Length, lines.Length];
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < lines[0].Length; x++)
            {
                var symbol = line[x];
                mazeTile[x, y] = new MazeTile(y, x, ToType(symbol), symbol);
            }
        }
        return new Maze(mazeTile).ExploreSurroundingsAndReturn();
    }

    private static MazeTileType ToType(char symbol)
    {
        return symbol switch
        {
            'S' => MazeTileType.Start,
            '.' => MazeTileType.Ground,
            'O' => MazeTileType.Ground,
            'I' => MazeTileType.Ground,
            '|' => MazeTileType.VerticalPipe,
            '-' => MazeTileType.HorizontalPipe,
            'L' => MazeTileType.NorthAndEastBend,
            'J' => MazeTileType.NorthAndWestBend,
            '7' => MazeTileType.SouthAndWestBend,
            'F' => MazeTileType.SouthAndEastBend,
            _ => throw new NotSupportedException($"Symbol '{symbol}' is not supported")
        };
    }

    public static void FloodFillWithEscapeGround(this MazeTile escapeTile)
    {
        foreach (MazeTile tile in escapeTile.ValidEscapeTiles.Where(x => x.Type == MazeTileType.Ground || x.Symbol == 'V' || x.Symbol == 'H' ||x.Symbol == '.'))
        {
            tile.ChangeToOutside();
            tile.FloodFillWithEscapeGround();
        }
    }
}

public record Maze(MazeTile[,] Tiles)
{
    public MazeTile? Start { get; private set; }

    public Maze ExploreSurroundingsAndReturn()
    {
        Start = GetStart();
        Start.DetermineStartType(Tiles);
        var allTiles = GetAllTiles().ToArray();
        foreach (MazeTile tile in allTiles)
        {
            tile.AttachNorthSouthEastAndWest(Tiles);
        }
        foreach (MazeTile tile in allTiles)
        {
            tile.DetermineValidConnections();
        }
        return this;

        MazeTile GetStart()
        {
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    MazeTile mazeTile = Tiles[x, y];
                    if (mazeTile.Type == MazeTileType.Start)
                    {
                        return mazeTile;
                    }
                }
            }

            throw new NotSupportedException("No Start was found");
        }
    }

    private IEnumerable<MazeTile> GetAllTiles()
    {
        for (int y = 0; y < Tiles.GetLength(1); y++)
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                yield return Tiles[x, y];
            }
        }
    }

    public string Visualize()
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < Tiles.GetLength(1); y++)
        {
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                MazeTile mazeTile = Tiles[x, y];
                if (mazeTile.Symbol == '.' && mazeTile.Type != MazeTileType.Ground)
                {
                    switch (mazeTile.Type)
                    {
                        case MazeTileType.SouthAndWestBend:
                            sb.Append("7");
                            break;
                        case MazeTileType.SouthAndEastBend:
                            sb.Append("F");
                            break;
                        case MazeTileType.NorthAndWestBend:
                            sb.Append("J");
                            break;
                        case MazeTileType.NorthAndEastBend:
                            sb.Append("F");
                            break;
                        default:
                            sb.Append(mazeTile.Symbol);
                            break;
                    }
                }
                else
                {
                    sb.Append(mazeTile.Symbol);
                }
            }
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public void RemoveNonLoopJunkPipes()
    {
        var allTiles = GetAllTiles().ToList();
        foreach (MazeTile tile in allTiles)
        {
            if (!tile.PartOfLoop)
            {
                tile.ChangeToGround();
            }
        }

        foreach (MazeTile tile in allTiles)
        {
            if (tile.PartOfLoop)
            {
                tile.DetermineTunnels();
            }
        }

        foreach (MazeTile tile in allTiles)
        {
            if (tile.Id == "X4Y8")
            {
                int i = 0;
            }
            if (!tile.PartOfLoop || tile.Symbol == 'V' || tile.Symbol == 'H' || tile.Symbol == '.')
            {
                tile.DetermineValidEscapeTiles(this);
            }
        }
    }

    public void FloodFillTileFromEdgesWithEscapeGround()
    {
        var edgeTiles = GetGroundEdgeTiles();
        foreach (MazeTile edgeTile in edgeTiles)
        {
            edgeTile.FloodFillWithEscapeGround();
        }
    }

    private MazeTile[] GetGroundEdgeTiles()
    {
        return GetAllTiles().Where(tile => tile.Type == MazeTileType.Ground && (tile.X == 0 || tile.X == Tiles.GetLength(0) - 1 || tile.Y == 0 || tile.Y == Tiles.GetLength(1) - 1)).ToArray();
    }

    public MazeTile[] GetRemainingGroundTiles()
    {
        var mazeTiles = GetAllTiles().Where(x => x.Type == MazeTileType.Ground);
        return mazeTiles.ToArray();
    }
}

[DebuggerDisplay("{Type} | X={X} | Y={Y}")]
public class MazeTile(int y, int x, MazeTileType type, char symbol)
{
    public string Id { get; } = $"X{x}Y{y}";
    private MazeTile? North { get; set; }
    private bool CanGoNorth { get; set; }
    public MazeTile? South { get; set; }
    private bool CanGoSouth { get; set; }
    private MazeTile? East { get; set; }
    private bool CanGoEast { get; set; }
    private MazeTile? West { get; set; }
    private bool CanGoWest { get; set; }
    private MazeTile[] ValidConnections { get; set; } = [];
    public MazeTile[] ValidEscapeTiles { get; private set; } = [];
    public int Y { get; } = y;
    public int X { get; } = x;
    public MazeTileType Type { get; internal set; } = type;
    public char Symbol { get; set; } = symbol;
    public bool PartOfLoop { get; private set; }
    public bool Start { get; set; }

    public void AttachNorthSouthEastAndWest(MazeTile[,] allMazeTiles)
    {
        North = Y - 1 >= 0 ? allMazeTiles[X, Y - 1] : null;
        South = Y + 1 < allMazeTiles.GetLength(1) ? allMazeTiles[X, Y + 1] : null;
        East = X + 1 < allMazeTiles.GetLength(0) ? allMazeTiles[X + 1, Y] : null;
        West = X - 1 >= 0 ? allMazeTiles[X - 1, Y] : null;
    }

    public void DetermineStartType(MazeTile[,] allMazeTiles)
    {
        AttachNorthSouthEastAndWest(allMazeTiles);
        CanGoNorth = North?.Type is MazeTileType.VerticalPipe or MazeTileType.SouthAndEastBend or MazeTileType.SouthAndWestBend;
        CanGoSouth = South?.Type is MazeTileType.VerticalPipe or MazeTileType.NorthAndEastBend or MazeTileType.NorthAndWestBend;
        CanGoEast = East?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndWestBend or MazeTileType.SouthAndWestBend;
        CanGoWest = West?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndEastBend or MazeTileType.SouthAndEastBend;

        List<MazeTileType> possibleStartTypes =
        [
            MazeTileType.VerticalPipe,
            MazeTileType.HorizontalPipe,
            MazeTileType.SouthAndWestBend,
            MazeTileType.NorthAndWestBend,
            MazeTileType.SouthAndEastBend,
            MazeTileType.NorthAndEastBend
        ];

        if (CanGoNorth)
        {
            North!.PartOfLoop = true;
            ValidConnections = ValidConnections.Append(North!).ToArray();
            possibleStartTypes.Remove(MazeTileType.HorizontalPipe);
            possibleStartTypes.Remove(MazeTileType.SouthAndWestBend);
            possibleStartTypes.Remove(MazeTileType.SouthAndEastBend);
        }
        if (CanGoSouth)
        {
            South!.PartOfLoop = true;
            ValidConnections = ValidConnections.Append(South!).ToArray();
            possibleStartTypes.Remove(MazeTileType.HorizontalPipe);
            possibleStartTypes.Remove(MazeTileType.NorthAndWestBend);
            possibleStartTypes.Remove(MazeTileType.NorthAndEastBend);
        }
        if (CanGoEast)
        {
            East!.PartOfLoop = true;
            ValidConnections = ValidConnections.Append(East!).ToArray();
            possibleStartTypes.Remove(MazeTileType.VerticalPipe);
            possibleStartTypes.Remove(MazeTileType.NorthAndWestBend);
            possibleStartTypes.Remove(MazeTileType.SouthAndWestBend);
        }
        if (CanGoWest)
        {
            West!.PartOfLoop = true;
            ValidConnections = ValidConnections.Append(West!).ToArray();
            possibleStartTypes.Remove(MazeTileType.VerticalPipe);
            possibleStartTypes.Remove(MazeTileType.NorthAndEastBend);
            possibleStartTypes.Remove(MazeTileType.SouthAndEastBend);
        }

        Type = possibleStartTypes.Single();
        Start = true;
    }

    public void DetermineValidConnections()
    {
        switch (Type)
        {
            case MazeTileType.VerticalPipe:
                CanGoNorth = North?.Type is MazeTileType.VerticalPipe or MazeTileType.SouthAndEastBend or MazeTileType.SouthAndWestBend;
                CanGoSouth = South?.Type is MazeTileType.VerticalPipe or MazeTileType.NorthAndEastBend or MazeTileType.NorthAndWestBend;
                CanGoEast = false;
                CanGoWest = false;
                if (CanGoNorth && CanGoSouth)
                {
                    North!.PartOfLoop = true;
                    South!.PartOfLoop = true;
                    ValidConnections = [North, South];
                }
                break;
            case MazeTileType.HorizontalPipe:
                CanGoNorth = false;
                CanGoSouth = false;
                CanGoEast = East?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndWestBend or MazeTileType.SouthAndWestBend;
                CanGoWest = West?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndEastBend or MazeTileType.SouthAndEastBend;
                if (CanGoEast && CanGoWest)
                {
                    East!.PartOfLoop = true;
                    West!.PartOfLoop = true;
                    ValidConnections = [East, West];
                }
                break;
            case MazeTileType.NorthAndEastBend:
                CanGoNorth = North?.Type is MazeTileType.VerticalPipe or MazeTileType.SouthAndEastBend or MazeTileType.SouthAndWestBend;
                CanGoSouth = false;
                CanGoEast = East?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndWestBend or MazeTileType.SouthAndWestBend;
                CanGoWest = false;
                if (CanGoNorth && CanGoEast)
                {
                    North!.PartOfLoop = true;
                    East!.PartOfLoop = true;
                    ValidConnections = [North, East];
                }
                break;
            case MazeTileType.NorthAndWestBend:
                CanGoNorth = North?.Type is MazeTileType.VerticalPipe or MazeTileType.SouthAndEastBend or MazeTileType.SouthAndWestBend;
                CanGoSouth = false;
                CanGoEast = false;
                CanGoWest = West?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndEastBend or MazeTileType.SouthAndEastBend;
                if (CanGoNorth && CanGoWest)
                {
                    North!.PartOfLoop = true;
                    West!.PartOfLoop = true;
                    ValidConnections = [North, West];
                }
                break;
            case MazeTileType.SouthAndEastBend:
                CanGoNorth = false;
                CanGoSouth = South?.Type is MazeTileType.VerticalPipe or MazeTileType.NorthAndEastBend or MazeTileType.NorthAndWestBend;
                CanGoEast = East?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndWestBend or MazeTileType.SouthAndWestBend;
                CanGoWest = false;
                if (CanGoSouth && CanGoEast)
                {
                    South!.PartOfLoop = true;
                    East!.PartOfLoop = true;
                    ValidConnections = [South, East];
                }
                break;
            case MazeTileType.SouthAndWestBend:
                CanGoNorth = false;
                CanGoSouth = South?.Type is MazeTileType.VerticalPipe or MazeTileType.NorthAndEastBend or MazeTileType.NorthAndWestBend;
                CanGoEast = false;
                CanGoWest = West?.Type is MazeTileType.HorizontalPipe or MazeTileType.NorthAndEastBend or MazeTileType.SouthAndEastBend;
                if (CanGoSouth && CanGoWest)
                {
                    South!.PartOfLoop = true;
                    West!.PartOfLoop = true;
                    ValidConnections = [South, West];
                }
                break;
            case MazeTileType.Ground:
                CanGoNorth = false;
                CanGoSouth = false;
                CanGoEast = false;
                CanGoWest = false;
                ValidConnections = Array.Empty<MazeTile>();
                break;
        }
    }

    public int GetStepsToFarthestPoint()
    {
        int steps = 1;
        List<string> explored = [Id];
        MazeTile direction1 = ValidConnections.First();
        MazeTile direction2 = ValidConnections.Last();
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

    public void ChangeToGround()
    {
        Symbol = '.';
        Type = MazeTileType.Ground;
    }

    public void DetermineValidEscapeTiles(Maze maze)
    {
        if (North?.Type == MazeTileType.Ground || North?.Symbol == 'V' || North?.Symbol == '.')
        {
            if (Symbol == 'V' || Symbol == '.')
            {
                AddValidEscapeTile(North);
                North.AddValidEscapeTile(this);
            }
        }

        if (South?.Type == MazeTileType.Ground || South?.Symbol == 'V' || South?.Symbol == '.')
        {
            if (Symbol == 'V' || Symbol == '.')
            {
                AddValidEscapeTile(South);
                South.AddValidEscapeTile(this);
            }
        }
        if (East?.Type == MazeTileType.Ground || East?.Symbol == 'H' || East?.Symbol == '.')
        {
            if (Symbol == 'H' || Symbol == '.')
            {
                AddValidEscapeTile(East);
                East.AddValidEscapeTile(this);
            }
        }
        if (West?.Type == MazeTileType.Ground || West?.Symbol == 'H' || West?.Symbol == '.')
        {
            if (symbol == 'H' || symbol == '.')
            {
                AddValidEscapeTile(West);
                West.AddValidEscapeTile(this);
            }
        }
    }

    private void AddValidEscapeTile(MazeTile mazeTile)
    {
        ValidEscapeTiles = ValidEscapeTiles.Append(mazeTile).ToArray();
    }

    public void ChangeToOutside()
    {
        Symbol = 'O';
        Type = MazeTileType.GroundOutside;
    }

    public void DetermineTunnels()
    {
        if (Type == MazeTileType.NorthAndEastBend && West?.Type is MazeTileType.NorthAndWestBend or MazeTileType.SouthAndWestBend)
        {
            Symbol = 'V';
        }

        if (Type == MazeTileType.NorthAndWestBend && East?.Type is MazeTileType.NorthAndEastBend or MazeTileType.SouthAndEastBend)
        {
            Symbol = 'V';
        }

        if (Type == MazeTileType.SouthAndEastBend && West?.Type is MazeTileType.SouthAndWestBend or MazeTileType.NorthAndWestBend)
        {
            Symbol = 'V';
        }

        if (Type == MazeTileType.SouthAndWestBend && East?.Type is MazeTileType.SouthAndEastBend or MazeTileType.NorthAndEastBend)
        {
            Symbol = 'V';
        }

        if (Type == MazeTileType.VerticalPipe && (West?.Type == MazeTileType.VerticalPipe || East?.Type == MazeTileType.VerticalPipe))
        {
            Symbol = 'V';
        }

        if (Type == MazeTileType.HorizontalPipe && (North?.Type == MazeTileType.HorizontalPipe || South?.Type == MazeTileType.HorizontalPipe))
        {
            Symbol = 'H';
        }

        if (Type == MazeTileType.SouthAndEastBend && (North?.Type == MazeTileType.Ground || West?.Type == MazeTileType.Ground))
        {
            Symbol = '.';
        }
    }
}

public enum MazeTileType
{
    Start,
    VerticalPipe,
    HorizontalPipe,
    NorthAndEastBend,
    NorthAndWestBend,
    SouthAndEastBend,
    SouthAndWestBend,
    Ground,
    Invalid,
    GroundOutside
}