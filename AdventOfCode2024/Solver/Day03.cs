using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solver
{
    internal partial class Day03() : BaseSolver()
    {
        [GeneratedRegex(@"(mul\((?<one>\d{1,3}),(?<two>\d{1,3})\)|do\(\)|don't\(\))")]
        private static partial Regex MulDoDontRegex();

        [GeneratedRegex(@"mul\((?<one>\d{1,3}),(?<two>\d{1,3})\)")]
        private static partial Regex MulRegex();

        public override string PuzzleTitle { get; } = "Mull It Over";

        public override string GetSolution1()
        {
            string fullData = string.Join("", _rawData);
            Regex _rgxExtractor = MulRegex();
            return _rgxExtractor.Matches(fullData)
                .Aggregate(0, (sum, current) => sum + int.Parse(current.Groups["one"].Value) * int.Parse(current.Groups["two"].Value))
                .ToString();
        }

        public override string GetSolution2()
        {
            bool mustConsider = true;
            string fullData = string.Join("", _rawData);
            Regex _rgxExtractor = MulDoDontRegex();
            return _rgxExtractor.Matches(fullData)
                .Aggregate(0, (sum, current) =>
                {
                    switch (current.Value)
                    {
                        case "do()":
                            mustConsider = true;
                            return sum;

                        case "don't()":
                            mustConsider = false;
                            return sum;

                        default:
                            return sum + (mustConsider ? int.Parse(current.Groups["one"].Value) * int.Parse(current.Groups["two"].Value) : 0);
                    }
                }).ToString();
        }
    }
}