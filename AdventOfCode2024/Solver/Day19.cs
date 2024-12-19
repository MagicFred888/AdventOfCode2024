using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Solver
{
    internal partial class Day19() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Linen Layout";

        private List<string> towels = [];
        private List<string> patterns = [];
        private readonly Dictionary<string, long> possibleArangement = [];

        public override string GetSolution1()
        {
            LoadData();

            // Compute result
            return patterns.Aggregate(0, (int acc, string pattern) => acc + (NbrPossible(pattern) > 0 ? 1 : 0)).ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Compute result
            return patterns.Aggregate(0, (long acc, string pattern) => acc + NbrPossible(pattern)).ToString();
        }

        private long NbrPossible(string pattern)
        {
            if (possibleArangement.TryGetValue(pattern, out long value))
            {
                return value;
            }

            long nbr = 0;
            foreach (string towel in towels)
            {
                if (towel.Length <= pattern.Length && pattern.StartsWith(towel))
                {
                    string newPattern = pattern[towel.Length..];
                    if (newPattern.Length == 0)
                    {
                        nbr++;
                    }
                    else
                    {
                        long result = NbrPossible(newPattern);
                        nbr += result;
                    }
                }
            }

            possibleArangement.TryAdd(pattern, nbr);
            return nbr;
        }

        private void LoadData()
        {
            possibleArangement.Clear();
            towels = new QuickMatrix([_rawData[0].Replace(" ", "")], ",").Rows[0].ConvertAll(d => d.Value);
            patterns = [.. _rawData[2..]];
        }
    }
}