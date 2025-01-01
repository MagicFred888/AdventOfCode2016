namespace AdventOfCode2016.Solver
{
    internal partial class Day23 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Safe Cracking";

        private List<(string instruction, string x, string y)> program = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return ExecuteProgramAndGetARegisterValue(7).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            return ExecuteProgramAndGetARegisterValue(12).ToString();
        }

        private int ExecuteProgramAndGetARegisterValue(int aRegisterValue)
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
            while (programPosition >= 0 && programPosition < program.Count)
            {
                (string instruction, string x, string y) = program[programPosition];

                switch (instruction)
                {
                    case "cpy":
                        if (y != "")
                        {
                            registers[y] = int.TryParse(x, out int value) ? value : registers[x];
                        }
                        programPosition++;
                        break;

                    case "inc":
                        if (y == "")
                        {
                            registers[x]++;
                        }
                        programPosition++;
                        break;

                    case "dec":
                        if (y == "")
                        {
                            registers[x]--;
                        }
                        programPosition++;
                        break;

                    case "jnz":
                        if (y != "")
                        {
                            int xVal = int.TryParse(x, out int value2) ? value2 : registers[x];
                            int yVal = int.TryParse(y, out int value3) ? value3 : registers[y];
                            programPosition += (xVal == 0 ? 1 : yVal);
                        }
                        else
                        {
                            programPosition++;
                        }
                        break;

                    case "tgl":
                        int tglDistance = int.TryParse(x, out int value4) ? value4 : registers[x];
                        if (programPosition + tglDistance < program.Count)
                        {
                            (string instruction, string x, string y) instructionToEdit = program[programPosition + tglDistance];
                            int nbrArgument = instructionToEdit.y == "" ? 1 : 2;
                            if (nbrArgument == 1)
                            {
                                program[programPosition + tglDistance] = (instructionToEdit.instruction == "inc" ? "dec" : "inc", instructionToEdit.x, instructionToEdit.y);
                            }
                            else
                            {
                                program[programPosition + tglDistance] = (instructionToEdit.instruction == "jnz" ? "cpy" : "jnz", instructionToEdit.x, instructionToEdit.y);
                            }
                        }
                        programPosition++;
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