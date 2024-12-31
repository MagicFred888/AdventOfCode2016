using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day16 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Dragon Checksum";

        public override string GetSolution1(bool isChallenge)
        {
            return ComputeDragonHash(isChallenge ? 272 : 20);
        }

        public override string GetSolution2(bool isChallenge)
        {
            return ComputeDragonHash(35651584);
        }

        private string ComputeDragonHash(int targetSize)
        {
            // Create random data
            StringBuilder randomData = new(_puzzleInput[0]);
            while (randomData.Length < targetSize)
            {
                StringBuilder newData = new(randomData.ToString());
                newData.Append('0');
                newData.Append(randomData.ToString().Reverse().Select(c => c == '0' ? '1' : '0').ToArray());
                randomData = newData;
            }

            // Compute checksum
            string hash = randomData.ToString()[..targetSize];
            do
            {
                StringBuilder newResult = new();
                for (int i = 0; i < hash.Length; i += 2)
                {
                    newResult.Append(hash[i] == hash[i + 1] ? '1' : '0');
                }
                hash = newResult.ToString();
            } while (hash.Length % 2 == 0);

            // Done
            return hash;
        }
    }
}