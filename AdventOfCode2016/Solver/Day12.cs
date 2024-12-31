namespace AdventOfCode2016.Solver
{
    internal partial class Day12 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Leonardo's Monorail";

        private List<(string instruction, string x, string y)> program = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return ExecuteProgramAndGetARegisterValue(0).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return ExecuteProgramAndGetARegisterValue(1).ToString();
        }

        private int ExecuteProgramAndGetARegisterValue(int cRegisterValue)
        {
            // Initialize registers
            Dictionary<string, int> registers = new()
            {
                ["a"] = 0,
                ["b"] = 0,
                ["c"] = cRegisterValue,
                ["d"] = 0
            };
            int programPosition = 0;

            // Execute program
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
                }
            }
            return registers["a"];
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