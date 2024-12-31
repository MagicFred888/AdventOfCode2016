using System.Text.RegularExpressions;

namespace AdventOfCode2016.Solver
{
    internal partial class Day09 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Explosives in Cyberspace";

        public override string GetSolution1(bool isChallenge)
        {
            long total = 0;
            foreach (string line in _puzzleInput)
            {
                total += LengthOfDecompressedString(line, false);
            }
            return total.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            long total = 0;
            foreach (string line in _puzzleInput)
            {
                total += LengthOfDecompressedString(line, true);
            }
            return total.ToString();
        }

        private static long LengthOfDecompressedString(string line, bool multiLevel)
        {
            long result = 0;
            while (CompressionInfoPattern().IsMatch(line))
            {
                Match match = CompressionInfoPattern().Match(line);
                int length = int.Parse(match.Groups["firstDigit"].Value);
                int repeat = int.Parse(match.Groups["secondDigit"].Value);
                int start = match.Index + match.Length;
                int end = start + length;

                // Add length before the match
                result += match.Index;

                // Add length of decompressed data
                string data = line.Substring(start, length);
                if (multiLevel)
                {
                    result += LengthOfDecompressedString(data, multiLevel) * repeat;
                }
                else
                {
                    result += length * repeat;
                }
                line = line[end..];
            }

            // Add end of the string
            result += line.Length;
            return result;
        }

        [GeneratedRegex(@"\((?<firstDigit>\d+)x(?<secondDigit>\d+)\)")]
        private static partial Regex CompressionInfoPattern();
    }
}