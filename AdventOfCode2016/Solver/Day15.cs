using System.Text.RegularExpressions;

namespace AdventOfCode2016.Solver
{
    internal partial class Day15 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Timing is Everything";

        private readonly List<(int id, int size, int posAtZero)> _discs = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return FindTime(null).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return FindTime(11).ToString();
        }

        private int FindTime(int? newDiscSize)
        {
            // Add disk if needed
            if (newDiscSize != null)
            {
                _discs.Add((_discs.Count + 1, newDiscSize.Value, 0));
            }

            // Serach best time
            for (int time = 0; time < int.MaxValue; time++)
            {
                bool success = true;
                foreach ((int id, int size, int posAtZero) in _discs)
                {
                    if ((posAtZero + time + id) % size != 0)
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    return time;
                }
            }
            throw new InvalidDataException("No solution found");
        }

        private void ExtractData()
        {
            _discs.Clear();
            foreach (string line in _puzzleInput)
            {
                Match match = DiskInfoExtractionRegex().Match(line);
                if (match.Success)
                {
                    int id = int.Parse(match.Groups["id"].Value);
                    int size = int.Parse(match.Groups["nbrPosition"].Value);
                    int posAtZero = int.Parse(match.Groups["position"].Value);
                    _discs.Add((id, size, posAtZero));
                }
            }
        }

        [GeneratedRegex(@"^Disc #(?<id>\d+) has (?<nbrPosition>\d+) positions; at time=0, it is at position (?<position>\d+).$")]
        private static partial Regex DiskInfoExtractionRegex();
    }
}