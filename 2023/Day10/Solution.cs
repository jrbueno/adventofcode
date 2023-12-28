using System;
using System.Linq;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
class Solution : Solver {
    public record struct Point(int X, int Y)
    {
        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
        public static Point operator -(Point a) => new Point(-a.X, -a.Y);
        public static implicit operator Point((int X, int Y) tuple) => new Point(tuple.X, tuple.Y);
    }
    public object PartOne(string input)
    {
        // var maze = input.Split("\n");
        // var data = "..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...";
        // var data = ".....\n.S-7.\n.|.|.\n.L-J.\n....."; 
        var data = input;
        var maze = data.Split("\n");
        var startingPosition = maze
            .Select((l, i) => new { Line = l, Index = i, StartIndex = l.IndexOf('S') })
            .First(l => l.StartIndex >= 0);
        var startingPoint = new Point(startingPosition.StartIndex, startingPosition.Index);
        
        var seen = new bool[maze[0].Length, maze.Length];
        var steps = Walk(maze, startingPoint, seen);
        return steps / 2;
    }

    // private bool[,] _seen;
    private readonly Point[] directions =
    [
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0)
    ];
    private int Walk(string[] maze, Point curr, bool[,] seen)
    {
        if(curr.X < 0 || curr.X >= maze[0].Length || curr.Y < 0 || curr.Y >= maze.Length) return 0;
        var c = maze[curr.Y][curr.X];
        if (seen[curr.X, curr.Y]) return 0;
        
        seen[curr.X, curr.Y] = true;
        var steps = 1;
        foreach (var direction in directions)
        {
            var np = curr + direction;
            
            if(np.X < 0 || np.X >= maze[0].Length || np.Y < 0 || np.Y >= maze.Length) continue;
            
            var nc = maze[np.Y][np.X];
            
            if (nc == '.' || (nc != 'S' && seen[np.X, np.Y])) continue;
            
            var cPositions = PipePositions(c);
            var newPositions = PipePositions(nc);
            
            //current pipe must go the same direction and the next pipe must go the opposite for recursion call.
            if (cPositions.Contains(direction) && newPositions.Contains(-direction)) 
                steps += Walk(maze, np, seen);
        }
        return steps;
    }

    private static Point[] PipePositions(char nc)
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
        
        return  nc switch
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
    }

    public object PartTwo(string input)
    {
        // var data = "...........\n.S-------7.\n.|F-----7|.\n.||.....||.\n.||.....||.\n.|L-7.F-J|.\n.|..|.|..|.\n.L--J.L--J.\n...........";
        // var data = ".F----7F7F7F7F-7....\n.|F--7||||||||FJ....\n.||.FJ||||||||L7....\nFJL7L7LJLJ||LJ.L-7..\nL--J.L7...LJS7F-7L7.\n....F-J..F7FJ|L7L7L7\n....L7.F7||L7|.L7L7|\n.....|FJLJ|FJ|F7|.LJ\n....FJL-7.||.||||...\n....L---J.LJ.LJLJ...";
        var data = input;
        var maze = data.Split("\n");
        var startingPosition = maze
            .Select((l, i) => new { Line = l, Index = i, StartIndex = l.IndexOf('S') })
            .First(l => l.StartIndex >= 0);
        var startingPoint = new Point(startingPosition.StartIndex, startingPosition.Index);
        
        var seen = new bool[maze[0].Length, maze.Length];
        var steps = Walk(maze, startingPoint, seen);
        var insidePipes = CountInsidePipes(maze, seen);
        return insidePipes;
    }

    private int CountInsidePipes(string[] maze, bool[,] seen)
    {
        //X,Y
        //Left:     -1 0
        //Right:    1 0
        //Up:       0 -1
        //Down      0 1
        int insideCount = 0;
        for (int y = 0; y < maze.Length; y++)
        {
            for (int x = 0; x < maze[y].Length; x++)
            {
                if (IsInsideLoop(y, x, maze, seen))
                {
                    // Console.Write('I');
                    insideCount++;
                }
                // else
                    // Console.Write(maze[y][x]);
            }
            // Console.WriteLine();
        }

        return insideCount;
    }

    private bool IsInsideLoop(int y, int x, string[] maze, bool[,] seen)
    {
        if (seen[x, y]) return false;
        var up = (0, -1);
        var edgeCounter = 0;
        for (var i = x - 1; i >= 0; i--)
        {
            if (maze[y][i] != 'S' && seen[i, y] && PipePositions(maze[y][i]).Contains(up))
                edgeCounter++;
        }
        return edgeCounter % 2 != 0;
    }
}