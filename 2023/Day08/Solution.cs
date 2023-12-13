using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver
{

    record NetworkNode(string value, string left, string right);
    public object PartOne(string input)
    {
        // var lines = "LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)".Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var lines = input.Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var instructions = lines[0];
        const string endNodeValue = "ZZZ";
        Console.WriteLine(instructions);
        var nodeMaps = lines[1..].Select(line =>
        {
            var parts = line.Split("=", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var value = parts[0];
            var leafs = parts[1].Trim('(',')').Split(",", StringSplitOptions.TrimEntries);
            return new NetworkNode(value, leafs[0], leafs[1]);
        }).ToList();
        int totalSteps = 0;
        NetworkNode curr = nodeMaps.First(n => n.value == "AAA");
        var i = 0;
        while (curr.value != endNodeValue)
        {
            var instruction = instructions[i];
            i++;
            totalSteps++;
            i %= instructions.Length;
            // Console.WriteLine("{0} - {1}", instruction, curr.value );
            curr = instruction switch
            {
                'L' => nodeMaps.First(n => n.value == curr.left),
                'R' => nodeMaps.First(n => n.value == curr.right),
                _ => throw new Exception($"Invalid instruction {instruction}")
            };
            // if (curr.value == endNodeValue) break;
            // Console.WriteLine("{0} - {1}", instruction, curr.value );
        }
        return totalSteps;
    }

    // private int WalkMap(string instructions, string curr, string endValue, int stepsTaken, List<NetworkNode> nodeMaps)
    // {
    //     if (curr == endValue) return ++stepsTaken;
    //
    //     var nextNode = nodeMaps.First(n => n.value == curr);
    //     
    // }

    public object PartTwo(string input) {
        return 0;
    }
}
