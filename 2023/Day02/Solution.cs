using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
class Solution : Solver {

    public object PartOne(string input)
    {
        const int maxRed = 12, maxGreen = 13, maxBlue = 14;
        var total = 0;
        
        foreach (var line in input.Split("\n"))
        {
            // Console.WriteLine(line);
            var gameId = line[line.IndexOf(' ')..line.IndexOf(':')];
            var possibleGame = true;
            foreach (var set in line[(line.IndexOf(':')+1)..].Split(";"))
            {
                // Console.WriteLine(set);
                foreach (var colorValue in set.Split(","))
                {
                    // Console.WriteLine(colorValue);
                    var cv = colorValue.Trim().Split(" ");
                    var v = int.Parse(cv[0]);
                    var color = cv[1].ToLower();
                    // Console.WriteLine("{0}={1}", color, v);
                    possibleGame = (color, v) switch
                    {
                        {color: "green", v: > maxGreen} => false,
                        {color: "blue", v: > maxBlue} => false,
                        {color: "red", v: > maxRed} => false,
                        _ => true
                    };
                    if(!possibleGame) break;
                }
                if(!possibleGame) break;
            }
            if (possibleGame) total += int.Parse(gameId);
            // Console.WriteLine("{0} = {1}", gameId, possibleGame);
            // break;
        }
        return total;
    }

    public object PartTwo(string input) {
         var total = 0;
         foreach (var line in input.Split("\n"))
         {
             Console.WriteLine(line);
             Dictionary<string, int> minimums = new()
             {
                 { "green", 0},
                 { "red", 0},
                 { "blue", 0},
             };
             var gameId = line[line.IndexOf(' ')..line.IndexOf(':')];
             foreach (var set in line[(line.IndexOf(':')+1)..].Split(";"))
             {
                 foreach (var colorValue in set.Split(","))
                 {
                     Console.WriteLine(colorValue);
                     var cv = colorValue.Trim().Split(" ");
                     var v = int.Parse(cv[0]);
                     var color = cv[1].ToLower();
                     // Console.WriteLine("{0}={1}", color, v);
                     minimums[color] = minimums[color] == 0 || minimums[color] < v ? v : minimums[color];
                 }
             }
             // Console.WriteLine("{0} = Green {1} Red {2} Blue {3} = {4}", gameId, minimums["green"], minimums["red"], minimums["blue"], minimums["green"] *  minimums["blue"] * minimums["red"]);
             total += minimums["green"] * minimums["blue"] * minimums["red"];
             // break;
         }
         return total;
    }
}