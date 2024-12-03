using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
class Solution : Solver {

    public object PartOne(string input)
    {
        var data =
            "...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....";
        var cosmos = data.Split("\n");
        var emptyRows = cosmos
            .Select((c, i) => new { Index = i, HasGalaxy = c.Contains('#') })
            .Where(c => !c.HasGalaxy);
        // var expanded = cosmos;
        // for (int y = 0; y < cosmos.Length; y++)
        // {
        //     var emptyRow = cosmos[y].Contains('#');
        //     for (int x = 0; x < cosmos[y].Length; x++)
        //     {
        //         var emptyCol =
        //             cosmos.Select((c, i) => new { Index = i, EmptyGalaxy = c[x] == '#' })
        //                 .Any(l => !l.EmptyGalaxy);
        //     }
        // }
        var emptyCols = cosmos[0]
            .Select((c, i) => new { Index = i, EmptyGalaxy = cosmos.All(l => l[i] != '#') })
            .Where(x => x.EmptyGalaxy);
        
        Console.WriteLine(emptyCols.Count());
        return 0;
    }

    public object PartTwo(string input) {
        return 0;
    }
}
