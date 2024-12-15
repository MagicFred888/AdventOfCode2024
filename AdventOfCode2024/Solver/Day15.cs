using AdventOfCode2024.Extensions;
using AdventOfCode2024.Tools;
using System.Drawing;

namespace AdventOfCode2024.Solver
{
    internal partial class Day15() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Warehouse Woes";

        private QuickMatrix warehouse = new();
        private List<Point> allMoves = [];

        public override string GetSolution1()
        {
            LoadData(false);

            // Make moves
            Point robotPos = warehouse.Cells.Find(c => c.Value == "@")?.Position ?? throw new InvalidDataException("Robot not found !");
            foreach (Point move in allMoves)
            {
                robotPos = MoveRobot([robotPos], move, true) ? robotPos.Add(move) : robotPos;
            }
            return ComputeWarehouseGpsScore("O").ToString();
        }

        public override string GetSolution2()
        {
            LoadData(true);

            // Make moves
            Point robotPos = warehouse.Cells.Find(c => c.Value == "@")?.Position ?? throw new InvalidDataException("Robot not found !");
            foreach (Point move in allMoves)
            {
                robotPos = MoveRobot([robotPos], move, true) ? robotPos.Add(move) : robotPos;
            }
            return ComputeWarehouseGpsScore("[").ToString();
        }

        private bool MoveRobot(List<Point> movePos, Point move, bool executeTheMove)
        {
            // Extend movePos if needed
            List<Point> newMovePos = [];
            if (move == new Point(0, 1) || move == new Point(0, -1))
            {
                // Vertical, we may need check multiple column
                foreach (Point pos in movePos)
                {
                    string next = warehouse.Cell(pos.Add(move)).Value;
                    if (next == "[")
                    {
                        newMovePos.Add(pos.Add(move));
                        newMovePos.Add(pos.Add(move).Add(1, 0));
                    }
                    else if (next == "]")
                    {
                        newMovePos.Add(pos.Add(move));
                        newMovePos.Add(pos.Add(move).Add(-1, 0));
                    }
                    else if (next == "O")
                    {
                        newMovePos.Add(pos.Add(move));
                    }
                    else if (next == "#")
                    {
                        return false;
                    }
                }
            }
            else
            {
                // Horizontal move, easy we always stay in 1 line
                string next = warehouse.Cell(movePos[0].Add(move)).Value;
                if (next == "#")
                {
                    return false;
                }
                if (next != ".")
                {
                    newMovePos.Add(movePos[0].Add(move));
                }
            }

            // Check if move is possible without doing it (just a check)
            foreach (Point testMove in newMovePos)
            {
                if (!MoveRobot([testMove], move, false))
                {
                    return false;
                }
            }

            // Execute the move if requested
            if (executeTheMove)
            {
                // Move can be executed
                foreach (Point testMove in newMovePos)
                {
                    _ = MoveRobot([testMove], move, true);
                }
                foreach (Point tmpMovePos in movePos)
                {
                    warehouse.Cell(tmpMovePos.Add(move)).Value = warehouse.Cell(tmpMovePos).Value;
                    warehouse.Cell(tmpMovePos).Value = ".";
                }
            }

            // Move ok
            return true;
        }

        private long ComputeWarehouseGpsScore(string boxTopLeftCorner)
        {
            // Compute GPS score
            long result = 0;
            var positions = warehouse.Cells?.FindAll(c => c.Value == boxTopLeftCorner).Select(info => info.Position) ?? [];
            foreach (Point position in positions)
            {
                result += (100 * position.Y) + position.X;
            }
            return result;
        }

        private void LoadData(bool extendHorizontaly)
        {
            // Raw data split
            int inputTypeSplitPosition = _rawData.FindIndex(d => d.Trim() == string.Empty);

            // Extract warehouse map
            if (extendHorizontaly)
            {
                List<string> newList = [];
                foreach (string line in _rawData.GetRange(0, inputTypeSplitPosition))
                {
                    string newLine = line.Aggregate("", (newLine, currentChar) => newLine + currentChar switch
                    {
                        '#' => "##",
                        '.' => "..",
                        'O' => "[]",
                        '@' => "@.",
                        _ => throw new InvalidDataException("Unknown character")
                    });
                    newList.Add(newLine);
                }
                warehouse = new(newList);
            }
            else
            {
                warehouse = new(_rawData.GetRange(0, inputTypeSplitPosition));
            }

            // Extract moves
            Dictionary<string, Point> moveDir = new()
            {
                {"v", new Point(0,1) },
                {"^", new Point(0,-1) },
                {"<", new Point(-1,0) },
                {">", new Point(1,0) }
            };
            string allMoveString = string.Join("", _rawData.GetRange(inputTypeSplitPosition + 1, _rawData.Count - inputTypeSplitPosition - 1));
            allMoves = allMoveString.Aggregate(new List<Point>(), (allMove, currentMove) =>
            {
                allMove.Add(moveDir[currentMove.ToString()]);
                return allMove;
            });
        }
    }
}