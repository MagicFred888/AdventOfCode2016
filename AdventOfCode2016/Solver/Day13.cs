using AdventOfCode2016.Tools;
using System.Drawing;

namespace AdventOfCode2016.Solver
{
    internal partial class Day13 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "A Maze of Twisty Little Cubicles";

        private QuickMatrix _maze = new();

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            Point start = new(1, 1);
            Point destination = isChallenge ? new(31, 39) : new(7, 4);
            if (!QuickMaze.SolveMaze(_maze, start, destination, "#"))
            {
                throw new InvalidDataException();
            }
            return ((QuickMaze.MazeCellInfos)_maze.Cell(start).ObjectVal!).BestDistance.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            Point start = new(1, 1);
            Point destination = isChallenge ? new(31, 39) : new(7, 4);
            if (!QuickMaze.SolveMaze(_maze, start, destination, "#"))
            {
                throw new InvalidDataException();
            }
            return _maze.Cells.Count(c => ((QuickMaze.MazeCellInfos)c.ObjectVal!).DistanceToStart <= 50 && ((QuickMaze.MazeCellInfos)c.ObjectVal!).DistanceToStart >= 0).ToString();
        }

        private void ExtractData()
        {
            // Create maze with outer wall at #
            int designerFavoriteNumber = int.Parse(_puzzleInput[0]);
            _maze = new(50, 50, "."); // 50x50 as a guess

            // Fill the maze
            for (int x = 0; x < _maze.ColCount - 1; x++)
            {
                for (int y = 0; y < _maze.RowCount - 1; y++)
                {
                    int value = x * x + 3 * x + 2 * x * y + y + y * y + designerFavoriteNumber;
                    string binary = Convert.ToString(value, 2);
                    int count = binary.Count(c => c == '1');
                    _maze.Cell(x, y).StringVal = count % 2 == 1 ? "#" : ".";
                }
            }
        }
    }
}