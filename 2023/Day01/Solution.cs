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
            last = tmp ?? first;

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
            int offset = 0;
            string? first = null;
            string? last;
            string? tmp = null;
            for (int i = 0; i < line.Length; i++)
            {
                Console.WriteLine(line[offset..i]);
                tmp = line[offset..i] switch
                {
                    "one" => "1",
                    "two" => "2",
                    "three" => "3",
                    "four" => "4",
                    "five" => "5",
                    "six" => "6",
                    "seven" => "7",
                    "eight" => "8",
                    "nine" => "9",
                    _ => null
                };
                if (tmp is null && !line[i].IsDigit()) continue;
                first ??= tmp;
                offset = i + 1;
            }
            last = tmp ?? first;
            var combine = $"{first}{last}";
            Console.WriteLine("First:{0} Last:{1} = {0}{1}", first, last);
            // total += int.Parse(combine);
            break;
        }
        return total;
    }
}