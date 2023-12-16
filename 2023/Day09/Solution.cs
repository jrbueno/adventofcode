using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;

namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
class Solution : Solver
{

    record HistoryRange(int level, int left, int right, int diff);
    public object PartOne(string input)
    {
        // var lines = "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45"
        // .Split("\n");
        var lines = input.Split("\n");
        var total = 0L;
        foreach (var line in lines)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToArray();
            // total += GetExtrapolatedValue(numbers);
            total += GetRunningTotal(numbers);
            // break;
        }
        int GetRunningTotal(int[] numbers)
        {
            var data = new Dictionary<long, List<int>>();
            var row = 0;
            data.Add(row++, numbers.ToList());
            var allZeroed = false;
            while (!allZeroed)
            {
                // Console.WriteLine(JsonSerializer.Serialize(data));
                allZeroed = true;
                data.Add(row, []);
                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    var left = numbers[i];
                    var right = numbers[i + 1];
                    var diff = right - left;
                    data[row].Add(diff);
                    if (diff != 0) allZeroed = false;
                }

                numbers = data[row].ToArray();
                row++;
            }
            // Console.WriteLine("done with {0}", line);
            // Console.WriteLine(JsonSerializer.Serialize(data));

            return data.Aggregate(0, (acc, kv) =>  acc + kv.Value.Last());
        }
        return total;
    }

    public object PartTwo(string input) {
        var lines = "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45"
        .Split("\n");
        // var lines = input.Split("\n");
        var total = 0L;
        foreach (var line in lines)
        {
            var numbers = line.Split(" ").Select(int.Parse).ToArray();
            // total += GetExtrapolatedValue(numbers);
            total += GetRunningTotal(numbers.Reverse().ToArray());
            // break;
        }
        int GetRunningTotal(int[] numbers)
        {
            var data = new Dictionary<long, List<int>>();
            var row = 0;
            data.Add(row++, numbers.ToList());
            var allZeroed = false;
            while (!allZeroed)
            {
                // Console.WriteLine(JsonSerializer.Serialize(data));
                allZeroed = true;
                data.Add(row, []);
                for (int i = 0; i < numbers.Length - 1; i++)
                {
                    var left = numbers[i];
                    var right = numbers[i + 1];
                    var diff = right - left;
                    data[row].Add(diff);
                    if (diff != 0) allZeroed = false;
                }

                numbers = data[row].ToArray();
                row++;
            }
            // Console.WriteLine("done with {0}", line);
            // Console.WriteLine(JsonSerializer.Serialize(data));

            return data.Aggregate(0, (acc, kv) =>  acc + kv.Value.Last());
        }
        return total;
    }
}