using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Solver
{
    internal partial class Day11 : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Radioisotope Thermoelectric Generators";

        private enum ItemType
        {
            Microchip,
            Generator
        }

        private interface IItem
        {
            string Name { get; }
            ItemType ItemType { get; }
        }

        private sealed class Microship(string name) : IItem
        {
            public string Name { get; } = name;
            public ItemType ItemType { get; } = ItemType.Microchip;
        }

        private sealed class Generator(string name) : IItem
        {
            public string Name { get; } = name;
            public ItemType ItemType { get; } = ItemType.Generator;
        }

        private sealed class Floor(int floorNumber)
        {
            public int FloorNumber { get; } = floorNumber;
            public List<Microship> Microchips { get; set; } = [];
            public List<Generator> Generators { get; set; } = [];

            public List<IItem> AllItems
            {
                get
                {
                    List<IItem> result = new(Microchips);
                    result.AddRange(Generators);
                    return result;
                }
            }

            public string Hash
            {
                get
                {
                    int nbrOfPair = Generators.Count(generator => Microchips.Any(Microchip => Microchip.Name.Equals(generator.Name)));
                    int nbrOfStandAloneGenerator = Generators.Select(g => (IItem)g).Except(Microchips).Count();
                    int nbrOfStandAloneMicrochip = Microchips.Select(m => (IItem)m).Except(Generators).Count();
                    return $"{nbrOfPair}GM-{nbrOfStandAloneGenerator}G-{nbrOfStandAloneMicrochip}M";
                }
            }

            public List<List<IItem>> GetAllPossibleNonFriedCombination()
            {
                List<List<IItem>> result = [];
                List<IItem> allItem = AllItems;
                List<List<int>> possibleChoice = Tools.Tools.GenerateCombinations(allItem.Count, 2);
                possibleChoice.AddRange(Tools.Tools.GenerateCombinations(allItem.Count, 1));
                foreach (List<int> choice in possibleChoice)
                {
                    // Check if valid both in lift and in floor after lift left
                    List<IItem> itemsToMove = choice.Select(index => allItem[index]).ToList();
                    List<IItem> itemsToStay = allItem.Except(itemsToMove).ToList();
                    if (IsSafeConfiguration(itemsToMove) && IsSafeConfiguration(itemsToStay))
                    {
                        result.Add(itemsToMove);
                    }
                }
                return result;
            }

            public override string ToString()
            {
                StringBuilder sb = new();
                sb.Append($"F{FloorNumber} ");
                sb.Append(string.Join(" ", Microchips.Select(microship => microship.ToString())));
                sb.Append(' ');
                sb.Append(string.Join(" ", Generators.Select(generator => generator.ToString())));
                return sb.ToString();
            }
        }

        private readonly Dictionary<int, Floor> _allFloors = [];

        private readonly List<(Dictionary<int, Floor> floors, int elevatorStartPosition, int nbrOfMove)> _caseToTest = [];

        private static readonly Dictionary<string, int> s_knownConfigurations = [];

        public override string GetSolution1(bool isChallenge)
        {
            ExtractData();
            return SimulateNextMove(_allFloors).ToString();
        }

        public override string GetSolution2(bool isChallenge)
        {
            ExtractData();
            _allFloors[1].Generators.Add(new Generator("elerium"));
            _allFloors[1].Microchips.Add(new Microship("elerium"));
            _allFloors[1].Generators.Add(new Generator("dilithium"));
            _allFloors[1].Microchips.Add(new Microship("dilithium"));
            return SimulateNextMove(_allFloors).ToString();
        }

        private int SimulateNextMove(Dictionary<int, Floor> allFloors)
        {
            _caseToTest.Clear();
            _caseToTest.Add((Clone(allFloors), 1, 0));

            while (_caseToTest.Count > 0)
            {
                (Dictionary<int, Floor> floors, int elevatorStartPosition, int nbrOfMove) = _caseToTest[0];
                _caseToTest.RemoveAt(0);
                int move = SimulateNextMove(floors, elevatorStartPosition, nbrOfMove);
                if (move != int.MaxValue)
                {
                    return move;
                }
            }
            return -1;
        }

        private int SimulateNextMove(Dictionary<int, Floor> allFloors, int elevatorFloorId, int nbrOfMove)
        {
            // Check if already tested and add it only if not
            string floorsHash = ComputeFloorsHash(allFloors, elevatorFloorId);
            if (!s_knownConfigurations.TryAdd(floorsHash, nbrOfMove))
            {
                return int.MaxValue;
            }

            // Check if puzzle is completed
            if (allFloors[1].AllItems.Count == 0 && allFloors[2].AllItems.Count == 0 && allFloors[3].AllItems.Count == 0)
            {
                return nbrOfMove;
            }

            // Search each possible move who don't create any fried microship in current floor, in lift or in next floor
            List<(int nextFloor, List<IItem> itemsToMove)> possibleMoves = [];
            foreach (List<IItem> item in allFloors[elevatorFloorId].GetAllPossibleNonFriedCombination())
            {
                if (elevatorFloorId < allFloors.Count)
                {
                    // Check if after a move up, the configuration is still valid
                    List<IItem> test = new(item);
                    test.AddRange(allFloors[elevatorFloorId + 1].AllItems);
                    if (IsSafeConfiguration(test))
                    {
                        possibleMoves.Add((elevatorFloorId + 1, item));
                    }
                }
                if (elevatorFloorId > 1)
                {
                    // Check if after a move down, the configuration is still valid
                    List<IItem> test = new(item);
                    test.AddRange(allFloors[elevatorFloorId - 1].AllItems);
                    if (IsSafeConfiguration(test))
                    {
                        possibleMoves.Add((elevatorFloorId - 1, item));
                    }
                }
            }

            // Simulate all these move
            foreach ((int nextFloor, List<IItem> itemsToMove) in possibleMoves)
            {
                // Copy allFloors and make the move who is always leading to a valid solution
                Dictionary<int, Floor> tmpAllFloors = Clone(allFloors);
                tmpAllFloors[elevatorFloorId].Microchips = tmpAllFloors[elevatorFloorId].Microchips.Except(itemsToMove.OfType<Microship>()).ToList();
                tmpAllFloors[elevatorFloorId].Generators = tmpAllFloors[elevatorFloorId].Generators.Except(itemsToMove.OfType<Generator>()).ToList();
                tmpAllFloors[nextFloor].Microchips.AddRange(itemsToMove.OfType<Microship>());
                tmpAllFloors[nextFloor].Generators.AddRange(itemsToMove.OfType<Generator>());
                _caseToTest.Add((tmpAllFloors, nextFloor, nbrOfMove + 1));
            }
            return int.MaxValue;
        }

        private static bool IsSafeConfiguration(List<IItem> items)
        {
            List<Microship> microchips = items.OfType<Microship>().ToList();
            List<Generator> generators = items.OfType<Generator>().ToList();

            if (generators.Count == 0 || microchips.Count == 0)
            {
                return true;
            }

            // Check if all microship are protected
            if (microchips.Any(microship => !generators.Any(generator => generator.Name == microship.Name)))
            {
                return false;
            }

            return true;
        }

        private static string ComputeFloorsHash(Dictionary<int, Floor> allFloors, int elevatorFloorId)
        {
            return $"{elevatorFloorId}{allFloors.Values.Aggregate("", (acc, val) => acc + "|" + val.Hash)}";
        }

        private static Dictionary<int, Floor> Clone(Dictionary<int, Floor> allFloors)
        {
            // Clone dictionary and also make deep copy of Floor object
            Dictionary<int, Floor> result = [];
            foreach (var item in allFloors)
            {
                result.Add(item.Key, new Floor(item.Key)
                {
                    Generators = new(item.Value.Generators),
                    Microchips = new(item.Value.Microchips)
                });
            }
            return result;
        }

        private void ExtractData()
        {
            _allFloors.Clear();
            s_knownConfigurations.Clear();
            int floorNumber = 0;
            foreach (string info in _puzzleInput)
            {
                floorNumber++;
                _allFloors.Add(floorNumber, new Floor(floorNumber));
                foreach (Match generatorMatch in GeneratorExtractionRegex().Matches(info))
                {
                    _allFloors[floorNumber].Generators.Add(new Generator(generatorMatch.Groups["generatorName"].Value));
                }
                foreach (Match microchipMatch in MicroshipExtractionRegex().Matches(info))
                {
                    _allFloors[floorNumber].Microchips.Add(new Microship(microchipMatch.Groups["microchipName"].Value));
                }
            }
        }

        [GeneratedRegex(@" (?<generatorName>[a-zA-Z]+) generator")]
        private static partial Regex GeneratorExtractionRegex();

        [GeneratedRegex(@" (?<microchipName>[a-zA-Z]+)-compatible microchip")]
        private static partial Regex MicroshipExtractionRegex();
    }
}