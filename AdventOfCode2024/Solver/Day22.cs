namespace AdventOfCode2024.Solver
{
    internal partial class Day22() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Monkey Market";

        private List<long> seeds = [];

        public override string GetSolution1()
        {
            LoadData();

            // Scan maze
            return seeds.Aggregate(0, (long total, long currentValue) => total + GetPseudoRandomValue(currentValue, 2000)).ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Not efficient but it works in about 1 minute
            // Compute price
            List<List<long>> allPrices = [];
            foreach (long seed in seeds)
            {
                allPrices.Add([]);
                ComputePrices(allPrices[^1], seed, 2000);
            }

            // Compute changes
            List<List<long>> allChanges = [];
            foreach (List<long> prices in allPrices)
            {
                allChanges.Add([]);
                allChanges[^1].Add(long.MinValue);
                for (int i = 1; i < 2000; i++)
                {
                    allChanges[^1].Add(prices[i] - prices[i - 1]);
                }
            }

            List<(long, long, long, long)> allSequences = [];
            List<Dictionary<(long, long, long, long), long>> sequences = [];
            for (int i = 0; i < allChanges.Count; i++)
            {
                List<long> prices = allPrices[i];
                List<long> changes = allChanges[i];
                Dictionary<(long, long, long, long), long> sequence = [];
                for (int j = 3; j < prices.Count; j++)
                {
                    (long, long, long, long) key = (changes[j - 3], changes[j - 2], changes[j - 1], changes[j]);
                    if (!allSequences.Contains(key))
                    {
                        allSequences.Add(key);
                    }
                    if (!sequence.ContainsKey(key))
                    {
                        sequence.Add(key, prices[j]);
                    }
                }
                sequences.Add(sequence);
            }

            long bestResult = 0;
            for (int i = 0; i < allSequences.Count; i++)
            {
                long tmpResult = 0;
                (long, long, long, long) key = allSequences[i];
                foreach (Dictionary<(long, long, long, long), long> sequence in sequences)
                {
                    if (sequence.ContainsKey(key))
                    {
                        tmpResult += sequence[key];
                    }
                }
                if (tmpResult > bestResult)
                {
                    bestResult = tmpResult;
                }
            }

            // Scan maze
            return bestResult.ToString();
        }

        private void ComputePrices(List<long> dictionary, long seed, int numberOfCycles)
        {
            dictionary.Add(seed % 10);
            seed = GetPseudoRandomValue(seed, 1);
            if (numberOfCycles > 1)
            {
                ComputePrices(dictionary, seed, numberOfCycles - 1);
            }
        }

        private long GetPseudoRandomValue(long secretNumber, int numberOfCycles)
        {
            secretNumber = MixAndPrune(64 * secretNumber, secretNumber);
            secretNumber = MixAndPrune(secretNumber / 32, secretNumber);
            secretNumber = MixAndPrune(secretNumber * 2048, secretNumber);
            return numberOfCycles > 1 ? GetPseudoRandomValue(secretNumber, numberOfCycles - 1) : secretNumber;
        }

        private long MixAndPrune(long number, long secretNumber)
        {
            return (number ^ secretNumber) % 16777216;
        }

        private void LoadData()
        {
            seeds = _rawData.ConvertAll(long.Parse);
        }
    }
}