namespace AdventOfCode2016.Solver
{
    internal partial class Day19 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "An Elephant Named Joseph";

        public override string GetSolution1(bool isChallenge)
        {
            int nbrOfElves = int.Parse(_puzzleInput[0]);
            List<int> elves = Enumerable.Repeat(1, nbrOfElves).ToList();

            int pos = -1;
            do
            {
                // Get next elve and check if have gift
                pos = (pos + 1) % nbrOfElves;
                if (elves[pos] == 0) continue;

                // Search next elf with gift
                int nextPos = (pos + 1) % nbrOfElves;
                while (elves[nextPos] == 0)
                {
                    nextPos = (nextPos + 1) % nbrOfElves;
                }

                // Check if back to ourselves
                if (nextPos == pos)
                {
                    break;
                }

                // Steal gift
                elves[pos] += elves[nextPos];
                elves[nextPos] = 0;

                // Move to next elf
                pos = nextPos;
            } while (true);

            return (1 + elves.IndexOf(nbrOfElves)).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            // Not really a programming problem : https://en.wikipedia.org/wiki/Josephus_problem
            // And some code from here: https://www.reddit.com/r/adventofcode/comments/5j4lp1/2016_day_19_solutions/
            // Not enjoyeable if you are not strong at Math...

            int count = int.Parse(_puzzleInput[0]);
            int section = (int)Math.Pow(3, (int)Math.Log(count - 1, 3));
            return (count - section + Math.Max(0, count - 2 * section)).ToString();
        }
    }
}