using Utils;

namespace Year2023;

//https://adventofcode.com/2023/day/4
public class Day04
{
    private const string Day = "Day04";

    //How many points are they (cards) worth in total?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 13)]
    [InlineData($"{Day}_Input.txt", 18619)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var cards = InputReader.ReadInputLines(inputFile).Select(x => x.ToDay04Card()).ToList();
        int calculatedAnswer = cards.Sum(x => x.GetPoints());
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //How many total scratchcards do you end up with?
    [Theory]
    [InlineData($"{Day}_Sample.txt", 30)]
    [InlineData($"{Day}_Input.txt", 8_063_216)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var cards = InputReader.ReadInputLines(inputFile).Select(x => x.ToDay04Card()).ToList();
        var lookupTable = cards.ToDictionary(x => x.Number, y => y);

        for (int cardIndex = 0; cardIndex < cards.Count; cardIndex++)
        {
            var card = cards[cardIndex];
            var copiesOfNextToAdd = card.GetWinningNumbersYouHave();
            for (int copyIndex = 1; copyIndex <= copiesOfNextToAdd; copyIndex++)
            {
                if (lookupTable.TryGetValue(card.Number + copyIndex, out Day04Card? copy))
                {
                    cards.Add(copy);
                }
            }
        }
        Assert.Equal(expectedAnswer, cards.Count);
    }
}

public static class Day04Extensions
{
    public static Day04Card ToDay04Card(this string line)
    {
        string[] cardAndNumbers = line.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        int card = Convert.ToInt32(cardAndNumbers[0][4..].Trim());

        string[] numberParts = cardAndNumbers[1].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        int[] winningNumbers = numberParts[0].SplitToNumbers(' ');
        int[] yourNumbers = numberParts[1].SplitToNumbers(' ');

        return new Day04Card(card, winningNumbers, yourNumbers);


    }
}

public record Day04Card(int Number, int[] WinningNumbers, int[] YourNumbers)
{
    public int GetPoints()
    {
        var winningNumbersYouHave = GetWinningNumbersYouHave();
        int nextPointValue = 1;
        int points = 0;
        for (int i = 0; i < winningNumbersYouHave; i++)
        {
            points = nextPointValue;
            nextPointValue = points * 2;
        }
        return points;
    }

    private int _winningNumbersYouHaveCache = -1;

    public int GetWinningNumbersYouHave()
    {
        if (_winningNumbersYouHaveCache != -1)
        {
            return _winningNumbersYouHaveCache;
        }
        _winningNumbersYouHaveCache = YourNumbers.Intersect(WinningNumbers).Count();
        return _winningNumbersYouHaveCache;
    }
}