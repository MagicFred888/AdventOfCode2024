namespace AdventOfCode2024.Solver
{
    internal partial class Day07() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Bridge Repair";

        private readonly List<(long result, List<long> numbers)> allData = [];

        public override string GetSolution1()
        {
            GetData();

            // Compute
            long total = 0;
            foreach ((long result, List<long> numbers) in allData)
            {
                if (CanMatch(result, numbers, false))
                {
                    total += result;
                }
            }

            // Done
            return total.ToString();
        }

        public override string GetSolution2()
        {
            GetData();

            // Compute
            long total = 0;
            foreach ((long result, List<long> numbers) in allData)
            {
                if (CanMatch(result, numbers, true))
                {
                    total += result;
                }
            }

            // Done
            return total.ToString();
        }

        private static bool CanMatch(long result, List<long> numbers, bool withCombination)
        {
            // Check if we found a match
            if (numbers.Count == 1)
            {
                return numbers[0] == result;
            }

            // Check with Add
            List<long> newNumbers = GetList(numbers[0] + numbers[1], numbers);
            if (CanMatch(result, newNumbers, withCombination))
            {
                return true;
            }

            // Check with Multiply
            newNumbers = GetList(numbers[0] * numbers[1], numbers);
            if (CanMatch(result, newNumbers, withCombination))
            {
                return true;
            }

            if (withCombination)
            {
                // Check with Combination
                newNumbers = GetList(long.Parse(numbers[0].ToString() + numbers[1].ToString()), numbers);
                if (CanMatch(result, newNumbers, withCombination))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<long> GetList(long newFirstValue, List<long> numbers)
        {
            List<long> result = numbers.GetRange(1, numbers.Count - 1);
            result[0] = newFirstValue;
            return result;
        }

        private void GetData()
        {
            allData.Clear();
            foreach (string line in _rawData)
            {
                List<long> allNbr = line.Replace(":", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(x => long.Parse(x));
                allData.Add((allNbr[0], allNbr.GetRange(1, allNbr.Count - 1)));
            }
        }
    }
}