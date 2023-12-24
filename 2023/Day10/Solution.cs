using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
class Solution : Solver {
    public record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static implicit operator Point((int X, int Y) tuple) => new Point(tuple.X, tuple.Y);
    }
    public object PartOne(string input)
    {
        // var maze = input.Split("\n");
        // var maze = "..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...".Split("\n");
        var data = ".....\n.S-7.\n.|.|.\n.L-J.\n....."; 
        var maze = data.Split("\n");
        var startingPosition = maze
            .Select((l, i) => new { Line = l, Index = i, StartIndex = l.IndexOf('S') })
            .First(l => l.StartIndex >= 0);
        var startingPoint = new Point(startingPosition.StartIndex, startingPosition.Index);
        // Console.WriteLine(startingPosition);
        Console.WriteLine(data);
        Console.WriteLine("Starting Point {0}", startingPoint);
        var seen = new bool[maze[0].Length, maze.Length];
        var steps = Walk(maze, startingPoint, seen, 0);
        return steps;
    }


    private readonly Point[] directions =
    [
        new (-1, 1), new (0, 1), new (1, 1),
        new (-1, 0), new (1, 0),
        new (1, -1), new (0, -1), new (-1, -1)
    ];
    private int Walk(string[] maze, Point curr, bool[,] seen, int steps)
    {
        if(curr.X < 0 || curr.X >= maze[0].Length || curr.Y < 0 || curr.Y >= maze.Length) return 0;
        Console.WriteLine("Current: {0}" ,maze[curr.Y][curr.X]);
        var c = maze[curr.Y][curr.X];
        // if(c == 'S' && seen[curr.X, curr.Y]) return 0;
        if (seen[curr.X, curr.Y]) return 0;
        
        seen[curr.X, curr.Y] = true;
        steps++;
        foreach (var direction in directions)
        {
            var np = curr + direction;
            if(np.X < 0 || np.X >= maze[0].Length || np.Y < 0 || np.Y >= maze.Length) continue;
            var nc = maze[np.Y][np.X]; 
            Console.WriteLine("Next: {0}", nc);
            // if(nc == '.' || (nc != 'S' && seen[np.X, np.Y] )) continue;
            var newPositions = NextPipePositions(nc);
            // Console.WriteLine(JsonSerializer.Serialize(newPositions));
            steps += newPositions.Aggregate(steps, (acc, p) => Walk(maze, curr + p, seen, acc));
        }
        return steps;
    }

    private static Point[] NextPipePositions(char nc)
    {
        //The pipes are arranged in a two-dimensional grid of tiles:
        // | is a vertical pipe connecting north and south.
        // - is a horizontal pipe connecting east and west.
        // L is a 90-degree bend connecting north and east.
        // J is a 90-degree bend connecting north and west.
        // 7 is a 90-degree bend connecting south and west.
        // F is a 90-degree bend connecting south and east.
        // . is ground; there is no pipe in this tile.
        // S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.          
        var newPosition = nc switch
        {
            '|' => [(0, -1), (0, 1)],
            '-' => [(1, 0), (-1, 0)],
            'L' => [(0, -1), (1, 0)],
            'J' => [(0, -1), (-1, 0)],
            '7' => [(0, 1), (-1, 0)],
            'F' => [(0, 1), (1, 0)],
            'S' => [(0, 1), (0, -1), (-1, 0), (1, 0)],
            _ => Array.Empty<Point>()
        };
        return newPosition;
    }

    public object PartTwo(string input) {
        return 0;
    }
}