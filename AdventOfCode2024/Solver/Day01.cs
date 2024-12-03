using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solver
{
    internal partial class Day01() : BaseSolver
    {
        [GeneratedRegex(@"^(?<left>\d+) +(?<right>\d+)$")]
        private static partial Regex LeftRightRegex();

        public override string PuzzleTitle { get; } = "Historian Hysteria";

        private List<int> _left = [];
        private List<int> _right = [];

        public override string GetSolution1()
        {
            ExtractData();
            _left.Sort();
            _right.Sort();

            return _left
                .Zip(_right, (left, right) => (left, right))
                .Aggregate(0, (total, zip) => total + Math.Abs(zip.left - zip.right))
                .ToString();
        }

        public override string GetSolution2()
        {
            ExtractData();
            return _left
                .Aggregate(0, (total, currentValue) => total + (currentValue * _right.Count(v => v == currentValue)))
                .ToString();
        }

        private void ExtractData()
        {
            Regex _rgxExtractor = LeftRightRegex();
            _left = _rawData.ConvertAll(t => int.Parse(_rgxExtractor.Match(t).Groups["left"].Value));
            _right = _rawData.ConvertAll(t => int.Parse(_rgxExtractor.Match(t).Groups["right"].Value));
        }
    }
}