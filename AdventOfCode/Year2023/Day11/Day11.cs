using System.Text;
using Utils;
using Year2023.Day10;

namespace Year2023.Day11;

//https://adventofcode.com/2023/day/11
public class Day11
{
    private const string Day = "Day11";

    //What is the sum of these lengths?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 374)]
    [InlineData($"{Day}\\Input.txt", 9329143)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var starMap = inputFile.ToLines().ToDay11StarMap(1);
        //var visualize = starMap.Visualize(true);
        var calculatedAnswer = starMap.FindShortestPathsAndSum();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //What is the sum of the ancient galaxy lenghts
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 10, 1030)]
    [InlineData($"{Day}\\Sample.txt", 100, 8410)]
    //[InlineData($"{Day}\\Input.txt", 0)]
    public void Part2(string inputFile, int sizeIncrease, int expectedAnswer)
    {
        var starMap = inputFile.ToLines().ToDay11StarMap(sizeIncrease-1);
        //var visualize = starMap.Visualize(true);
        var calculatedAnswer = starMap.FindShortestPathsAndSum();
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public static class Day11Extensions
{
    public static StarMap ToDay11StarMap(this string[] inputLines, int emptySpaceIncrease)
    {
        List<int> rowsWithNoGalaxies = FindEmptyRows();
        List<int> columnsWithNoGalaxies = FindEmptyColumns();
        List<Point> galaxyLocations = [];
        long columns = inputLines[0].Length;
        long columnsExpanded = columns + columnsWithNoGalaxies.Count*emptySpaceIncrease;
        long rows = inputLines.Length;
        long rowsExpanded = rows + rowsWithNoGalaxies.Count*emptySpaceIncrease;
        long rowOffset = 0;

        Point[,] points = new Point[columnsExpanded, rowsExpanded];
        int galaxyNumber = 0;

        for (int row = 0; row < rows; row++)
        {
            var rowData = inputLines[row];
            AddColumnData();
            if (rowsWithNoGalaxies.Contains(row))
            {
                for (int i = 0; i < emptySpaceIncrease; i++)
                {
                    rowOffset++;
                    AddColumnData();
                }
            }

            void AddColumnData()
            {
                var columnOffset = 0;
                for (int column = 0; column < columns; column++)
                {
                    var content = rowData[column];
                    bool isGalaxy = content == '#';
                    var y = row + rowOffset;
                    if (isGalaxy)
                    {
                        galaxyNumber++;
                        var x = column + columnOffset;
                        var galaxy = new Point('#', x, y, galaxyNumber);
                        galaxyLocations.Add(galaxy);
                        points[x, y] = galaxy;
                    }
                    else
                    {
                        var x = column + columnOffset;
                        points[x, y] = new Point('.', x, y);
                    }

                    if (columnsWithNoGalaxies.Contains(column))
                    {
                        for (int i = 0; i < emptySpaceIncrease; i++)
                        {
                            columnOffset++;
                            var x = column + columnOffset;
                            points[x, y] = new Point('.', x, y);
                        }
                    }
                }
            }
        }

        return new StarMap(points, galaxyLocations.ToArray());

        List<int> FindEmptyRows()
        {
            List<int> result = [];
            for (int r = 0; r < inputLines.Length; r++)
            {
                if (!inputLines[r].Contains("#"))
                {
                    result.Add(r);
                }
            }

            return result;
        }

        List<int> FindEmptyColumns()
        {
            var columnCount = inputLines[0].Length;
            List<int> result = [];
            for (int c = 0; c < columnCount; c++)
            {
                var columnToExtract = c;
                var columnData = inputLines.Select(x => x.Substring(columnToExtract, 1));
                if (!columnData.Contains("#"))
                {
                    result.Add(c);
                }
            }

            return result;
        }
    }
}

public class Point(char content, long x, long y, int galaxyNumber = -1)
{
    public char Content { get; } = content;
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
    public int GalaxyNumber { get; } = galaxyNumber;
    public Dictionary<int, int> Routes { get; set; } = new();
}

public record StarMap(Point[,] Points, Point[] GalaxyLocations)
{
    public string Visualize(bool showGalaxyNumberIfLowEnough)
    {
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < Points.GetLength(1); y++)
        {
            for (int x = 0; x < Points.GetLength(0); x++)
            {
                var point = Points[x, y];
                if (point.GalaxyNumber > 0 && point.GalaxyNumber <= 9)
                {
                    sb.Append((point.GalaxyNumber));
                }
                else
                {
                    sb.Append((point?.Content ?? '?'));
                }


            }
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }


    public int FindShortestPathsAndSum()
    {
        int result = 0;
        foreach (Point from in GalaxyLocations)
        {
            foreach (Point to in GalaxyLocations)
            {
                if (from == to)
                {
                    continue;
                }

                if (from.Routes.ContainsKey(to.GalaxyNumber) || to.Routes.ContainsKey(from.GalaxyNumber))
                {
                    continue;//Already visited
                }

                int moves = 0;
                var pathLocation = new Point('X', from.X, from.Y);
                while (pathLocation.X != to.X || pathLocation.Y != to.Y)
                {
                    var diffX = Math.Abs(pathLocation.X - to.X);
                    var diffY = Math.Abs(pathLocation.Y - to.Y);
                    if (diffY > diffX)
                    {
                        //Move Up/down
                        if (pathLocation.Y > to.Y)
                        {
                            //Move Up
                            pathLocation.Y--;
                        }
                        else
                        {
                            //Move Down
                            pathLocation.Y++;
                        }
                    }
                    else
                    {
                        //Move Left/Right
                        if (pathLocation.X > to.X)
                        {
                            //Move Left
                            pathLocation.X--;
                        }
                        else
                        {
                            //Move Right
                            pathLocation.X++;
                        }
                    }

                    moves++;
                }


                from.Routes.Add(to.GalaxyNumber, moves);
                to.Routes.Add(from.GalaxyNumber, moves);
                result += moves;
            }
        }

        return result;
    }
}