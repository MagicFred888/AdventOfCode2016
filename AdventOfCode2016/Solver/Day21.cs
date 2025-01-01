using System.Diagnostics;

namespace AdventOfCode2016.Solver
{
    internal partial class Day21 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Scrambled Letters and Hash";

        private List<(string instruction, string x, string y)> program = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return ScramblePassword(isChallenge ? "abcdefgh" : "abcde");
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return UnscramblePassword(isChallenge ? "fbgdceah" : "aefgbcdh");
        }

        private string UnscramblePassword(string password)
        {
            // Execute program
            char[] passwordArray = password.ToCharArray();

            int programPosition = program.Count - 1;
            while (programPosition >= 0 && programPosition < program.Count)
            {
                (string instruction, string x, string y) = program[programPosition];
                switch (instruction)
                {
                    case "move": // Reverse move
                        int a = int.Parse(x);
                        int b = int.Parse(y);
                        if (a < b)
                        {
                            char bChar = passwordArray[b];
                            Array.Copy(passwordArray, a, passwordArray, a + 1, b - a);
                            passwordArray[a] = bChar;
                        }
                        else
                        {
                            char bChar = passwordArray[b];
                            Array.Copy(passwordArray, b + 1, passwordArray, b, a - b);
                            passwordArray[a] = bChar;
                        }
                        break;

                    case "reverse":
                        // Reverse char from x to y
                        int start = int.Parse(x);
                        int end = int.Parse(y);
                        Array.Reverse(passwordArray, start, end - start + 1);
                        break;

                    case "rotate":
                        if (x == "left")
                        {
                            // Rotate array left by X positions
                            passwordArray = RotateRight(passwordArray, int.Parse(y)); // We then need move right to reverse
                        }
                        else if (x == "right")
                        {
                            // Rotate array right by X positions
                            passwordArray = RotateLeft(passwordArray, int.Parse(y)); // We then need move left to reverse
                        }
                        else
                        {
                            // Compute Left move to revert after reverse engineering, will only work for 8 characters
                            int index = Array.IndexOf(passwordArray, y[0]);
                            if (index == 0)
                            {
                                index = 8;
                            }
                            int moveSize;
                            if (index % 2 == 0)
                            {
                                moveSize = (10 + index) / 2;
                            }
                            else
                            {
                                moveSize = (1 + index) / 2;
                            }
                            passwordArray = RotateLeft(passwordArray, moveSize);
                        }
                        break;

                    case "swap":
                        int pos1 = int.TryParse(x, out int p1) ? p1 : Array.IndexOf(passwordArray, x[0]);
                        int pos2 = int.TryParse(y, out int p2) ? p2 : Array.IndexOf(passwordArray, y[0]);
                        (passwordArray[pos1], passwordArray[pos2]) = (passwordArray[pos2], passwordArray[pos1]);
                        break;
                }
                programPosition--;
            }

            // Convert array to string
            return new string(passwordArray);
        }

        private string ScramblePassword(string password)
        {
            // Execute program
            char[] passwordArray = password.ToCharArray();

            int programPosition = 0;
            while (programPosition >= 0 && programPosition < program.Count)
            {
                (string instruction, string x, string y) = program[programPosition];
                switch (instruction)
                {
                    case "move":
                        int a = int.Parse(x);
                        int b = int.Parse(y);
                        if (a < b)
                        {
                            char aChar = passwordArray[a];
                            Array.Copy(passwordArray, a + 1, passwordArray, a, b - a);
                            passwordArray[b] = aChar;
                        }
                        else
                        {
                            char aChar = passwordArray[a];
                            Array.Copy(passwordArray, b, passwordArray, b + 1, a - b);
                            passwordArray[b] = aChar;
                        }
                        break;

                    case "reverse":
                        // Reverse char from x to y
                        int start = int.Parse(x);
                        int end = int.Parse(y);
                        Array.Reverse(passwordArray, start, end - start + 1);
                        break;

                    case "rotate":
                        if (x == "left")
                        {
                            // Rotate array left by X positions
                            passwordArray = RotateLeft(passwordArray, int.Parse(y));
                        }
                        else if (x == "right")
                        {
                            // Rotate array right by X positions
                            passwordArray = RotateRight(passwordArray, int.Parse(y));
                        }
                        else
                        {
                            // Search y in password array
                            int index = Array.IndexOf(passwordArray, y[0]);
                            if (index >= 4) index++;
                            passwordArray = RotateRight(passwordArray, index + 1);
                        }
                        break;

                    case "swap":
                        int pos1 = int.TryParse(x, out int p1) ? p1 : Array.IndexOf(passwordArray, x[0]);
                        int pos2 = int.TryParse(y, out int p2) ? p2 : Array.IndexOf(passwordArray, y[0]);
                        (passwordArray[pos1], passwordArray[pos2]) = (passwordArray[pos2], passwordArray[pos1]);
                        break;
                }

                Debug.WriteLine($"{programPosition}: {new string(passwordArray)}");

                programPosition++;
            }

            // Convert array to string
            return new string(passwordArray);
        }

        private static char[] RotateRight(char[] passwordArray, int rotateDistance)
        {
            int rotateRight = rotateDistance % passwordArray.Length;
            char[] temp = new char[passwordArray.Length];
            Array.Copy(passwordArray, 0, temp, rotateRight, passwordArray.Length - rotateRight);
            Array.Copy(passwordArray, passwordArray.Length - rotateRight, temp, 0, rotateRight);
            return temp;
        }

        private static char[] RotateLeft(char[] passwordArray, int rotateDistance)
        {
            int rotateLeft = rotateDistance % passwordArray.Length;
            char[] temp = new char[passwordArray.Length];
            Array.Copy(passwordArray, rotateLeft, temp, 0, passwordArray.Length - rotateLeft);
            Array.Copy(passwordArray, 0, temp, passwordArray.Length - rotateLeft, rotateLeft);
            return temp;
        }

        private void ExtractData()
        {
            program = [];
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string instruction = parts[0];

                switch (instruction)
                {
                    case "move":
                        program.Add(("move", parts[2], parts[5]));
                        break;

                    case "reverse":
                        program.Add(("reverse", parts[2], parts[4]));
                        break;

                    case "rotate":
                        if (parts[1] == "left" || parts[1] == "right")
                        {
                            program.Add(("rotate", parts[1], parts[2]));
                        }
                        else
                        {
                            program.Add(("rotate", "", parts[6]));
                        }
                        break;

                    case "swap":
                        program.Add(("swap", parts[2], parts[5]));
                        break;

                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }
}