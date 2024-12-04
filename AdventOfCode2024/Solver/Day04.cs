using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;

namespace AdventOfCode2024.Solver
{
    internal partial class Day04() : BaseSolver()
    {
        QuickMatrix qm = new();

        public override string PuzzleTitle { get; } = "Ceres Search";

        public override string GetSolution1()
        {
            qm = new(_rawData);
            int result = qm.Cells.FindAll(c => c.Value == "X").Aggregate(0, (acc, cell) => acc + CountAround(cell.Position));
            return result.ToString();
        }

        private int CountAround(Point p)
        {
            // From given pos, scan data in each direction to find XMAS
            string refString = "XMAS";
            int result = 0;
            for (int dirX = -1; dirX <= 1; dirX++)
            {
                for (int dirY = -1; dirY <= 1; dirY++)
                {
                    if (dirX == 0 && dirY == 0) continue;
                    int distance = 1;
                    while (distance < refString.Length)
                    {
                        if (!qm.CellStr(p.X + distance * dirX, p.Y + distance * dirY).Equals(refString.Substring(distance, 1))) break;
                        distance++;
                    }
                    if (distance >= refString.Length)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public override string GetSolution2()
        {
            qm = new(_rawData);
            int result = qm.Cells.FindAll(c => c.Value == "A").Aggregate(0, (acc, cell) => acc + CountXAround(cell.Position));
            return result.ToString();
        }

        private int CountXAround(Point p)
        {
            int nbrOfMas = 0;

            Point p1 = new(p.X - 1, p.Y - 1);
            Point p2 = new(p.X + 1, p.Y + 1);
            for (int i = 0; i < 4; i++)
            {
                nbrOfMas += qm.CellStr(p1) == "M" && qm.CellStr(p2) == "S" ? 1 : 0;
                p1 = p1.RotateClockwise(p);
                p2 = p2.RotateClockwise(p);
            }
            return nbrOfMas == 2 ? 1 : 0;
        }
    }
}