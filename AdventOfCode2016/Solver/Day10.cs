using System.Text.RegularExpressions;

namespace AdventOfCode2016.Solver
{
    internal partial class Day10 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Balance Bots";

        private sealed class Bot(int id, int lowBot, int highBot, int lowOutput, int highOutput)
        {
            public int Id { get; } = id;
            public List<int> Chips { get; set; } = [];
            public int LowBot { get; } = lowBot;
            public int HighBot { get; } = highBot;
            public int LowOutput { get; } = lowOutput;
            public int HighOutput { get; } = highOutput;
        }

        private readonly Dictionary<int, Bot> _bots = [];
        private readonly Dictionary<int, int> _outputs = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return RunFullSequenceAndGetBotOfInterest([isChallenge ? 17 : 2, isChallenge ? 61 : 5]).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            _ = RunFullSequenceAndGetBotOfInterest([isChallenge ? 17 : 2, isChallenge ? 61 : 5]);
            return (_outputs[0] * _outputs[1] * _outputs[2]).ToString();
        }

        private int RunFullSequenceAndGetBotOfInterest(List<int> targetChip)
        {
            int result = -1;
            bool haveChange;
            do
            {
                haveChange = false;
                foreach (Bot bot in _bots.Values)
                {
                    if (bot.Chips.Count == 2)
                    {
                        // Get information
                        haveChange = true;
                        int lowChip = bot.Chips.Min();
                        int highChip = bot.Chips.Max();

                        // Move low and high chips
                        if (bot.LowBot != -1)
                        {
                            _bots[bot.LowBot].Chips.Add(lowChip);
                        }
                        else if (bot.LowOutput != -1)
                        {
                            _outputs.TryAdd(bot.LowOutput, lowChip);
                        }
                        if (bot.HighBot != -1)
                        {
                            _bots[bot.HighBot].Chips.Add(highChip);
                        }
                        else if (bot.HighOutput != -1)
                        {
                            _outputs.TryAdd(bot.HighOutput, highChip);
                        }
                        bot.Chips.Clear();

                        // Check if bot of interest
                        if (targetChip.Contains(lowChip) && targetChip.Contains(highChip))
                        {
                            result = bot.Id;
                        }
                    }
                }
            } while (haveChange);
            return result;
        }

        private void ExtractData()
        {
            _bots.Clear();
            _outputs.Clear();
            _outputs.Clear();

            // Extract bots
            foreach (string info in _puzzleInput.FindAll(l => BotInfoExtractorRegex().IsMatch(l)))
            {
                Match match = BotInfoExtractorRegex().Match(info);
                int botId = int.Parse(match.Groups["botId"].Value);
                int output1Id = int.Parse(match.Groups["output1Id"].Value);
                int output2Id = int.Parse(match.Groups["output2Id"].Value);
                string output1Type = match.Groups["output1Type"].Value;
                string output2Type = match.Groups["output2Type"].Value;
                _bots.Add(botId, new Bot(botId,
                    output1Type == "bot" ? output1Id : -1,
                    output2Type == "bot" ? output2Id : -1,
                    output1Type == "output" ? output1Id : -1,
                    output2Type == "output" ? output2Id : -1));
            }

            // Initialize bot
            foreach (string info in _puzzleInput.FindAll(l => ChipInfoExtractorRegex().IsMatch(l)))
            {
                Match match = ChipInfoExtractorRegex().Match(info);
                int chip = int.Parse(match.Groups["chip"].Value);
                int botId = int.Parse(match.Groups["botId"].Value);
                if (_bots.TryGetValue(botId, out Bot? bot))
                {
                    bot.Chips.Add(chip);
                }
            }
        }

        [GeneratedRegex("^bot (?<botId>\\d+) gives low to (?<output1Type>(output|bot)) (?<output1Id>\\d+) and high to (?<output2Type>(output|bot)) (?<output2Id>\\d+)$")]
        private static partial Regex BotInfoExtractorRegex();

        [GeneratedRegex("^value (?<chip>\\d+) goes to bot (?<botId>\\d+)$")]
        private static partial Regex ChipInfoExtractorRegex();
    }
}