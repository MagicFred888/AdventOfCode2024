using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;

namespace AdventOfCode2024.Solver
{
    internal partial class Day08() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Resonant Collinearity";

        QuickMatrix map = new();

        public override string GetSolution1()
        {
            GetData();

            // Compute
            List<string> antennas = map.Cells.FindAll(c => !c.Value.Equals(".")).ConvertAll(c => c.Value).Distinct().ToList();
            int position = ComputeAntinodes(antennas, false);

            // Done
            return position.ToString();
        }

        public override string GetSolution2()
        {
            GetData();

            // Compute
            List<string> antennas = map.Cells.FindAll(c => !c.Value.Equals(".")).ConvertAll(c => c.Value).Distinct().ToList();
            int position = ComputeAntinodes(antennas, true);

            // Done
            return position.ToString();
        }

        private int ComputeAntinodes(List<string> allAntennaTypes, bool multiAntinode)
        {
            foreach (string antennaType in allAntennaTypes)
            {
                List<Point> allAntenna = map.Cells.FindAll(c => c.Value.Equals(antennaType)).ConvertAll(c => c.Position);
                for (int i = 0; i < allAntenna.Count; i++)
                {
                    Point antenna1 = allAntenna[i];
                    for (int j = i + 1; j < allAntenna.Count; j++)
                    {
                        Point antenna2 = allAntenna[j];

                        // In multiAntinode mode, we tag the two antennas
                        if (multiAntinode)
                        {
                            map.Cell(antenna1).Tag = true;
                            map.Cell(antenna2).Tag = true;
                        }

                        Point direction = antenna1.Subtract(antenna2);
                        Point target;
                        int step = 1;
                        bool loopAgain = false;
                        do
                        {
                            loopAgain = false;

                            // Check in direction 1
                            target = antenna1.Add(direction.Multiply(step));
                            if (map.Cell(target).Value != "")
                            {
                                map.Cell(target).Tag = true;
                                loopAgain = true;
                            }

                            // Check in direction 2
                            target = antenna2.Subtract(direction.Multiply(step));
                            if (map.Cell(target).Value != "")
                            {
                                map.Cell(target).Tag = true;
                                loopAgain = true;
                            }

                            // Next position
                            step++;
                        } while (loopAgain && multiAntinode);
                    }
                }
            }

            // Count
            int position = map.Cells.Count(c => c.Tag != null);

            // Done
            return position;
        }

        private void GetData()
        {
            map = new QuickMatrix(_rawData);
        }
    }
}