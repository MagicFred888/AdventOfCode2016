using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day04 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Security Through Obscurity";

        private readonly List<(string name, int sectorId, string checksum)> _rooms = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();

            long result = 0;
            foreach ((string name, int sectorId, string checksum) in _rooms)
            {
                result += ComputeCorrectChecksum(name) == checksum ? sectorId : 0;
            }
            return result.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();

            // Search room about North Pole
            foreach ((string name, int sectorId, string checksum) in _rooms)
            {
                if (ComputeCorrectChecksum(name) == checksum)
                {
                    string decryptedName = DecryptName(name, sectorId);
                    if (!isChallenge)
                    {
                        return decryptedName;
                    }
                    if (decryptedName.Contains("north"))
                    {
                        return sectorId.ToString();
                    }
                }
            }
            throw new InvalidDataException();
        }

        private static string DecryptName(string encryptedData, int numberOfShift)
        {
            // Compute Cesar cipher by moving letter sectorId times
            int offset = numberOfShift % 26;
            StringBuilder result = new();
            foreach (char c in encryptedData)
            {
                if (c == '-')
                {
                    result.Append(' ');
                }
                else
                {
                    result.Append((char)('a' + (c - 'a' + offset) % 26));
                }
            }
            return result.ToString();
        }

        private static string ComputeCorrectChecksum(string name)
        {
            Dictionary<char, int> letterCount = [];
            foreach (char c in name)
            {
                if (c == '-')
                {
                    continue;
                }
                if (letterCount.TryGetValue(c, out int value))
                {
                    letterCount[c] = ++value;
                }
                else
                {
                    letterCount.Add(c, 1);
                }
            }
            return string.Join("", letterCount.OrderByDescending(x => x.Value).ThenBy(x => x.Key).Take(5).Select(x => x.Key));
        }

        private void ExtractData()
        {
            _rooms.Clear();
            foreach (string roomInfo in _puzzleInput)
            {
                string[] parts = roomInfo.TrimEnd(']').Split('-');
                string name = string.Join("-", parts[..^1]);
                int sectorId = int.Parse(parts[^1].Split('[')[0]);
                string checksum = parts[^1].Split('[')[1];
                _rooms.Add((name, sectorId, checksum));
            }
        }
    }
}