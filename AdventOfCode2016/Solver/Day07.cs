namespace AdventOfCode2016.Solver
{
    internal partial class Day07 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Internet Protocol Version 7";

        private readonly List<(List<string> supernets, List<string> hypernets)> _data = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return _data.Count(d => ContainABBA(d.supernets) && !ContainABBA(d.hypernets)).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return _data.Count(d => SupportSSL(d.supernets, d.hypernets)).ToString();
        }

        private static bool SupportSSL(List<string> supernets, List<string> hypernets)
        {
            List<string> aba = supernets.SelectMany(GetABA).Distinct().ToList();
            List<string> invertedBab = hypernets.SelectMany(GetABA).Distinct().Select(s => s[1].ToString() + s[0].ToString() + s[1].ToString()).ToList();
            return aba.Any(a => invertedBab.Contains(a));
        }

        private static List<string> GetABA(string dataToCheck)
        {
            List<string> result = [];
            for (int i = 0; i < dataToCheck.Length - 2; i++)
            {
                if (dataToCheck[i] == dataToCheck[i + 2] && dataToCheck[i] != dataToCheck[i + 1])
                {
                    result.Add(dataToCheck[i..(i + 3)]);
                }
            }
            return result;
        }

        private static bool ContainABBA(List<string> dataToCheck)
        {
            foreach (string s in dataToCheck)
            {
                for (int i = 0; i < s.Length - 3; i++)
                {
                    if (s[i] == s[i + 3] && s[i + 1] == s[i + 2] && s[i] != s[i + 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void ExtractData()
        {
            _data.Clear();
            foreach (string line in _puzzleInput)
            {
                List<string> standard = [];
                List<string> hypernet = [];
                string[] parts = line.Replace(']', '[').Split('[');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        standard.Add(parts[i]);
                    }
                    else
                    {
                        hypernet.Add(parts[i]);
                    }
                }
                _data.Add((standard, hypernet));
            }
        }
    }
}