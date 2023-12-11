using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using AngleSharp.Text;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver
{
    private class MapRange(long dest, long src, long len)
    {
        public bool IsinRange(long v) => v >= src && v < src + len;
        public long ToDestination(long v) => IsinRange(v) ? (v - src) + dest : v;
    };
    public object PartOne(string input)
    {
        var lines = @"
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
".Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        
        // var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        // var initialSeeds = lines[0].Split(":", StringSplitOptions.TrimEntries)[1]
        //     .Split(" ", StringSplitOptions.TrimEntries)
        //     .Select(long.Parse)
        //     .ToArray();
        var seeds = lines[0].Split(":", StringSplitOptions.TrimEntries)[1]
            .Split(" ", StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToArray();

        // List<long> seeds = [];

        // foreach (var r in initialSeeds.Chunk(2))
        // {
        //     var start = r[0];
        //     var len = r[1];
        //     for (long i = 0; i < len; i++, start++)
        //         seeds.Add(start);
        // };
        Dictionary<string, List<MapRange>> maps = [];
        var currentKey = "";
        for (int i = 1; i < lines.Count; i++)
        {
            var line = lines[i];
            // Console.WriteLine(line);
            if (!line[0].IsDigit())
            {
                currentKey = line.Split(" ")[0];
                // Console.WriteLine(currentKey);
                continue;
            }

            var dsl= line.Split(" ");
            var dest = long.Parse(dsl[0]);
            var src = long.Parse(dsl[1]);
            var len = long.Parse(dsl[2]);
            // Console.WriteLine(currentKey);
            if(!maps.ContainsKey(currentKey))
                maps.Add(currentKey, []);
            maps[currentKey].Add(new MapRange(dest, src, len));
        }
        // lines.ForEach(Console.WriteLine);
        Console.WriteLine("-----");
        // foreach (var map in maps)
        // {
        //     map.Value.ForEach(v =>
        //     {
        //         var (d, s, l) = v;
        //         Console.WriteLine("Map: {0} - ({1}-{2}-{3}", map.Key, d, s, l);
        //     });
        // }
        var lowestLocation = seeds.Select(seed =>
        {
            // var soil = maps["seed-to-soil"]
            //     .Where(m => m.IsinRange(seed))
            //     .Select(m => m.ToDestination(seed))
            //     .FirstOrDefault(seed);
            long next = seed;
            foreach (var map in maps)
            {
                next = map.Value.Where(r => r.IsinRange(next)).Select(r => r.ToDestination(next)).FirstOrDefault(next);
                Console.WriteLine("Seed: {0} to {1}: {2}", seed, map.Key, next);
            }

            // Console.WriteLine("-----");
            return next;
            // Console.WriteLine("Seed: {0} to Soil: {1}",seed,soil);
        }).Min();
        Console.WriteLine(lowestLocation);
        // var lines = input.Split("\n");
        return 0;
    }

    public object PartTwo(string input) {
        return 0;
    }
}
