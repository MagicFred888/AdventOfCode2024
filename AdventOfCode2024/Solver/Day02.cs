namespace AdventOfCode2024.Solver
{
    internal partial class Day02() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Red-Nosed Reports";

        private List<List<int>> allLines = [];

        public override string GetSolution1()
        {
            ExtractData();
            return allLines.Sum(line => IsSafe(line, false) ? 1 : 0).ToString();
        }

        public override string GetSolution2()
        {
            ExtractData();
            return allLines.Sum(line => IsSafe(line, true) ? 1 : 0).ToString();
        }

        private static bool IsSafe(List<int> line, bool canRemoveOneItem)
        {
            for (int posToRemove = -1; posToRemove < line.Count; posToRemove++)
            {
                List<int> fixedData = new(line);
                if (posToRemove >= 0)
                {
                    fixedData.RemoveAt(posToRemove);
                }
                bool isSafe = IsSafe(fixedData);
                if (isSafe || !canRemoveOneItem)
                {
                    return isSafe;
                }
            }
            return false;
        }

        private static bool IsSafe(List<int> line)
        {
            if (line.Count == 1 || line[0] == line[1])
            {
                return false;
            }
            bool increasing = line[0] < line[1];
            for (int pos = 0; pos < line.Count - 1; pos++)
            {
                if ((increasing && line[pos] > line[pos + 1])
                    || (!increasing && line[pos] < line[pos + 1])
                    || Math.Abs(line[pos] - line[pos + 1]) == 0
                    || Math.Abs(line[pos] - line[pos + 1]) > 3)
                {
                    return false;
                }
            }
            return true;
        }

        private void ExtractData()
        {
            allLines = _rawData.Aggregate(new List<List<int>>(), (data, line) =>
            {
                data.Add(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(t => int.Parse(t)));
                return data;
            });
        }
    }
}