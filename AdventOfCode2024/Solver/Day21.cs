using AdventOfCode2024.Tools;
using System.Drawing;
using System.Text;

namespace AdventOfCode2024.Solver
{
    internal partial class Day21() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Keypad Conundrum";

        private sealed class Pad
        {
            private readonly Dictionary<Point, string> _keys;
            private readonly string _startKey;

            private static readonly Dictionary<(int level, Point from, Point to), long> keypadSequences = [];
            private readonly QuickMatrix _pad;
            private Point _position;

            public Pad(Dictionary<Point, string> keys, string startKey, bool eraseDic)
            {
                // Save value for clonning
                _keys = keys;
                _startKey = startKey;

                // Initialize the pad
                if (eraseDic)
                {
                    keypadSequences.Clear();
                }
                int maxX = keys.Keys.Max(p => p.X);
                int maxY = keys.Keys.Max(p => p.Y);
                _pad = new QuickMatrix(maxX + 1, maxY + 1, "");
                foreach (Point p in keys.Keys)
                {
                    _pad.Cell(p.X, p.Y).Value = keys[p];
                }
                _position = _pad.Cells.Find(c => c.Value == startKey)?.Position ?? throw new InvalidDataException("Start key not found");
            }

            private Pad Clone()
            {
                return new Pad(_keys, _startKey, false);
            }

            public string ComputeBestSequences(string input)
            {
                List<List<string>> result = [];
                for (int i = 0; i < input.Length; i++)
                {
                    QuickMaze.SolveMaze(_pad, _position, input.Substring(i, 1), "#");
                    result.Add(QuickMaze.GetAllBestDistances(_pad));
                    _position = _pad.Cells.Find(c => c.Value == input.Substring(i, 1))?.Position ?? new(-1, -1);
                }
                return GetBestSequence(result);
            }

            public long ComputeBestSequencesLength(string sequence, int remainingLevel)
            {
                long result = 0;
                for (int i = 0; i < sequence.Length; i++)
                {
                    string currentChar = sequence.Substring(i, 1);
                    Point endPosition = _pad.Cells.Find(c => c.Value == currentChar)?.Position ?? new(-1, -1);
                    if (!keypadSequences.ContainsKey((remainingLevel, _position, endPosition)))
                    {
                        QuickMaze.SolveMaze(_pad, _position, endPosition, "#");
                        List<string> sequences = QuickMaze.GetAllBestDistances(_pad);
                        string bestSequence = GetBestSequence(sequences);
                        long moveCount = 0;
                        if (remainingLevel == 1)
                        {
                            moveCount = bestSequence.Length;
                        }
                        else
                        {
                            Pad subKeyPad = this.Clone();
                            foreach (char c in bestSequence)
                            {
                                moveCount += subKeyPad.ComputeBestSequencesLength(c.ToString(), remainingLevel - 1);
                            }
                        }
                        keypadSequences.Add((remainingLevel, _position, endPosition), moveCount);
                    }

                    result += keypadSequences[(remainingLevel, _position, endPosition)];
                    _position = endPosition;
                }
                return result;
            }

            public static string GetBestSequence(List<List<string>> allSequences)
            {
                StringBuilder result = new();
                foreach (List<string> sequence in allSequences)
                {
                    result.Append(GetBestSequence(sequence));
                }
                return result.ToString();
            }

            public static string GetBestSequence(List<string> sequences)
            {
                // Try use dictionary
                sequences.Sort();

                // Keep items with as less move as possible (first selection)
                List<string> bestSequences = [];
                List<int> sequenceScore = [];
                foreach (string sequence in sequences)
                {
                    sequenceScore.Add(Pad.ComputeNbrOfChange(sequence));
                }
                int min = sequenceScore.Min();
                for (int i = 0; i < sequenceScore.Count; i++)
                {
                    if (sequenceScore[i] == min)
                    {
                        bestSequences.Add(sequences[i]);
                    }
                }

                // Simplify chain to keep only < | (instead of v or ^) and > ==> A bit dirty, this is to avoid go back and forth on move pad
                Dictionary<string, string> clean = new()
                {
                    { "^", "|" },
                    { "v", "|" },
                    { "||", "|" },
                    { "<<", "<" },
                    { ">>", ">" }
                };
                sequenceScore = [];
                foreach (string sequence in bestSequences)
                {
                    string fixedSequence = sequence.Replace("A", "");
                    foreach (string key in clean.Keys)
                    {
                        while (fixedSequence.Contains(key))
                        {
                            fixedSequence = fixedSequence.Replace(key, clean[key]);
                        }
                    }
                    if ("<|>".Contains(fixedSequence))
                    {
                        sequenceScore.Add("<|>".IndexOf(fixedSequence));
                    }
                    else
                    {
                        sequenceScore.Add(1000);
                    }
                }
                min = sequenceScore.Min();

                // Keep only the best
                string bestSequence = bestSequences[sequenceScore.IndexOf(min)];
                return bestSequence;
            }

            private static List<string> GetAllSequence(List<List<string>> numSequence, string start = "")
            {
                if (numSequence.Count == 0)
                {
                    return [start];
                }
                List<string> result = [];
                foreach (string seq in numSequence[0])
                {
                    result.AddRange(GetAllSequence(numSequence.GetRange(1, numSequence.Count - 1), start + seq));
                }
                return result;
            }

            private static int ComputeNbrOfChange(string v)
            {
                int result = 0;
                string last = v[..1];
                for (int i = 1; i < v.Length; i++)
                {
                    if (v.Substring(i, 1) != last)
                    {
                        result++;
                        last = v.Substring(i, 1);
                    }
                }
                return result;
            }
        }

        private readonly Dictionary<Point, string> numericalPad = new()
        {
            {new(0, 0), "7"},
            {new(1, 0), "8"},
            {new(2, 0), "9"},
            {new(0, 1), "4"},
            {new(1, 1), "5"},
            {new(2, 1), "6"},
            {new(0, 2), "1"},
            {new(1, 2), "2"},
            {new(2, 2), "3"},
            {new(0, 3), "#"},
            {new(1, 3), "0"},
            {new(2, 3), "A"}
        };

        private readonly Dictionary<Point, string> directionPad = new()
        {
            {new(0, 0), "#"},
            {new(1, 0), "^"},
            {new(2, 0), "A"},
            {new(0, 1), "<"},
            {new(1, 1), "v"},
            {new(2, 1), ">"}
        };

        private List<string> codes = [];

        public override string GetSolution1()
        {
            LoadData();

            // Solve
            long result = 0;
            foreach (string code in codes)
            {
                // Get possible numpad moves
                Pad pad = new(numericalPad, "A", true);
                string bestSequence = pad.ComputeBestSequences(code);

                // Get the length of the sequence on movepad
                pad = new(directionPad, "A", true);
                long sequenceLength = pad.ComputeBestSequencesLength(bestSequence, 2);
                long codeNum = long.Parse(string.Join("", code.Where(c => char.IsDigit(c))));
                result += codeNum * sequenceLength;
            }

            return result.ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Solve
            long result = 0;
            foreach (string code in codes)
            {
                // Get possible numpad moves
                Pad pad = new(numericalPad, "A", true);
                string bestSequence = pad.ComputeBestSequences(code);

                // Get the length of the sequence on movepad
                pad = new(directionPad, "A", true);
                long sequenceLength = pad.ComputeBestSequencesLength(bestSequence, 25);
                long codeNum = long.Parse(string.Join("", code.Where(c => char.IsDigit(c))));
                result += codeNum * sequenceLength;
            }

            return result.ToString();
        }

        private void LoadData()
        {
            codes = _rawData;
        }
    }
}