using Utils;

namespace Year2023.Day04;

//https://adventofcode.com/2023/day/4
public class Day04
{
    private const string Day = "Day04";

    //How many points are they (cards) worth in total?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 13)]
    [InlineData($"{Day}\\Input.txt", 18619)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        var cards = inputFile.ToLines().Select(x => x.ToDay04Card()).ToList();
        int calculatedAnswer = cards.Sum(x => x.GetPoints());
        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //How many total scratchcards do you end up with?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 30)]
    [InlineData($"{Day}\\Input.txt", 8_063_216)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        var cards = inputFile.ToLines().Select(x => x.ToDay04Card()).ToList();
        var lookupTable = cards.ToDictionary(x => x.Number, y => y);

        for (int cardIndex = 0; cardIndex < cards.Count; cardIndex++)
        {
            var card = cards[cardIndex];
            var copiesOfNextToAdd = card.GetWinningNumbersYouHave();
            for (int copyIndex = 1; copyIndex <= copiesOfNextToAdd; copyIndex++)
            {
                if (lookupTable.TryGetValue(card.Number + copyIndex, out Card? copy))
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
    public static Card ToDay04Card(this string line)
    {
        var (_, number, restOfString) = line.GetPrefixWithInteger();
        var WinningAndYourNumbers = restOfString.SplitTwice<int>('|', ' ');
        return new Card(number, WinningAndYourNumbers[0], WinningAndYourNumbers[1]);
    }
}

public record Card(int Number, int[] WinningNumbers, int[] YourNumbers)
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