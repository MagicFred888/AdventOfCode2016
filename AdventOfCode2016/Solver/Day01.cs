using AdventOfCode2016.Extensions;
using System.Drawing;

namespace AdventOfCode2016.Solver
{
    internal partial class Day01 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "No Time for a Taxicab";

        private readonly List<(string turn, int distance)> _allTurns = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            Point position = new();
            Point direction = new(0, -1);
            foreach ((string turn, int distance) turn in _allTurns)
            {
                direction = turn.turn == "L" ? direction.RotateCounterclockwise() : direction.RotateClockwise();
                position = position.Add(direction.Multiply(turn.distance));
            }
            return position.ManhattanDistance().ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            Point position = new();
            Point direction = new(0, -1);
            List<Point> visited = [];
            foreach ((string turn, int distance) turn in _allTurns)
            {
                direction = turn.turn == "L" ? direction.RotateCounterclockwise() : direction.RotateClockwise();
                for (int i = 0; i < turn.distance; i++)
                {
                    position = position.Add(direction);
                    if (visited.Contains(position))
                    {
                        return position.ManhattanDistance().ToString();
                    }
                    visited.Add(position);
                }
            }
            throw new InvalidDataException();
        }

        private void ExtractData()
        {
            _allTurns.Clear();
            foreach (string turn in _puzzleInput[0].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(t => t.Trim()))
            {
                _allTurns.Add((turn[..1], int.Parse(turn[1..])));
            }
        }
    }
}