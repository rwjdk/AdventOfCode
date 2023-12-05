using Utils;
using Year2023;

var almanac = InputReader.ReadInputLines("Day05_Input.txt").ToDay05Almanac();
long lowestLocation = long.MaxValue;
var seedRanges = almanac.GetSeedRanges();
foreach (var seedRange in seedRanges)
{
    Console.WriteLine("Processing Seed Range: " + seedRange.ToString());
    long location = seedRange.GetLowestLocation(almanac);
    if (location < lowestLocation)
    {
        lowestLocation = location;
    }
}
Console.WriteLine(lowestLocation);
File.WriteAllText("D:\\answer.txt", lowestLocation.ToString());
Console.ReadLine();