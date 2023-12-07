using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AngleSharp.Text;

namespace AdventOfCode.Y2023.Day04;

[ProblemName("Scratchcards")]
class Solution : Solver
{
    public object PartOne(string input)
    {
//         var lines =
//             @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
// Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
// Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
// Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
// Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
// Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
//                 .Split("\n");
        var lines = input.Split("\n");
        var total = lines.Select(line =>
        {
            var numbers = line.Split(":")[1]
                .Split("|", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return (
                winning: numbers[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(),
                haveNumbers: numbers[1].Split(" ",
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
        }).Aggregate(0, (agg, t) =>
        {
            var point = 0;
            var (winning, numbers) = t;
            foreach (var number in numbers)
            {
                if (!winning.Contains(number)) continue;
                if (point == 0)
                    point = 1;
                else
                    point <<= 1; // same as p *= 2
            }
            return agg + point;
        });
        // OR alternate method
            // .Select(n => n.winning.Intersect(n.haveNumbers).Count())
            // .Where(x => x > 0)
            // .Select(x => Math.Pow(2, x - 1)).Sum();
        Console.WriteLine(JsonSerializer.Serialize(total));
        return total;
    }

    public object PartTwo(string input)
    {
 //        var lines =
 //            @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
 // Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
 // Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
 // Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
 // Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
 // Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
 //                .Split("\n");
        var lines = input.Split("\n");
        var cardWithMatches = lines.Select(line =>
        {
            var numbers = line.Split(":")[1].Split("|", StringSplitOptions.TrimEntries);
            return (winning: numbers[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
                haveNumbers: numbers[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }).Select((numbers, i) => numbers.winning.Intersect(numbers.haveNumbers).Count()).ToArray();

        var copies = cardWithMatches.Select(_ => 1).ToArray();
        Console.WriteLine(JsonSerializer.Serialize(cardWithMatches.Length));
        Console.WriteLine(JsonSerializer.Serialize(copies.Length));
        for (int i = 0; i < cardWithMatches.Length; i++)
        {
            var c = copies[i];
            for (int j = 0; j < cardWithMatches[i]; j++)
            {
                copies[i + j + 1] += c;
                // Console.WriteLine($"{i} - {JsonSerializer.Serialize(copies)}");
            }
        }
        // Console.WriteLine(JsonSerializer.Serialize(copies));
        // Console.WriteLine(JsonSerializer.Serialize(cardWithMatches));
        return copies.Sum();
    }
}