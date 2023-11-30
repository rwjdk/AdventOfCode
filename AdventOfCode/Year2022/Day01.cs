namespace Year2022;

public class Day01
{
    [Theory]
    [InlineData("Day01_Sample.txt", "Elf 4", 24000)]
    [InlineData("Day01_Input.txt", "Elf 151", 69912)]
    public void Part1(string inputFile, string elf, int calories)
    {
        var data = GetElvesAndTheirCarriedCalories(inputFile);
        var maxElf = data.MaxBy(x => x.Value);
        Assert.Equal(elf, maxElf.Key);
        Assert.Equal(calories, maxElf.Value);
    }
    
    [Theory]
    [InlineData("Day01_Sample.txt", "Elf 4", "Elf 3", "Elf 5", 45000)]
    [InlineData("Day01_Input.txt", "Elf 151", "Elf 36", "Elf 211", 208180)]
    public void Part2(string inputFile, string top1Elf, string top2Elf, string top3Elf, int top3TotalCalories)
    {
        var data = GetElvesAndTheirCarriedCalories(inputFile);
        var top3Elves = data.OrderByDescending(x=> x.Value).Take(3).ToDictionary();
        Assert.Equal(top1Elf, top3Elves.Keys.ToList()[0]);
        Assert.Equal(top2Elf, top3Elves.Keys.ToList()[1]);
        Assert.Equal(top3Elf, top3Elves.Keys.ToList()[2]);
        Assert.Equal(top3TotalCalories, top3Elves.Sum(x=> x.Value));
    }

    private static Dictionary<string, int> GetElvesAndTheirCarriedCalories(string inputFile)
    {
        string[] inputLines = InputReader.ReadInputLines(inputFile);

        Dictionary<string, int> data = new();
        int elveNumber = 1;
        foreach (var line in inputLines)
        {
            if (line.IsNullOrWhiteSpace())
            {
                elveNumber++;
                continue;
            }

            var key = $"Elf {elveNumber}";
            data.TryAdd(key, 0);
            data[key] += line.ToInteger();
        }

        return data;
    }
}