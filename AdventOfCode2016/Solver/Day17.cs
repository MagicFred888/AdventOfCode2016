using AdventOfCode2016.Extensions;
using AdventOfCode2016.Tools;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Solver
{
    internal partial class Day17 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Two Steps Forward";

        private readonly Dictionary<string, Point> _directions = new()
        {
            { "U", new Point(0, -1) },
            { "D", new Point(0, 1) },
            { "L", new Point(-1, 0) },
            { "R", new Point(1, 0) }
        };

        private readonly List<(string passcode, QuickMatrix vaultRoom, Point position, string moves)> _toTest = [];

        private static readonly MD5 md5 = MD5.Create();

        public override string GetSolution1(bool isChallenge)
        {
            return GetPathToVault(false);
        }

        public override string GetSolution2(bool isChallenge)
        {
            return GetPathToVault(true).Length.ToString();
        }

        private string GetPathToVault(bool findLongest)
        {
            // Initiate a BFS search
            string iniPasscode = _puzzleInput[0];
            _toTest.Clear();
            _toTest.Add((iniPasscode, new QuickMatrix(4, 4), new Point(0, 0), ""));
            string longestPath = string.Empty;
            while (_toTest.Count > 0)
            {
                (string passcode, QuickMatrix vaultRoom, Point position, string moves) = _toTest[0];
                _toTest.RemoveAt(0);

                // Check if we are at the vault
                string? path = GetPathToVault(passcode, vaultRoom, position, moves);
                if (path != null)
                {
                    if (path.Length > longestPath.Length)
                    {
                        longestPath = path;
                    }
                    if (!findLongest)
                    {
                        break;
                    }
                }
            }
            return longestPath;
        }

        private string? GetPathToVault(string passcode, QuickMatrix vaultRoom, Point position, string moves)
        {
            // Check if we are at the vault
            if (position.X == vaultRoom.ColCount - 1 && position.Y == vaultRoom.RowCount - 1)
            {
                return moves;
            }

            // Compute MD5 hash
            Dictionary<string, bool> moveOkBasedOnDoorLocked = GetDoorState(passcode, moves);
            foreach (KeyValuePair<string, Point> move in _directions)
            {
                if (!moveOkBasedOnDoorLocked[move.Key])
                {
                    // Door is locked
                    continue;
                }
                if (!vaultRoom.Cell(position.Add(move.Value)).IsValid)
                {
                    // Out of the vault (wall)
                    continue;
                }

                // Save to move as a valid move to be tested
                _toTest.Add((passcode, vaultRoom.Clone(), position.Add(move.Value), moves + move.Key));
            }
            return null;
        }

        private static Dictionary<string, bool> GetDoorState(string passcode, string moves)
        {
            // Return a dictionary with the state of each door
            string hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(passcode + moves))).Replace("-", "").ToLower()[..4];
            return new()
            {
                { "U", hash[0] > 'a' },
                { "D", hash[1] > 'a' },
                { "L", hash[2] > 'a' },
                { "R", hash[3] > 'a' }
            };
        }
    }
}