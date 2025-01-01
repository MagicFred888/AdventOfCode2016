namespace AdventOfCode2016.Solver
{
    internal partial class Day25 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Clock Signal";

        private List<(string instruction, string x, string y)> program = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            int iniVal = 0;
            while (!ExecuteProgramAndGetARegisterValue(iniVal))
            {
                iniVal++;
            }
            return iniVal.ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            return "Merry Christmas";
        }

        private bool ExecuteProgramAndGetARegisterValue(int aRegisterValue)
        {
            // Initialize registers
            Dictionary<string, int> registers = new()
            {
                ["a"] = aRegisterValue,
                ["b"] = 0,
                ["c"] = 0,
                ["d"] = 0
            };
            int programPosition = 0;

            // Execute program
            bool expectedSignal = false;
            int nbrToMatch = 50;

            while (programPosition >= 0 && programPosition < program.Count)
            {
                (string instruction, string x, string y) = program[programPosition];
                switch (instruction)
                {
                    case "cpy":
                        registers[y] = int.TryParse(x, out int value) ? value : registers[x];
                        programPosition++;
                        break;

                    case "inc":
                        registers[x]++;
                        programPosition++;
                        break;

                    case "dec":
                        registers[x]--;
                        programPosition++;
                        break;

                    case "jnz":
                        int xVal = int.TryParse(x, out int value2) ? value2 : registers[x];
                        programPosition += (xVal == 0 ? 1 : int.Parse(y));
                        break;

                    case "out":
                        int outValue = int.TryParse(x, out int value3) ? value3 : registers[x];
                        if (outValue != 0 && outValue != 1)
                        {
                            return false;
                        }
                        if (!expectedSignal && outValue == 1 || expectedSignal && outValue == 0)
                        {
                            return false;
                        }
                        expectedSignal = !expectedSignal;
                        nbrToMatch--;
                        if (nbrToMatch == 0)
                        {
                            return true;
                        }
                        programPosition++;
                        break;
                }
            }
            return false;
        }

        private void ExtractData()
        {
            program = [];
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string instruction = parts[0];
                string a = parts[1];
                string b = parts.Length > 2 ? parts[2] : "";
                program.Add((instruction, a, b));
            }
        }
    }
}