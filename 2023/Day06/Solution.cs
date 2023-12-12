using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day06;

[ProblemName("Wait For It")]
class Solution : Solver {
    record TimeDistance(int time, int Record, List<int> PossibleDistanceTraveled)
    {
        public int PossibleNewRecords => PossibleDistanceTraveled.Count(i => i > Record);
    }

    public object PartOne(string input)
    {
        // var lines = "Time:      7  15   30\nDistance:  9  40  200".Split("\n");
        var lines = input.Split("\n");
        var times = lines[0]
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            [1..].Select(int.Parse).ToList();
        var distances = lines[1]
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            [1..].Select(int.Parse).ToList();
        // times.ForEach(Console.WriteLine);
        // distances.ForEach(Console.WriteLine);
        List<TimeDistance> records = [];
        for (int i = 0; i < times.Count; i++)
        {
            List<int> possibleNewRecords = [];
            var currentRecord = distances[i];
            for (int j = 0; j < times[i]; j++)
            {
                var speed = j + 1;
                var distanceTraveled = speed * (times[i] - speed);
                // Console.WriteLine("Speed: {0} - Max Distance: {1}", speed, distanceTraveled);
                possibleNewRecords.Add(distanceTraveled);
            }
            records.Add(new TimeDistance(times[i], currentRecord, possibleNewRecords));
        }
        return records.Aggregate(1, (acc, x) => acc * x.PossibleNewRecords);
    }

    public object PartTwo(string input) {
        // var lines = "Time:      7  15   30\nDistance:  9  40  200".Split("\n");
        var lines = input.Split("\n");
        var time = long.Parse(lines[0][lines[0].IndexOf(" ", StringComparison.Ordinal)..].Replace(" ", ""));
        var record = long.Parse(lines[1][lines[1].IndexOf(" ", StringComparison.Ordinal)..].Replace(" ", ""));
        Console.WriteLine("Time: {0} Record: {1}", time, record);
        long newRecord = 0;
        for (long i = 0; i < time; i++)
        {
            var speed = i + 1;
            var distanceTraveled = speed * (time - speed);
            // Console.WriteLine("Speed: {0} - Max Distance: {1}", speed, distanceTraveled);
            if (distanceTraveled > record) newRecord++;
        }
        return newRecord;
    }
}