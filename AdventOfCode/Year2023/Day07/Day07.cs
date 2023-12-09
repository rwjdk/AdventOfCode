using System.Diagnostics;
using Utils;

namespace Year2023.Day07;

//https://adventofcode.com/2023/day/7
public class Day07
{
    private const string Day = "Day07";

    //What are the total winnings?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 6440)]
    [InlineData($"{Day}\\Input.txt", 250946742)]
    public void Part1(string inputFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        Hand[] hands = inputFile.ToLines().ToDay07Hands(false);

        var orderedHands = hands.OrderBy(x => (int)x.Strength).ThenBy(x => x.SecondaryStrength).ToList();
        int rank = 1;
        foreach (Hand orderedHand in orderedHands)
        {
            calculatedAnswer += orderedHand.Bid * rank;
            rank++;
        }

        Assert.Equal(expectedAnswer, calculatedAnswer);
    }

    //What are the new total winnings?
    [Theory]
    [InlineData($"{Day}\\Sample.txt", 5905)]
    [InlineData($"{Day}\\Input.txt", 251824095)]
    public void Part2(string inputFile, int expectedAnswer)
    {
        int calculatedAnswer = 0;
        Hand[] hands = inputFile.ToLines().ToDay07Hands(true);

        var orderedHands = hands.OrderBy(x => (int)x.Strength).ThenBy(x => x.SecondaryStrength).ToList();
        int rank = 1;
        foreach (Hand orderedHand in orderedHands)
        {
            calculatedAnswer += orderedHand.Bid * rank;
            rank++;
        }

        Assert.Equal(expectedAnswer, calculatedAnswer);
    }
}

public static class Day07Extensions
{
    public static Hand[] ToDay07Hands(this string[] inputLines, bool jokerRule)
    {
        var result = new List<Hand>();
        foreach (var inputLine in inputLines)
        {
            var parts = inputLine.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var handAsString = parts[0];
            var bid = Convert.ToInt32(parts[1]);
            result.Add(new Hand(handAsString, bid, jokerRule));
        }

        return result.ToArray();
    }
}

[DebuggerDisplay("{HandAsString} = {Strength} - Bid: {Bid}")]
public class Hand(string handAsString, int bid, bool jokerRule)
{
    public HandStrength Strength = jokerRule ? CalculateJokerRuleStrength(handAsString) : CalculateNormalStrength(handAsString);
    public readonly string SecondaryStrength = CalculateSecondaryStrength(handAsString, jokerRule);

    private static string CalculateSecondaryStrength(string handAsString, bool jokerRule)
    {
        //Here we use a trick to make the hand sortable as a string by choosing higher ascii values for the higher cards
        return handAsString
            .Replace("A", "E")
            .Replace("K", "D")
            .Replace("Q", "C")
            .Replace("J", jokerRule ? "0" : "B")
            .Replace("T", "A");
    }

    private static HandStrength CalculateNormalStrength(string handAsString)
    {
        var typesOfCards = handAsString.GroupBy(x => x).ToList();
        var anyThreeOfAKind = typesOfCards.Any(x => x.Count() is 3);
        var anyPair = typesOfCards.Any(x => x.Count() is 2);
        switch (typesOfCards.Count)
        {
            case 1:
                return HandStrength.FiveOfAKind;
            case 2:
                return anyPair ? HandStrength.FullHouse : HandStrength.FourOfAKind;
            case 3:
                return anyThreeOfAKind ? HandStrength.ThreeOfAKind : HandStrength.TwoPair;
            case 4:
                return HandStrength.OnePair;
            case 5:
                return HandStrength.HighCard;
        }
        throw new ArgumentException("Invalid Input", nameof(handAsString));
    }

    private static HandStrength CalculateJokerRuleStrength(string handAsString)
    {
        //Jokers are wildcards
        var numberOfJokers = 5 - handAsString.Replace("J", "").Length;
        bool anyJokers = numberOfJokers > 0;
        bool singleJoker = numberOfJokers == 1;
        bool moreThanOneJoker = numberOfJokers > 1;

        var typesOfCards = handAsString.GroupBy(x => x).ToList();
        var anyThreeOfAKind = typesOfCards.Any(x => x.Count() is 3);
        var anyPair = typesOfCards.Any(x => x.Count() is 2);

        switch (typesOfCards.Count)
        {
            case 1:
                return HandStrength.FiveOfAKind;
            case 2:
                return anyJokers ? HandStrength.FiveOfAKind : anyPair ? HandStrength.FullHouse : HandStrength.FourOfAKind;
            case 3:
                if (moreThanOneJoker)
                {
                    return HandStrength.FourOfAKind;
                }
                if (singleJoker && anyThreeOfAKind)
                {
                    return HandStrength.FourOfAKind;
                }
                if (singleJoker)
                {
                    return HandStrength.FullHouse;
                }
                return anyThreeOfAKind ? HandStrength.ThreeOfAKind : HandStrength.TwoPair;
            case 4:
                return anyJokers ? HandStrength.ThreeOfAKind : HandStrength.OnePair;
            case 5:
                return anyJokers ? HandStrength.OnePair : HandStrength.HighCard;
        }

        throw new ArgumentException("Invalid Input", nameof(handAsString));
    }

    public string HandAsString { get; init; } = handAsString;
    public int Bid { get; init; } = bid;
}

public enum HandStrength
{
    FiveOfAKind = 7,
    FourOfAKind = 6,
    FullHouse = 5,
    ThreeOfAKind = 4,
    TwoPair = 3,
    OnePair = 2,
    HighCard = 1,
}