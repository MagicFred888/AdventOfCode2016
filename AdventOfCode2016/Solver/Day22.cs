using AdventOfCode2016.Tools;
using System.Drawing;
using static AdventOfCode2016.Tools.QuickMaze;

namespace AdventOfCode2016.Solver
{
    internal partial class Day22 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Grid Computing";

        private readonly List<(Point position, int size, int used, int avail)> _clusterGrid = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return CountValidPair().ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            // Create a grid with all nodes
            int minX = _clusterGrid.Min(n => n.position.X);
            int maxX = _clusterGrid.Max(n => n.position.X);
            int minY = _clusterGrid.Min(n => n.position.Y);
            int maxY = _clusterGrid.Max(n => n.position.Y);
            QuickMatrix grid = new(maxX - minX + 1, maxY - minY + 1, ".");

            // Get position of empty node
            (Point position, int size, _, _) = _clusterGrid.Find(n => n.used == 0);

            // Based on empty node, we define which data are unmoveable and empty space position
            int maxToBeMoveable = size;
            foreach (var info in _clusterGrid.FindAll(c => c.used == 0 || c.used > maxToBeMoveable))
            {
                grid.Cell(info.position).StringVal = info.used == 0 ? "_" : "#";
            }

            // First we find shortes way through the maze to cell left to target
            _ = QuickMaze.SolveMaze(grid, position, new Point(grid.ColCount - 2, 0), "#");

            // Now we have all to calculate the answer...

            // Move to cell left to target
            long distance = ((MazeCellInfos)grid.Cell(new Point(grid.ColCount - 2, 0)).ObjectVal!).DistanceToStart;

            // Move data one block left
            distance++;

            // Every further move require 5 move
            distance += (grid.ColCount - 2) * 5;

            // Done
            return distance.ToString();
        }

        private long CountValidPair()
        {
            long validPairs = 0;
            for (int i = 0; i < _clusterGrid.Count; i++)
            {
                for (int j = 0; j < _clusterGrid.Count; j++)
                {
                    if (i == j) continue;
                    if (_clusterGrid[i].used == 0) continue;
                    if (_clusterGrid[i].used <= _clusterGrid[j].avail) validPairs++;
                }
            }
            return validPairs;
        }

        private void ExtractData()
        {
            _clusterGrid.Clear();
            foreach (string line in _puzzleInput.FindAll(s => s.StartsWith("/dev/grid")))
            {
                string[] parts = line.Replace("T", " ").Replace("%", " ").Replace("-", " ").Replace("y", " ").Replace("x", " ")
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

                Point position = new(int.Parse(parts[1]), int.Parse(parts[2]));
                int size = int.Parse(parts[3]);
                int used = int.Parse(parts[4]);
                int avail = int.Parse(parts[5]);
                _clusterGrid.Add((position, size, used, avail));
            }
        }
    }
}