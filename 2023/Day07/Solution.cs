using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver
{
    private static readonly List<char> _partOnePossibleCards = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'];
    private static readonly List<char> _partTwoPossibleCards = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

    private enum HandType
    {
        FiveOfKind = 1,
        FourOfKind,
        FullHouse,
        ThreeOfKind,
        TwoPair,
        OnePair,
        HighCard
    }

    class Hand(string Cards, int Bid, List<char> cardRanks) : IComparable<Hand>
    {
        public string Cards { get; } = Cards;
        public int Bid { get; } = Bid;

        // public int Rank
        public HandType HandType { get; set; } = HandType.HighCard;

        public HandType CalculatePartTwoHandType(string cards)
        {
            var cg = cards.Where(c => c != 'J').GroupBy(c => c).ToList();
            var jCount = Cards.Count(c => c == 'J');
                
            if (cg.Any(g => g.Count() == (5 - jCount)) || jCount == 5) //Because I calculate groups via link, cg is empty therefore any == false so I have to account for j == 5
                return HandType.FiveOfKind;
            if (cg.Any(c => c.Count() == (4 - jCount)))
                return HandType.FourOfKind;
            if (
                (cg.Any(c => c.Count() == 3) && cg.Any(g => g.Count() == 2))
                || (jCount == 1 && cg.Count(g => g.Count() == 2) == 2)
                || (jCount >= 2 && cg.Any(g => g.Count() == 2))
            )
                return HandType.FullHouse;
            if (cg.Any(c => c.Count() == (3 - jCount)))
                return HandType.ThreeOfKind;
            if (cg.Count(g => g.Count() == 2) == 2 
                || (jCount == 1 && cg.Count(g => g.Count() == 2) >= 1)
                || jCount == 2) 
                return HandType.TwoPair;
            return cg.Any(g => g.Count() == (2 - jCount))
                ? HandType.OnePair 
                : HandType.HighCard;
        }

        public HandType CalculatePartOneHandType(string cards)
        {
            var cg = cards.GroupBy(c => c).ToList();
                
            if (cg.Any(g => g.Count() == 5))
                return HandType.FiveOfKind;
            if (cg.Any(c => c.Count() == 4))
                return HandType.FourOfKind;
            if (cg.Any(c => c.Count() == 3) && cg.Any(g => g.Count() == 2))
                return HandType.FullHouse;
            if (cg.Any(c => c.Count() == 3) &&  cg.Any(g => g.Count() != 2))
                return HandType.ThreeOfKind;
            if (cg.Count(g => g.Count() == 2) == 2)
                return HandType.TwoPair;
            return cg.Count(g => g.Count() == 2) == 1 
                ? HandType.OnePair 
                : HandType.HighCard;
        }

        public int CompareTo(Hand other)
        {
            // Console.WriteLine("Comparing {0} vs {1}", this.HandType, other.HandType);
            if (this.HandType != other.HandType)
                return this.HandType < other.HandType ? 1 : -1;
            // var handCompare = this.HandType.CompareTo(other.HandType);
            // if (handCompare != 0) return handCompare;
            for (int i = 0; i < Cards.Length; i++)
            {
                var c1 = cardRanks.IndexOf(Cards[i]);
                var c2 = cardRanks.IndexOf(other.Cards[i]);
                if (c1 != c2) 
                    return c1 < c2 ? 1 : -1;
                // var cardCompare = cardRanks.IndexOf(Cards[i]).CompareTo(cardRanks.IndexOf(other.Cards[i]));
                // return cardCompare;
            }
            return 0;
        }
    };
    
    public object PartOne(string input)
    {
        // var lines = "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483".Split("\n");
        var lines = input.Split("\n");
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            // Console.WriteLine(line);
            var parts = line.Split(" ");
            var h = new Hand(parts[0], int.Parse(parts[1]), _partOnePossibleCards);
            h.HandType = h.CalculatePartOneHandType(h.Cards);
            hands.Add(h);
            // Console.WriteLine("Cards: {0} - Type: {1}",h.Cards, h.HandType);
            // break;
        }
        hands.Sort();
        // Console.WriteLine(JsonSerializer.Serialize(hands));
        var total = 0;
        foreach (var (h, i) in hands.Select((h, i) => (h, i)))
        {
            // Console.WriteLine("Cards: {0} - Type: {1} Rank: {2}", h.Cards, h.HandType, i + 1);
            total += h.Bid * (i + 1);
        }
        return total;
    }

    public object PartTwo(string input)
    {
        // var lines = "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483".Split("\n");
        var lines = input.Split("\n");
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            // Console.WriteLine(line);
            var parts = line.Split(" ");
            var h = new Hand(parts[0], int.Parse(parts[1]), _partTwoPossibleCards);
            h.HandType = h.CalculatePartTwoHandType(h.Cards);
            hands.Add(h);
            // Console.WriteLine("Cards: {0} - Type: {1}",h.Cards, h.HandType);
            // break;
        }

        hands.Sort();
        // Console.WriteLine(JsonSerializer.Serialize(hands));
        var total = 0;
        foreach (var (h, i) in hands
                     // .Where(h => h.Cards.Contains('J'))
                     .Select((h, i) => (h, i)))
        {
            // Console.WriteLine("Cards: {0} - Type: {1} Rank: {2}", h.Cards, h.HandType, i + 1);
            total += h.Bid * (i + 1);
        }

        return total;
    }
}