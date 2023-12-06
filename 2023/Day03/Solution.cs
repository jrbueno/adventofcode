using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Text;

namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
class Solution : Solver
{
    (int, int)[] directions =
    [
        (-1, 1), (0, 1), (1, 1),
        (-1, 0), (1, 0),
        (1, -1), (0, -1), (-1, -1)
        // (0, 1),
        // (1, 0),
        // (0, -1),
        // (-1, 0),
        // (1, 1),
        // (-1, 1),
        // (1, -1),
        // (-1, -1)
    ];

    public object PartOne(string input)
    {
        var lines = input.Split("\n").ToArray();
        // var lines = "467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..".Split("\n");
        var sum = 0;
        for (var y = 0; y < lines.Length; y++)
        {
            var currentNumber = "";
            var addme = false;
            var isNumber = false;
            var line = lines[y];
            // Console.WriteLine(line);
            for (int x = 0; x < line.Length; x++)
            {
                isNumber = line[x].IsDigit();
                if (!isNumber)
                {
                    if (addme) sum += int.Parse(currentNumber);
                    // Console.WriteLine(currentNumber);
                    currentNumber = "";
                    addme = false;
                }

                if (isNumber)
                {
                    foreach (var (dx, dy) in directions)
                    {
                        var nx = x + dx;
                        var ny = y + dy;
                        if (nx < 0 || nx >= line.Length || ny < 0 || ny >= lines.Length) continue;
                        var c = lines[ny][nx];
                        if (!c.IsDigit() && c != '.')
                            addme = true;
                    }
                }

                if (isNumber) currentNumber += line[x];
            }

            if (isNumber && addme)
                sum += int.Parse(currentNumber);
        }

        return sum;
    }

    public object PartTwo(string input)
    {
        // var lines = "467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..".Split("\n");
        var lines = input.Split("\n").ToArray();
        var sum = 0;
        var neighborsPoint = new Dictionary<(int, int), List<int>>();
        for (var y = 0; y < lines.Length; y++)
        {
            var currentNumber = "";
            var addme = false;
            var isNumber = false;
            var line = lines[y];
            (int, int) currentCoors = (-1, -1);
            // Console.WriteLine(line);
            for (int x = 0; x < line.Length; x++)
            {
                isNumber = line[x].IsDigit();
                if (!isNumber)
                {
                    if (addme && currentCoors.Item1 >= 0)
                    {
                        if(!neighborsPoint.ContainsKey(currentCoors)) neighborsPoint.Add(currentCoors, []);
                        neighborsPoint[currentCoors].Add(int.Parse(currentNumber));
                    }
                    // Console.WriteLine(currentNumber);
                    currentNumber = "";
                    addme = false;
                    currentCoors = (-1, -1);
                }

                if (isNumber)
                {
                    foreach (var (dx, dy) in directions)
                    {
                        var nx = x + dx;
                        var ny = y + dy;
                        if (nx < 0 || nx >= line.Length || ny < 0 || ny >= lines.Length) continue;
                        var c = lines[ny][nx];
                        if (c == '*')
                        {
                            addme = true;
                            currentCoors = (nx, ny);
                        }
                    }
                }

                if (isNumber) currentNumber += line[x];
            }

            if (isNumber && addme)
            {
                if(!neighborsPoint.ContainsKey(currentCoors)) neighborsPoint.Add(currentCoors, []);
                neighborsPoint[currentCoors].Add(int.Parse(currentNumber));
            }
        }

        foreach (var (coords, numbers) in neighborsPoint)
        {
            if (numbers.Count == 2)
            {
                sum += numbers[0] * numbers[1];
            }
        }

        return sum;
    }
}