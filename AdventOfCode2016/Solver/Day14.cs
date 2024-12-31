using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day14 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "One-Time Pad";

        private static readonly MD5 md5 = MD5.Create();

        public override string GetSolution1(bool isChallenge)
        {
            return SearchKeys(1).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            return SearchKeys(2017).ToString();
        }

        private int SearchKeys(int nbrOfRecursion)
        {
            int nbrFound = 0;
            string salt = _puzzleInput[0];
            List<(char letter, int start, int last)> keysToCheck = [];

            for (int i = 0; i < int.MaxValue; i++)
            {
                // Compute Hash and convert it as string
                string hashString = ComputeMD5($"{salt}{i}", nbrOfRecursion);

                // Check if we have pentaples in the keysToCheck
                for (int j = 0; j < keysToCheck.Count; j++)
                {
                    if (keysToCheck[j].last >= i && FindPintaple(hashString, keysToCheck[j].letter))
                    {
                        nbrFound++;
                        if (nbrFound == 64)
                        {
                            return keysToCheck[j].start;
                        }
                    }
                }

                // Check if there is a triple in the hash
                char? tripleChar = FindTripleChar(hashString);
                if (tripleChar != null)
                {
                    keysToCheck.Add((tripleChar.Value, i, i + 1000));
                }

                // Remove already checked items
                keysToCheck = keysToCheck.Where(k => k.last > i).ToList();
            }
            throw new InvalidDataException();
        }

        private static string ComputeMD5(string data, int nbrOfRecursion)
        {
            byte[] source = Encoding.ASCII.GetBytes(data);
            for (int i = 0; i < nbrOfRecursion; i++)
            {
                source = md5.ComputeHash(source);
                if (i < nbrOfRecursion - 1)
                {
                    // Convert to lowercase hexadecimal string and back to byte array
                    string hexString = BitConverter.ToString(source).Replace("-", "").ToLower();
                    source = Encoding.ASCII.GetBytes(hexString);
                }
            }
            return BitConverter.ToString(source).Replace("-", "").ToLower();
        }

        private static bool FindPintaple(string hashString, char letter)
        {
            return hashString.Contains(new string(letter, 5));
        }

        private static char? FindTripleChar(string hashString)
        {
            // Serach 3 charcters same in a row
            for (int i = 0; i < hashString.Length - 2; i++)
            {
                if (hashString[i] == hashString[i + 1] && hashString[i] == hashString[i + 2])
                {
                    return hashString[i];
                }
            }
            return null;
        }
    }
}