using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Text;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.VisualBasic;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input)
    {
        
        int total = 0;
        foreach (var line in input.Split("\n"))
        {
            char? first = null;
            char? last = null;
            char? tmp = null;
            // Console.WriteLine(line);
            foreach (var c in line.AsSpan())
            {
                if (c.IsDigit())
                {
                    if (first is null) first = c;
                    tmp = c;
                }
            }
            last = tmp;

            var combine = $"{first}{last}";
            // Console.WriteLine("First:{0} Last:{1} = {0}{1}", first, last);
            total += int.Parse(combine);
        }
        return total;
    }

    public object PartTwo(string input) {
        int total = 0;
        foreach (var line in input.Split("\n"))
        {
            Console.WriteLine(line);
            int first;
            int last;
            string? tmp = null;
            Dictionary<string, int> charNumbers = new Dictionary<string, int>
            {
                { "one", 1},
                { "two", 2},
                { "three", 3},
                { "four", 4},
                { "five", 5},
                { "six", 6},
                { "seven", 7},
                {"eight", 8},
                {"nine", 9}
            };
            var lineNumbers =
                charNumbers.Select(kv =>
                {
                    var fx = line.IndexOf(kv.Key, StringComparison.OrdinalIgnoreCase);
                    var lx = line.LastIndexOf(kv.Key, StringComparison.OrdinalIgnoreCase);
                    return new { Key = kv.Value, FirstIndex = fx, LastIndex = lx };
                }).Where(kv => kv.FirstIndex >= 0).ToList();
            for (int i = 0; i < line.Length; i++)
            {
                if (!line[i].IsDigit()) continue;
                lineNumbers.Add(new { Key = int.Parse(line[i].ToString()), FirstIndex = i, LastIndex = i });
            }

            first = lineNumbers.MinBy(kv => kv.FirstIndex).Key;
            last = lineNumbers.MaxBy(kv => kv.LastIndex).Key;
            var combine = $"{first}{last}";
            Console.WriteLine("First:{0} Last:{1} = {0}{1}", first, last);
            total += int.Parse(combine);
        }
        return total;
    }
}