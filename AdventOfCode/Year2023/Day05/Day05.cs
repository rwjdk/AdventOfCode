﻿using System.Diagnostics;
using Utils;

namespace Year2023.Day05;

//https://adventofcode.com/2023/day/5
public class Day05
{
    private const string Day = "Day05";

    //What is the lowest location number that corresponds to any of the initial seed numbers?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 35)]
    [InlineData($"{Day}\\Input.txt", 579439039)]
    public void Part1(string inputFile, long expectedAnswer)
    {
        var almanac = inputFile.ToLines().ToDay05Almanac();
        long lowestLocation = long.MaxValue;
        foreach (var seed in almanac.Seeds)
        {
            long location = seed.GetLocation(almanac);
            if (location < lowestLocation)
            {
                lowestLocation = location;
            }
        }

        Assert.Equal(expectedAnswer, lowestLocation);
    }

    //What is the lowest location number that corresponds to any of the initial seed numbers (given that they are ranges)?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 46)]
    //[InlineData($"{Day}\\Input.txt", 7873084)] //This one take a loooong time to run (3+ minutes)
    public async Task Part2(string inputFile, int expectedAnswer)
    {
        var almanac = inputFile.ToLines().ToDay05Almanac();
        long lowestLocation = long.MaxValue;
        var seedRanges = almanac.GetSeedRanges();
        List<Task<long>> tasks = [];
        foreach (var seedRange in seedRanges)
        {
            var task = new Task<long>(() => seedRange.GetLowestLocation(almanac));
            task.Start();
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        foreach (var task in tasks)
        {
            long location = task.Result;
            if (location < lowestLocation)
            {
                lowestLocation = location;
            }
        }

        Assert.Equal(expectedAnswer, lowestLocation);
    }
}

public record SeedRange(long StartSeed, long Range)
{
    private readonly long _endSeed = StartSeed + Range;

    public long GetLowestLocation(Almanac almanac)
    {
        long lowestLocation = long.MaxValue;
        for (long i = StartSeed; i <= _endSeed; i++)
        {
            long location = i.GetLocation(almanac);
            if (location < lowestLocation)
            {
                lowestLocation = location;
            }
        }

        return lowestLocation;
    }

    public override string ToString()
    {
        return $"Start:{StartSeed} - Range: {Range}";
    }
}

public record Almanac(long[] Seeds, Dictionary<string, AlmanacMap> Maps)
{
    public List<SeedRange> GetSeedRanges()
    {
        var result = new List<SeedRange>();
        for (int i = 0; i < Seeds.Length; i++)
        {
            var seed = Seeds[i];
            var range = Seeds[i + 1];
            result.Add(new SeedRange(seed, range));
            i++;
        }

        return result;
    }
}

public record AlmanacMap(string SourceCategory, string DestinationCategory, List<AlmanacMapRanges> Ranges)
{
    public override string ToString()
    {
        return $"{SourceCategory} > {DestinationCategory}";
    }

    public long GetDestinationNumber(long source)
    {
        foreach (var range in Ranges)
        {
            if (source >= range.SourceRangeStart && source <= range.SourceRangeEnd)
            {
                var diff = source - range.SourceRangeStart;
                var destination = range.DestinationRangeStart + diff;
                return destination;
            }
        }

        //No Mapping, Source = Destination
        return source;
    }
}

public record AlmanacMapRanges(long DestinationRangeStart, long SourceRangeStart, long RangeLength)
{
    public long SourceRangeEnd => SourceRangeStart + RangeLength - 1;

    public override string ToString()
    {
        return $"{DestinationRangeStart} {SourceRangeStart} {RangeLength}";
    }
}

public static class Day05Extensions
{
    public static long GetLocation(this long seed, Almanac almanac)
    {
        var map = almanac.Maps["seed"];
        long result = map.GetDestinationNumber(seed);
        while (map != null)
        {
            almanac.Maps.TryGetValue(map.DestinationCategory, out map);
            if (map != null)
            {
                result = map.GetDestinationNumber(result);
            }
        }

        return result;
    }

    public static Almanac ToDay05Almanac(this string[] input)
    {
        var seeds = input[0].RemovePrefix().SplitToLongs();

        List<AlmanacMap> maps = [];
        AlmanacMap? currentMap = null;
        for (int inputLineId = 1; inputLineId < input.Length; inputLineId++)
        {
            string line = input[inputLineId];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.EndsWith(" map:"))
            {
                //Seed Map Header
                if (currentMap != null)
                {
                    maps.Add(currentMap);
                }

                line = line.Remove(line.Length - 5); //Remove ' map:' part (we do not need it)
                var sourceAndDestination = line.Split("-to-", StringSplitOptions.RemoveEmptyEntries);
                string sourceCategory = sourceAndDestination[0];
                string destinationCategory = sourceAndDestination[1];
                currentMap = new AlmanacMap(sourceCategory, destinationCategory, []);
            }
            else
            {
                //Seed Map Line
                long[] mapRangeNumbers = line.SplitToLongs();
                Debug.Assert(currentMap != null, nameof(currentMap) + " != null");
                currentMap.Ranges.Add(new AlmanacMapRanges(mapRangeNumbers[0], mapRangeNumbers[1], mapRangeNumbers[2]));
            }
        }

        //Make sure last map is included
        if (currentMap != null)
        {
            maps.Add(currentMap);
        }

        return new Almanac(seeds, maps.ToDictionary(x => x.SourceCategory, y => y));
    }
}