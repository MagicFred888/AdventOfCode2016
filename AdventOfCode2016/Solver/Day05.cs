using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day05 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "How About a Nice Game of Chess?";

        private static readonly MD5 md5 = MD5.Create();

        public override string GetSolution1(bool isChallenge)
        {
            StringBuilder result = new();
            string doorId = _puzzleInput[0];
            for (int i = 0; i < int.MaxValue; i++)
            {
                // Compute Hash and check first 5 bytes
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes($"{doorId}{i}"));
                if (hash[0] != 0b00000000 || hash[1] != 0b00000000 || hash[2] > 0b00001111)
                {
                    continue;
                }

                // Add to password
                result.Append(hash[2].ToString("x2")[1]);
                if (result.Length == 8)
                {
                    return result.ToString();
                }
            }
            throw new InvalidDataException();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // fill list<string> aaa with 300 char "k"
            List<string> password = Enumerable.Repeat("-", 8).ToList();
            string doorId = _puzzleInput[0];
            for (int i = 0; i < int.MaxValue; i++)
            {
                // Compute Hash and check first 5 bytes
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes($"{doorId}{i}"));
                if (hash[0] != 0b00000000 || hash[1] != 0b00000000 || hash[2] > 0b00001111)
                {
                    continue;
                }

                // Add to password
                int position = hash[2] & 0b00001111;
                if (position >= 0 && position < 8 && password[position] == "-")
                {
                    password[position] = hash[3].ToString("x2")[0].ToString();
                    if (!password.Contains("-"))
                    {
                        return string.Join("", password);
                    }
                }
            }
            throw new InvalidDataException();
        }
    }
}