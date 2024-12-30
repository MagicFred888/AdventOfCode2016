using AdventOfCode2016.Tools;
using System.Text;
using static AdventOfCode2016.Tools.QuickMatrix;

namespace AdventOfCode2016.Solver
{
    internal partial class Day06 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Signals and Noise";

        public override string GetSolution1(bool isChallenge)
        {
            StringBuilder result = new();
            QuickMatrix data = new(_puzzleInput);
            foreach (List<CellInfo> list in data.Cols)
            {
                result.Append(FindLetter(list, false));
            }
            return result.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            StringBuilder result = new();
            QuickMatrix data = new(_puzzleInput);
            foreach (List<CellInfo> list in data.Cols)
            {
                result.Append(FindLetter(list, true));
            }
            return result.ToString();
        }

        private static string FindLetter(List<CellInfo> list, bool minValue)
        {
            // Find the letter that appears the most or the least
            Dictionary<string, int> countDic = list.Aggregate(new Dictionary<string, int>(), (acc, value) =>
            {
                acc.TryGetValue(value.StringVal, out int count);
                acc[value.StringVal] = count + 1;
                return acc;
            });
            int target = minValue ? countDic.Values.Min() : countDic.Values.Max();
            return countDic.First(x => x.Value == target).Key;
        }
    }
}