using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;
using static AdventOfCode2024.Tools.QuickMatrix;

namespace AdventOfCode2024.Solver
{
    internal partial class Day20() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Race Condition";

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
            return GetNbrBetterThanBest(100, 2).ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Scan maze
            return GetNbrBetterThanBest(100, 20).ToString();
        }

        private long GetNbrBetterThanBest(long scoreImprovement, long cheatManhattanDistance)
        {
            // Very bad solution but it works even if very slow... around 60s :-(((
            // Scan maze
            Point direction = new(-1, 0);
            Dictionary<Point, long> distanceToExit = [];
            foreach (Point pos in maze.Cells.FindAll(c => c.Value != "#").ConvertAll(c => c.Position))
            {
                maze.ClearAllTags();
                distanceToExit.Add(pos, SolveMazeIterative(pos, direction, "E"));
            }

            // Get base time
            maze.ClearAllTags();
            Point startPosition = maze.Cells.Find(c => c.Value == "S")?.Position ?? throw new InvalidDataException("Unable to find S pos");
            long baseTime = SolveMazeIterative(startPosition, direction, "E");

            // Check each possible cheat
            long answer = 0;
            foreach (Point cheatStart in distanceToExit.Keys)
            {
                maze.ClearAllTags();
                long distToCheatStart = SolveMazeIterative(cheatStart, direction, "S");
                for (int manhattanDistance = 2; manhattanDistance <= cheatManhattanDistance; manhattanDistance++)
                {
                    foreach (Point cheatEnd in maze.CellsAtManhattanDistance(cheatStart, manhattanDistance).FindAll(c => c.Value != "#").ConvertAll(c => c.Position))
                    {
                        long newTime = distToCheatStart + manhattanDistance + distanceToExit[cheatEnd];
                        if (baseTime - newTime >= scoreImprovement) answer++;
                    }
                }
            }
            return answer;
        }

        private long SolveMazeIterative(Point startPosition, Point startDirection, string targetCellValue)
        {
            bool okToReveret = true;
            Stack<(Point position, Point direction, int score)> stack = new();
            stack.Push((startPosition, startDirection, 0));
            int bestScore = int.MaxValue;

            while (stack.Count > 0)
            {
                var (position, direction, score) = stack.Pop();

                // Get cell info
                CellInfo currentCell = maze.Cell(position);

                // Are we in a wall?
                if (!currentCell.IsValid || currentCell.Value == "#")
                {
                    continue;
                }

                // Already visited?
                if (currentCell.Tag != null && score >= ((CellTag)currentCell.Tag).Score - 1)
                {
                    continue;
                }

                // Are we at the end?
                if (currentCell.Value == targetCellValue)
                {
                    currentCell.Tag = new CellTag(score, true);
                    bestScore = Math.Min(bestScore, score);
                    continue;
                }
                else
                {
                    currentCell.Tag = new CellTag(score, currentCell.Tag != null && ((CellTag)currentCell.Tag).PartOfBest);
                }

                // Continue ahead
                stack.Push((position.Add(direction), direction, score + 1));

                // Turn right
                Point newDirection = direction.RotateClockwise(new());
                stack.Push((position.Add(newDirection), newDirection, score + 1));

                // Turn left
                newDirection = direction.RotateCounterclockwise(new());
                stack.Push((position.Add(newDirection), newDirection, score + 1));

                // Move back
                if (okToReveret)
                {
                    okToReveret = false;
                    newDirection = direction.Rotate180Degree(new());
                    stack.Push((position.Add(newDirection), newDirection, score + 1));
                }
            }

            return bestScore;
        }

        private void LoadData()
        {
            maze = new(_rawData);
            List<Point> walls = [];
            walls.AddRange(maze.Cols[0].ConvertAll(c => c.Position));
            walls.AddRange(maze.Cols[^1].ConvertAll(c => c.Position));
            walls.AddRange(maze.Rows[0].ConvertAll(c => c.Position));
            walls.AddRange(maze.Rows[^1].ConvertAll(c => c.Position));
        }
    }
}