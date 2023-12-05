using Utils;
using Year2023;

var almanac = InputReader.ReadInputLines("Day05_Input.txt").ToDay05Almanac();
long lowestLocation = long.MaxValue;
var seedRanges = almanac.GetSeedRanges();
List<Task<long>> tasks = new();
foreach (var seedRange in seedRanges)
{
    Console.WriteLine("Processing Seed Range: " + seedRange.ToString());
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
Console.WriteLine(lowestLocation);
File.WriteAllText("D:\\answer.txt", lowestLocation.ToString());
Console.ReadLine();