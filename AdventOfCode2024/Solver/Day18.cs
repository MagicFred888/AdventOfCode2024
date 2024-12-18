using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day18() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "RAM Run";

        private List<Point> corruption = [];
        private QuickMatrix maze = new();

        private sealed class CellTag(int score, bool partOfBest)
        {
            public int Score { get; set; } = score;
            public bool PartOfBest { get; set; } = partOfBest;
        }

        public override string GetSolution1()
        {
            LoadData(1);

            // Scan maze
            Point startPosition = new(0, 0);
            Point direction = new(1, 0);
            return SolveMaze(startPosition, direction, 0, -1).ToString();
        }

        public override string GetSolution2()
        {
            LoadData(2);

            // Scan maze removing corrupted blocks one by one until we found a path
            Point startPosition = new(0, 0);
            Point direction = new(1, 0);
            for (int i = corruption.Count - 1; i >= 0; i--)
            {
                maze.Cell(corruption[i]).Value = ".";
                maze.ClearAllTags();
                int result = SolveMaze(startPosition, direction, 0, -1);
                if (result != int.MaxValue)
                {
                    return $"{corruption[i].X},{corruption[i].Y}";
                }
            }

            // Oups
            return "Not found !";
        }

        private int SolveMaze(Point position, Point direction, int Score, int bestScore)
        {
            CellInfo currentCell = maze.Cell(position);

            // Are we in a wall ?
            if (!currentCell.IsValid || currentCell.Value == "#")
            {
                return int.MaxValue;
            }

            // Already visited ?
            if (currentCell.Tag != null && Score >= ((CellTag)currentCell.Tag).Score - 1)
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
            possibleScore.Add(SolveMaze(position.Add(newDirection), newDirection, Score + 1, bestScore));

            // Turn left
            newDirection = direction.RotateCounterclockwise(new());
            possibleScore.Add(SolveMaze(position.Add(newDirection), newDirection, Score + 1, bestScore));

            // Update part of the path
            int best = possibleScore.Min();
            if (best <= bestScore)
            {
                currentCell.Tag = new CellTag(Score, true);
            }

            // Done
            return best;
        }

        private void LoadData(int stepId)
        {
            int mazeSize = _rawData.Count < 100 ? 7 : 71;
            int corruptedBlockToLoad = stepId switch
            {
                1 => _rawData.Count < 100 ? 12 : 1024,
                _ => int.MaxValue,
            };

            // Load corruption infos
            corruption = QuickList.ListOfListInt(_rawData, ",").ConvertAll(i => new Point(i[0], i[1]));

            // Load maze
            maze = new(mazeSize, mazeSize, ".");
            corruption.GetRange(0, Math.Min(corruptedBlockToLoad, corruption.Count)).ForEach(c => maze.Cell(c).Value = "#");
            maze.Cell(0, 0).Value = "S";
            maze.Cell(mazeSize - 1, mazeSize - 1).Value = "E";
        }
    }
}