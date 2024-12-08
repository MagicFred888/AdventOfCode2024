using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solver
{
    internal enum RoundId
    {
        FirstRound = 0,
        SecondRound = 1,
    }

    internal abstract partial class BaseSolver
    {
        [GeneratedRegex(@"^Day(?<day>\d+)$")]
        private static partial Regex ExtractDayNumberRegex();

        private readonly int day;

        private sealed class DataSet
        {
            public required string TestFileName;
            public List<string> Data = [];
            public string[] RoundIdAnswers = new string[2];
        }

        // To be used by children class to solve challenges
        protected List<string> _rawData = [];

        // For internal use only
        private readonly List<string> _challengeData = [];

        private readonly List<DataSet> _sampleDataSet = [];

        private readonly Stopwatch _stopwatch = new();

        private readonly string _splitChar = "_";

        protected BaseSolver()
        {
            // Base data folder
            this.day = int.Parse(ExtractDayNumberRegex().Match(this.GetType().Name).Groups["day"].Value);
            _challengeData.Clear();
            _sampleDataSet.Clear();
            string dataFolder = @$"..\..\..\Data\Day{day:00}\";

            // Load challenge data
            string challengeDataFilePath = $"{dataFolder}Challenge.txt";
            if (File.Exists(challengeDataFilePath)) _challengeData = [.. File.ReadAllLines(challengeDataFilePath)];

            // Load sample data
            string[] sampleDataFilePath = Directory.GetFiles(dataFolder, "Sample*.txt");
            foreach (string filePath in sampleDataFilePath)
            {
                // Skip if empty
                if (new FileInfo(filePath).Length == 0) continue;

                // Split file name
                string cleanFileName = Path.GetFileNameWithoutExtension(filePath);
                cleanFileName = cleanFileName[(cleanFileName.IndexOf(_splitChar) + 1)..];
                string[] answers = cleanFileName.Split('_');

                // Load, check and save sample
                if (answers.Length > 0)
                {
                    DataSet newSet = new()
                    {
                        TestFileName = Path.GetFileName(filePath),
                        Data = [.. File.ReadAllLines(filePath)]
                    };
                    if (answers[0] != "X") newSet.RoundIdAnswers[0] = answers[0];
                    if (answers.Length >= 2 && answers[1] != "X") newSet.RoundIdAnswers[1] = answers[1];
                    if (newSet.RoundIdAnswers[0] != null || newSet.RoundIdAnswers[1] != null) _sampleDataSet.Add(newSet);
                }
            }

            // Print title
            Console.WriteLine("");
            string title = $"Day {day}: {PuzzleTitle}";
            Console.WriteLine(title);
            Console.WriteLine(new string('^', title.Length));
            Console.WriteLine("");
        }

        public bool SolveSample(RoundId roundId, out string[] resultString)
        {
            // Test sample
            bool allTestPassed = true;
            List<string> results = [];
            int testId = 1;
            foreach (DataSet ds in _sampleDataSet)
            {
                // Check if must be tested
                if (ds.RoundIdAnswers[(int)roundId] == null) continue;

                // Do test
                _rawData = ds.Data;
                _stopwatch.Restart();
                string answer = roundId == RoundId.FirstRound ? GetSolution1() : GetSolution2();
                _stopwatch.Stop();
                if (answer == ds.RoundIdAnswers[(int)roundId])
                {
                    results.Add($"SAMPLE {testId} PASSED: {answer} found in {GetProperUnitAndRounding(_stopwatch.Elapsed.TotalMilliseconds)}");
                }
                else
                {
                    results.Add($"SAMPLE {testId} FAILED: Found {answer} instead of {ds.RoundIdAnswers[(int)roundId]}");
                    allTestPassed = false;
                }
            }

            // Give result
            if (results.Count == 0)
            {
                // No sample
                resultString = ["No sample data found !"];
            }
            else
            {
                // Give results
                resultString = [.. results];
            }

            // Done
            return allTestPassed;
        }

        public bool SolveChallenge(RoundId roundId, out string resultString)
        {
            // Test sample
            _rawData = _challengeData;
            if (_challengeData.Count == 0)
            {
                resultString = $"NO CHALLENGE DATA FOUND ! Please make sure you save your puzzle input into Data\\Day{day:00}\\Challenge.txt !";
                return false;
            }
            _stopwatch.Restart();
            string answer = roundId == RoundId.FirstRound ? GetSolution1() : GetSolution2();
            _stopwatch.Stop();
            resultString = $"{answer} found in {GetProperUnitAndRounding(_stopwatch.Elapsed.TotalMilliseconds)}";
            return true;
        }

        private static string GetProperUnitAndRounding(double totalMilliseconds)
        {
            // Change scale
            double duration = totalMilliseconds;
            string unit = "[ms]";
            if (totalMilliseconds < 1)
            {
                duration *= 1000;
                unit = "[μs]";
            }
            else if (totalMilliseconds >= 1000)
            {
                duration /= 1000;
                unit = "[s]";
            }

            // Choose rounding
            int nbrOfDecimals = 1;
            if (duration < 100 && duration >= 10)
            {
                nbrOfDecimals = 2;
            }
            else if (duration < 10)
            {
                nbrOfDecimals = 3;
            }

            // Done
            return $"{Math.Round(duration, nbrOfDecimals)} {unit}";
        }

        public abstract string PuzzleTitle { get; }

        public abstract string GetSolution1();

        public abstract string GetSolution2();
    }
}