using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day12() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Garden Groups";

        private QuickMatrix garden = new();

        private enum ScanDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        public override string GetSolution1()
        {
            garden = new(_rawData);

            // Scan full garden
            long result = 0;
            for (int i = 0; i < garden.Cells.Count; i++)
            {
                (int area, List<(Point position, ScanDirection scanDirection)> fences) = ScanGardenArea(garden.Cells[i]);
                if (area == 0) continue;
                result += area * fences.Count;
            }
            return result.ToString();
        }

        public override string GetSolution2()
        {
            garden = new(_rawData);

            // Scan full garden
            long result = 0;
            for (int i = 0; i < garden.Cells.Count; i++)
            {
                (int area, List<(Point position, ScanDirection scanDirection)> fences) = ScanGardenArea(garden.Cells[i]);
                if (area == 0) continue;
                int fenceWall = GetNbrOfFenceWall(fences);
                result += area * fenceWall;
            }
            return result.ToString(); // Right answer is 844132
        }

        private (int area, List<(Point position, ScanDirection scanDirection)> fencesPosition) ScanGardenArea(CellInfo cellInfo)
        {
            // Already scanned ?
            if (cellInfo.Tag != null)
            {
                return (0, []);
            }

            // Initialize the search
            string gardenLetter = cellInfo.Value;
            cellInfo.Tag = true;
            (int area, List<(Point position, ScanDirection scanDirection)> fencesPosition) result = (1, []);
            List<(int xDir, int yDir, ScanDirection scanDir)> directions = [(0, -1, ScanDirection.Up), (0, 1, ScanDirection.Down), (-1, 0, ScanDirection.Left), (1, 0, ScanDirection.Right)];

            // Search all directions
            foreach (var (xDir, yDir, scanDir) in directions)
            {
                CellInfo nextCell = garden.Cell(cellInfo.Position.Add(new Point(xDir, yDir)));
                if (!nextCell.IsValid || nextCell.Value != gardenLetter)
                {
                    result.fencesPosition.Add((cellInfo.Position.Add(new Point(xDir, yDir)), scanDir));
                }
                else if (nextCell.Tag != null)
                {
                    // Nothing to do
                }
                else
                {
                    (int area, List<(Point position, ScanDirection scanDirection)> fencesPosition) = ScanGardenArea(nextCell);
                    result.area += area;
                    result.fencesPosition.AddRange(fencesPosition);
                }
            }
            return result;
        }

        private static int GetNbrOfFenceWall(List<(Point position, ScanDirection scanDirection)> fences)
        {
            int result = 0;

            // Scan each fence by type and merge the one who are connected
            foreach (ScanDirection scanDirection in Enum.GetValues(typeof(ScanDirection)))
            {
                var fencesToConsider = fences.FindAll(f => f.scanDirection == scanDirection);
                while (fencesToConsider.Count > 0)
                {
                    bool loopAgain = false;
                    List<Point> current = [fencesToConsider[0].position];
                    do
                    {
                        loopAgain = false;
                        for (int j = 1; j < fencesToConsider.Count; j++)
                        {
                            Point testedFence = fencesToConsider[j].position;
                            bool isTouching;
                            if (scanDirection is ScanDirection.Up or ScanDirection.Down)
                            {
                                isTouching = current.Any(f => f.Y == testedFence.Y && Math.Abs(f.X - testedFence.X) == 1);
                            }
                            else
                            {
                                isTouching = current.Any(f => f.X == testedFence.X && Math.Abs(f.Y - testedFence.Y) == 1);
                            }

                            if (isTouching)
                            {
                                current.Add(fencesToConsider[j].position);
                                fencesToConsider.RemoveAt(j);
                                loopAgain = true;
                                break;
                            }
                        }
                    } while (loopAgain);
                    result += 1;
                    fencesToConsider.RemoveAt(0);
                }
            }

            // Done
            return result;
        }
    }
}