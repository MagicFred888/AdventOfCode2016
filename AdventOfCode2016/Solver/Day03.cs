using AdventOfCode2016.Tools;

namespace AdventOfCode2016.Solver
{
    internal partial class Day03 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Squares With Three Sides";

        private List<(int a, int b, int c)> _sides = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return _sides.Count(t => t.a + t.b > t.c && t.b + t.c > t.a && t.c + t.a > t.b).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            // Permute data by group of 3
            List<(int a, int b, int c)> newSides = [];
            for (int i = 0; i < _sides.Count - 2; i += 3)
            {
                newSides.Add((_sides[i].a, _sides[i + 1].a, _sides[i + 2].a));
                newSides.Add((_sides[i].b, _sides[i + 1].b, _sides[i + 2].b));
                newSides.Add((_sides[i].c, _sides[i + 1].c, _sides[i + 2].c));
            }
            _sides = newSides;

            return _sides.Count(t => t.a + t.b > t.c && t.b + t.c > t.a && t.c + t.a > t.b).ToString();  //1635 too high
        }

        private void ExtractData()
        {
            _sides = QuickList.ListOfListInt(_puzzleInput, " ", true).ConvertAll(i => (i[0], i[1], i[2]));
        }
    }
}