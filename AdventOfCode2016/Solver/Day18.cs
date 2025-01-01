using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day18 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Like a Rogue";

        public override string GetSolution1(bool isChallenge)
        {
            return ComputeNbrOfSafeTile(_puzzleInput[0], isChallenge ? 40 : 10).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            return ComputeNbrOfSafeTile(_puzzleInput[0], 400000).ToString();
        }

        private static long ComputeNbrOfSafeTile(string initialLine, int nbrOfRows)
        {
            string line = $".{initialLine}.";
            int totalSafe = 0;
            for (int i = 0; i < nbrOfRows; i++)
            {
                totalSafe += line.Count(c => c == '.') - 2; // => -2 because we add safe tile left and right who are in fact wall
                line = GetNextLine(line);
            }
            return totalSafe;
        }

        private static string GetNextLine(string line)
        {
            StringBuilder newLine = new();
            newLine.Append('.');
            for (int i = 1; i < line.Length - 1; i++)
            {
                newLine.Append(SafeOrTrap(line[i - 1], line[i], line[i + 1]));
            }
            newLine.Append('.');
            return newLine.ToString();
        }

        private static string SafeOrTrap(char leftChar, char centerChar, char rightChar)
        {
            bool leftSafe = leftChar == '.';
            bool centerSafe = centerChar == '.';
            bool rightSafe = rightChar == '.';

            bool isTrap = (!leftSafe && !centerSafe && rightSafe) ||
                          (leftSafe && !centerSafe && !rightSafe) ||
                          (!leftSafe && centerSafe && rightSafe) ||
                          (leftSafe && centerSafe && !rightSafe);

            return isTrap ? "^" : ".";
        }
    }
}