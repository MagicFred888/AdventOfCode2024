using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day16() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Reindeer Maze";

        private QuickMatrix maze = new();

        private sealed class CellTag(int score, bool partOfBest)
        {
            public int Score { get; set; } = score;
            public bool PartOfBest { get; set; } = partOfBest;
        }

        public override string GetSolution1()
        {
            LoadData();

            // Scan maze
            Point startPosition = maze.Cells.Find(c => c.Value == "S")?.Position ?? throw new InvalidDataException("Start not found !");
            Point direction = new(1, 0);
            return SolveMaze(startPosition, direction, 0, -1).ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Scan maze
            Point startPosition = maze.Cells.Find(c => c.Value == "S")?.Position ?? throw new InvalidDataException("Start not found !");
            Point direction = new(1, 0);
            int bestScore = SolveMaze(startPosition, direction, 0, -1);
            maze.ClearAllTags();
            _ = SolveMaze(startPosition, direction, 0, bestScore); // Dirty to do double search but too tired to keep improving...
            return maze.Cells.Count(c => c.Tag != null && ((CellTag)c.Tag).PartOfBest).ToString();
        }

        private int SolveMaze(Point position, Point direction, int Score, int bestScore)
        {
            CellInfo currentCell = maze.Cell(position);

            // Are we in a wall ?
            if (currentCell.Value == "#")
            {
                return int.MaxValue;
            }

            // Already visited ?
            if (currentCell.Tag != null && ((CellTag)currentCell.Tag).Score < Score - 1000) // We remove 1000 in case we can continue in the same direction)
            {
                return int.MaxValue;
            }

            // Are we at the end ?
            if (currentCell.Value == "E")
            {
                // Mark as part of solution
                currentCell.Tag = new CellTag(Score, true);
                return Score;
            }
            else
            {
                // We mark the cell as visited
                currentCell.Tag = new CellTag(Score, currentCell.Tag != null && ((CellTag)currentCell.Tag).PartOfBest);
            }

            // Continue ahead
            List<int> possibleScore = [];
            possibleScore.Add(SolveMaze(position.Add(direction), direction, Score + 1, bestScore));

            // Turn right
            Point newDirection = direction.RotateClockwise(new());
            possibleScore.Add(SolveMaze(position.Add(newDirection), newDirection, Score + 1001, bestScore));

            // Turn left
            newDirection = direction.RotateCounterclockwise(new());
            possibleScore.Add(SolveMaze(position.Add(newDirection), newDirection, Score + 1001, bestScore));

            // Update part of the path
            int best = possibleScore.Min();
            if (best <= bestScore)
            {
                currentCell.Tag = new CellTag(Score, true);
            }

            // Done
            return best;
        }

        private void LoadData()
        {
            maze = new(_rawData);
        }
    }
}