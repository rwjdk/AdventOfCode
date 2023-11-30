using Utils;
using Xunit;

namespace Day01;

public class UnitTest1
{
    [Theory]
    [InlineData("Sample.txt", "Elf 4", 24000)]
    [InlineData("Input1.txt", "Elf 151", 69912)]
    public void Test1(string filename, string elf, int calories)
    {
        string[] input = InputReader.ReadInputLines(filename);

        Dictionary<string, int> data = new();
        int elveNumber = 1;
        foreach (var line in input)
        {
            if (line.IsNullOrWhiteSpace())
            {
                elveNumber++;
                continue;
            }
            var key = $"Elf {elveNumber}";
            data.TryAdd(key, 0);
            data[key]+= line.ToInteger();
        }

        var maxElf = data.MaxBy(x => x.Value);
        Assert.Equal(elf, maxElf.Key);
        Assert.Equal(calories, maxElf.Value);
    }
}