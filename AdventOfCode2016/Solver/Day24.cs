using AdventOfCode2016.Extensions;
using AdventOfCode2016.Tools;
using System.Drawing;
using static AdventOfCode2016.Tools.QuickMaze;

namespace AdventOfCode2016.Solver
{
    internal partial class Day24 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Air Duct Spelunking";

        private QuickMatrix _ductsNetwork = new();

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return GetShortestDistance(false).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return GetShortestDistance(true).ToString();
        }

        private long GetShortestDistance(bool returnToOrigin)
        {
            // Get points to visit
            Dictionary<string, Point> locToVisit = _ductsNetwork.Cells.FindAll(c => c.StringVal.IsNumeric())
                .OrderBy(c => c.StringVal)
                .ToDictionary(c => c.StringVal, c => c.Position);

            // Extract all pair of distance
            List<string> keys = new(locToVisit.Keys);
            List<(string from, string to, long distance)> _allPair = [];
            for (int i = 0; i < locToVisit.Count; i++)
            {
                Point from = locToVisit[keys[i]];
                QuickMaze.SolveMaze(_ductsNetwork, from, from, "#");
                for (int j = i + 1; j < locToVisit.Count; j++)
                {
                    Point to = locToVisit[keys[j]];
                    _allPair.Add((i.ToString(), j.ToString(), ((MazeCellInfos)_ductsNetwork.Cell(to.X, to.Y).ObjectVal!).DistanceToStart));
                }
            }

            // Get shortest path
            QuickDijkstra qd = new(_allPair);
            return qd.GetShortestPathVisitingAllNodes("0", returnToOrigin);
        }

        private void ExtractData()
        {
            _ductsNetwork = new QuickMatrix(_puzzleInput);
        }
    }
}