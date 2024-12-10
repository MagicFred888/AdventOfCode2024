using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day10() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Hoof It";

        QuickMatrix map = new();

        public override string GetSolution1()
        {
            GetData();

            // Scan each possible start
            long result = 0;
            List<CellInfo> starts = map.Cells.FindAll(c => c.Value.ToInt() == 0);
            foreach (CellInfo point in starts)
            {
                map.ClearAllTags();
                result += FindPathToNine(point, false);
            }

            // Done
            return result.ToString();
        }

        public override string GetSolution2()
        {
            GetData();

            // Scan each possible start
            long result = 0;
            List<CellInfo> starts = map.Cells.FindAll(c => c.Value.ToInt() == 0);
            foreach (CellInfo point in starts)
            {
                map.ClearAllTags();
                result += FindPathToNine(point, true);
            }

            // Done
            return result.ToString();
        }

        private long FindPathToNine(CellInfo refCell, bool countAllTrails)
        {
            // End reached
            if (refCell.Value.ToInt() == 9)
            {
                if (refCell.Tag == null || countAllTrails)
                {
                    refCell.Tag = true;
                    return 1;
                }
                return 0;
            }

            // Scan each cells around
            int nextValue = refCell.Value.ToInt() + 1;
            long result = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (Math.Abs(x) == Math.Abs(y))
                    {
                        continue;
                    }
                    CellInfo nextCell = map.Cell(refCell.Position.Add(new Point(x, y)));
                    if (!nextCell.IsValid || nextCell.Value.ToInt() != nextValue)
                    {
                        continue;
                    }
                    result += FindPathToNine(nextCell, countAllTrails);
                }
            }

            // Done
            return result;
        }

        private void GetData()
        {
            map = new(_rawData);
        }
    }
}