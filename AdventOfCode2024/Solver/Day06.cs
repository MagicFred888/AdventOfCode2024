using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day06() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Guard Gallivant";

        private QuickMatrix map = new();

        public override string GetSolution1()
        {
            GetData();
            _ = ComputeMove();
            return map.Cells.Count(c => c.Tag != null).ToString();
        }

        public override string GetSolution2()
        {
            GetData();

            // Get all block where we can add an obstacle
            _ = ComputeMove();
            List<Point> blocks = map.Cells.Where(c => c.Tag != null).Select(c => c.Position).ToList();
            map.ClearAllTags();
            blocks.Remove(map.Cells.First(c => c.Value == "^").Position);

            // Place block in each position and check if it make a loop (not very efficient)
            int count = 0;
            foreach (Point block in blocks)
            {
                map.Cell(block.X, block.Y).Value = "#";
                if (ComputeMove())
                {
                    count++;
                }
                map.Cell(block.X, block.Y).Value = ".";
                map.ClearAllTags();
            }

            // Done
            return count.ToString();
        }

        private bool ComputeMove()
        {
            // Set initial position and direction
            Point pos = map.Cells.First(c => c.Value == "^").Position;
            Point dir = new(0, -1);

            // Loop until we are trapped in an endless loop or we leave the grid
            do
            {
                // Check if out of the grid
                CellInfo current = map.Cell(pos.X, pos.Y);
                if (current.Position.X < 0)
                {
                    return false;
                }

                // Check if we already visited this cell moving in the same direction
                current.Tag ??= new List<Point>();
                List<Point> dirList = (List<Point>)current.Tag;
                if (dirList.Contains(dir))
                {
                    // Endless loop detected
                    return true;
                }
                else
                {
                    // Record our diretcion
                    dirList.Add(dir);
                }

                // Check if obstacle and move or change direction
                if (map.Cell(pos.X + dir.X, pos.Y + dir.Y).Value == "#")
                {
                    dir = new Point(-dir.Y, dir.X);
                }
                else
                {
                    pos = new Point(pos.X + dir.X, pos.Y + dir.Y);
                }
            } while (true);
        }

        private void GetData()
        {
            map = new(_rawData, "", true);
        }
    }
}