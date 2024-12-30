using AdventOfCode2016.Extensions;
using System.Drawing;
using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day02 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Bathroom Security";

        private readonly Dictionary<char, Point> _move = new()
        {
            {'U', new Point(0, -1)},
            {'D', new Point(0, 1)},
            {'L', new Point(-1, 0)},
            {'R', new Point(1, 0)}
        };

        public override string GetSolution1(bool isChallenge)
        {
            Point position = new(1, 1);
            StringBuilder code = new();
            foreach (string sequence in _puzzleInput)
            {
                foreach (char direction in sequence)
                {
                    Point tmpPosition = position.Add(_move[direction]);
                    if (tmpPosition.X >= 0 && tmpPosition.X <= 2 && tmpPosition.Y >= 0 && tmpPosition.Y <= 2)
                    {
                        position = tmpPosition;
                    }
                }
                code.Append(position.Y * 3 + position.X + 1);
            }
            return code.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            Point position = new(0, 2); // Row, Col
            StringBuilder code = new();
            string[,] keypad = new string[5, 5]
            {
                { "#", "#", "1", "#", "#" },
                { "#", "2", "3", "4", "#" },
                { "5", "6", "7", "8", "9" },
                { "#", "A", "B", "C", "#" },
                { "#", "#", "D", "#", "#" }
            };

            foreach (string sequence in _puzzleInput)
            {
                foreach (char direction in sequence)
                {
                    Point tmpPosition = position.Add(_move[direction]);
                    if (tmpPosition.X >= 0 && tmpPosition.X <= 4 && tmpPosition.Y >= 0 && tmpPosition.Y <= 4 && keypad[tmpPosition.Y, tmpPosition.X] != "#")
                    {
                        position = tmpPosition;
                    }
                }
                code.Append(keypad[position.Y, position.X]);
            }
            return code.ToString();
        }
    }
}