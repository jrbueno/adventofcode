using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AngleSharp.Text;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver
{
    record class MapRange(long DestinationStart, long SourceStart, long Length)
    {
        public bool IsinRange(long v) => v >= SourceStart && v < SourceStart + Length;
        public long ToDestination(long v) => IsinRange(v) ? (v - SourceStart) + DestinationStart : v;
    };
    record SeedRange(long Start, long Length);
    public object PartOne(string input)
    {
//         var lines = @"
// seeds: 79 14 55 13
//
// seed-to-soil map:
// 50 98 2
// 52 50 48
//
// soil-to-fertilizer map:
// 0 15 37
// 37 52 2
// 39 0 15
//
// fertilizer-to-water map:
// 49 53 8
// 0 11 42
// 42 0 7
// 57 7 4
//
// water-to-light map:
// 88 18 7
// 18 25 70
//
// light-to-temperature map:
// 45 77 23
// 81 45 19
// 68 64 13
//
// temperature-to-humidity map:
// 0 69 1
// 1 0 69
//
// humidity-to-location map:
// 60 56 37
// 56 93 4
// ".Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        var seeds = lines[0].Split(":", StringSplitOptions.TrimEntries)[1]
            .Split(" ", StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToArray();

        var maps = ParseMaps(lines);
        
        var lowestLocation = seeds.Select(seed =>
        {
            long next = seed;
            foreach (var map in maps)
            {
                next = map.Value.Where(r => r.IsinRange(next)).Select(r => r.ToDestination(next)).FirstOrDefault(next);
                Console.WriteLine("Seed: {0} to {1}: {2}", seed, map.Key, next);
            }
        
            return next;
        }).Min();
        
        return lowestLocation;
    }

    private static Dictionary<string, List<MapRange>> ParseMaps(List<string> lines)
    {
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

        // foreach (var map in maps)
        // {
        //     map.Value.ForEach(v =>
        //     {
        //         var (d, s, l) = v;
        //         Console.WriteLine("Map: {0} - ({1}-{2}-{3}", map.Key, d, s, l);
        //     });
        // }
        return maps;
    }

    public object PartTwo(string input) {
        
//         var lines = @"
// seeds: 79 14 55 13
//
// seed-to-soil map:
// 50 98 2
// 52 50 48
//
// soil-to-fertilizer map:
// 0 15 37
// 37 52 2
// 39 0 15
//
// fertilizer-to-water map:
// 49 53 8
// 0 11 42
// 42 0 7
// 57 7 4
//
// water-to-light map:
// 88 18 7
// 18 25 70
//
// light-to-temperature map:
// 45 77 23
// 81 45 19
// 68 64 13
//
// temperature-to-humidity map:
// 0 69 1
// 1 0 69
//
// humidity-to-location map:
// 60 56 37
// 56 93 4
// ".Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        var initialSeeds = lines[0].Split(":", StringSplitOptions.TrimEntries)[1]
            .Split(" ", StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToArray();

        //Get seed Pairs
        List<SeedRange> seedRanges = [];
        foreach (var r in initialSeeds.Chunk(2))
        {
            var start = r[0];
            var len = r[1];
            seedRanges.Add(new SeedRange(start, len));
        };
        
        var maps = ParseMaps(lines);
        // Part two
        foreach (var map in maps)
        {
            var newSeeds = new List<SeedRange>();
            Console.WriteLine("Mapping: {0}", map.Key);
            foreach (var seedRange in seedRanges)
            {
                var remain = seedRange;
                foreach (var mapRange in map.Value.OrderBy(m => m.SourceStart))
                {
                    // if current seed  range is before map start, create a new smaller seed range from range start to map start - length
                    // also update current range length to cover the remaining chunk
                    if (remain.Start < mapRange.SourceStart)
                    {
                        var newlen = Math.Min(remain.Length, mapRange.SourceStart - remain.Start);
                        newSeeds.Add(new SeedRange(remain.Start, newlen));
                        remain = new SeedRange(remain.Start + newlen, remain.Length - newlen);
                    }

                    //break if current range length is covered above
                    if (remain.Length <= 0) break;

                    //if current range is within map start and map end, ToDestination the start of the current seed and grab middle length
                    if (remain.Start >= mapRange.SourceStart && remain.Start < (mapRange.SourceStart + mapRange.Length))
                    {
                        var midLen = Math.Min(remain.Length, mapRange.SourceStart + mapRange.Length - remain.Start);
                        var mapped = new SeedRange(mapRange.ToDestination(remain.Start), midLen);
                        newSeeds.Add(mapped);
                        remain = new SeedRange(remain.Start + midLen, remain.Length - midLen);
                    }
                    //break if current range length is covered above
                    if (remain.Length <= 0) break;
                }
                //if remain is not covered in those use cases, add it to the new set of seed ranges
                if(remain.Length > 0) newSeeds.Add(remain);
            }
            //Update seed ranges based on the set create above. 
            seedRanges = newSeeds;
        }
        
        //The lowest seed start is the answers after all seeds start were ToDestination
        return seedRanges.Select(s => s.Start).Min();
    }
}