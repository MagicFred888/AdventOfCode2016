using AdventOfCode2016.Tools;

namespace AdventOfCode2016.Solver
{
    internal partial class Day08 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Two-Factor Authentication";

        private enum Instruction
        {
            Rect,
            RotateRow,
            RotateColumn
        }

        private readonly struct Command(Instruction instruction, int a, int b)
        {
            public Instruction Instruction { get; } = instruction;
            public int A { get; } = a;
            public int B { get; } = b;

            public override string ToString()
            {
                return $"{Instruction} : {A} and {B}";
            }
        }

        private List<Command> _allCommands = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            QuickMatrix screen = new(isChallenge ? 50 : 7, isChallenge ? 6 : 3, " ");
            ExecuteProgram(screen);
            return screen.Cells.Count(c => c.StringVal == "#").ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            QuickMatrix screen = new(isChallenge ? 50 : 7, isChallenge ? 6 : 3, " ");
            ExecuteProgram(screen);
            return string.Join("\r\n    ", screen.GetDebugPrintString());
        }

        private void ExecuteProgram(QuickMatrix screen)
        {
            foreach (var command in _allCommands)
            {
                switch (command.Instruction)
                {
                    case Instruction.Rect:
                        screen.GetCellsInRange(new(), new(command.A - 1, command.B - 1)).ForEach(c => c.StringVal = "#");
                        break;

                    case Instruction.RotateRow:
                        List<string> rowValues = screen.Rows[command.A].Select(c => c.StringVal).ToList();
                        for (int i = 0; i < rowValues.Count; i++)
                        {
                            screen.Rows[command.A][i].StringVal = rowValues[(1000 * screen.ColCount + i - command.B) % screen.ColCount];
                        }
                        break;

                    case Instruction.RotateColumn:
                        List<string> colValues = screen.Cols[command.A].Select(c => c.StringVal).ToList();
                        for (int i = 0; i < colValues.Count; i++)
                        {
                            screen.Cols[command.A][i].StringVal = colValues[(1000 * screen.RowCount + i - command.B) % screen.RowCount];
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void ExtractData()
        {
            _allCommands = [];
            foreach (string line in _puzzleInput)
            {
                string[] parts = line.Replace("x=", " ").Replace("y=", " ").Replace("by", " ").Replace("x", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (parts[0] == "rect")
                {
                    _allCommands.Add(new Command(Instruction.Rect, int.Parse(parts[1]), int.Parse(parts[2])));
                }
                else if (parts[0] == "rotate")
                {
                    if (parts[1] == "row")
                    {
                        _allCommands.Add(new Command(Instruction.RotateRow, int.Parse(parts[2]), int.Parse(parts[3])));
                    }
                    else if (parts[1] == "column")
                    {
                        _allCommands.Add(new Command(Instruction.RotateColumn, int.Parse(parts[2]), int.Parse(parts[3])));
                    }
                }
            }
        }
    }
}