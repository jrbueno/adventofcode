using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using AngleSharp.Text;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver
{
    private static readonly List<char> _individualCards = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

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

    class Hand(string Cards, int Bid) : IComparable<Hand>
    {
        public string Cards { get; } = Cards;
        public int Bid { get; } = Bid;
        // public int Rank
        public HandType HandType => CalculateHandType(Cards);

        private HandType CalculateHandType(string cards)
        {
            var cg = cards.GroupBy(c => c).ToList();
            var jCount = cg.FirstOrDefault(g => g.Key == 'J')?.Count() ?? 0;
                
            if (cg.Any(g => g.Count() == (5 - jCount)))
                return HandType.FiveOfKind;
            if (cg.Any(c => c.Count() == 4 - jCount))
                return HandType.FourOfKind;
            if ((cg.Any(c => c.Count() == 3) || cg.Any(g => g.Count() == 3 - jCount)) 
                && (cg.Any(g => g.Count() == 2 - jCount || cg.Any(g => g.Count() == 2))))
                return HandType.FullHouse;
            if ((cg.Any(c => c.Count() == 3 - jCount) || cg.Any(c => c.Count() == 3)) && cg.Any(g => g.Count() != 2))
                return HandType.ThreeOfKind;
            if (cg.Count(g => g.Count() == 2 || g.Count() == 2 - jCount) == 2)
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
            if (this.HandType == other.HandType)
            {
                for (int i = 0; i < Cards.Length; i++)
                {
                    var c1 = _individualCards.IndexOf(Cards[i]);
                    var c2 = _individualCards.IndexOf(other.Cards[i]);
                    if (c1 == c2) continue;
                    return c1 < c2 ? 1 : -1;
                }
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
            var h = new Hand(parts[0], int.Parse(parts[1]));
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

    public object PartTwo(string input) {
        return 0;
    }
}