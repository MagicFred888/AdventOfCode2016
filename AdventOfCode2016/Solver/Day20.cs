namespace AdventOfCode2016.Solver
{
    internal partial class Day20 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Firewall Rules";

        List<(uint start, uint stop)> _blacklistedRanges = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            List<(uint start, uint stop)> cleanRanges = CleanRanges(_blacklistedRanges);
            return (cleanRanges[0].stop + 1).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            List<(uint start, uint stop)> cleanRanges = CleanRanges(_blacklistedRanges);
            return (cleanRanges.Count - 1).ToString();
        }

        private static List<(uint start, uint stop)> CleanRanges(List<(uint start, uint stop)> blacklistedRanges)
        {
            // Group the ranges who are overlapping
            bool restart = false;
            do
            {
                restart = false;
                for (int i = 0; i < blacklistedRanges.Count; i++)
                {
                    (uint start, uint stop) = blacklistedRanges[i];
                    for (int j = i + 1; j < blacklistedRanges.Count; j++)
                    {
                        (uint start, uint stop) rangeToTest = blacklistedRanges[j];

                        // Test if baseRange intersect with testRange
                        if ((rangeToTest.stop >= start && rangeToTest.stop <= stop)
                            || (rangeToTest.start >= start && rangeToTest.start <= stop)
                            || (start > rangeToTest.stop && start - rangeToTest.stop == 1)
                            || (rangeToTest.start > stop && rangeToTest.start - stop == 1))
                        {
                            // Merge the ranges
                            (uint start, uint stop) mergedRange = (Math.Min(start, rangeToTest.start), Math.Max(stop, rangeToTest.stop));

                            // Report the change and break to start over
                            blacklistedRanges.RemoveAt(j);
                            blacklistedRanges.RemoveAt(i);
                            blacklistedRanges.Add(mergedRange);
                            blacklistedRanges = [.. blacklistedRanges.OrderBy(v => v.start)];
                            restart = true;
                            break;
                        }
                    }
                    if (restart) break;
                }
            } while (restart);
            return [.. blacklistedRanges.OrderBy(r => r.start)];
        }

        private void ExtractData()
        {
            _blacklistedRanges = [];
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split('-');
                _blacklistedRanges.Add((uint.Parse(parts[0]), uint.Parse(parts[1])));
            }
        }
    }
}