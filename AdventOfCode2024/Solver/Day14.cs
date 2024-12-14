using AdventOfCode2024.Tools;
using System.Drawing;

namespace AdventOfCode2024.Solver
{
    internal partial class Day14() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Restroom Redoubt";

        private readonly List<(Point position, Point speed)> robots = [];

        public override string GetSolution1()
        {
            LoadData();

            // Grid change depending if we are on sample or challenge
            int width = robots.Count > 100 ? 101 : 11;
            int height = robots.Count > 100 ? 103 : 7;

            // Make 100 cycles
            for (int step = 0; step < 100; step++)
            {
                // Move the robots
                for (int robotId = 0; robotId < robots.Count; robotId++)
                {
                    (Point robotPosition, Point robotSpeed) = robots[robotId];
                    Point nextPos = new((robotPosition.X + robotSpeed.X + width) % width, (robotPosition.Y + robotSpeed.Y + height) % height);
                    robots[robotId] = (nextPos, robotSpeed);
                }
            }

            // Calculate the result
            return (robots.Count(r => r.position.X < width / 2 && r.position.Y < height / 2)
                    * robots.Count(r => r.position.X > width / 2 && r.position.Y < height / 2)
                    * robots.Count(r => r.position.X < width / 2 && r.position.Y > height / 2)
                    * robots.Count(r => r.position.X > width / 2 && r.position.Y > height / 2)).ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Grid change depending if we are on sample or challenge
            int width = robots.Count > 100 ? 101 : 11;
            int height = robots.Count > 100 ? 103 : 7;

            // Make cycles until we find the tree, I assum that robot will be touching so will look for 100 touching robots
            int step = 0;
            int maxTouching = 0;
            int touchingRobotToStop = 100;
            do
            {
                // Move the robots
                for (int robotId = 0; robotId < robots.Count; robotId++)
                {
                    (Point robotPosition, Point robotSpeed) = robots[robotId];
                    Point nextPos = new((robotPosition.X + robotSpeed.X + width) % width, (robotPosition.Y + robotSpeed.Y + height) % height);
                    robots[robotId] = (nextPos, robotSpeed);
                }
                step++;

                QuickMatrix bathroom = new(width, height, robots.ConvertAll(r => r.position), "#", " ");
                foreach (Point position in robots.ConvertAll(r => r.position))
                {
                    int touching = bathroom.GetTouchingCellsWithValue(position, TouchingMode.All).Count;
                    if (touching > maxTouching)
                    {
                        maxTouching = touching;
                    }
                }

                if (maxTouching > touchingRobotToStop)
                {
                    bathroom.DebugPrint();
                    return step.ToString();
                }
            } while (true);
        }

        private void LoadData()
        {
            // Parse the data
            robots.Clear();
            foreach (string line in _rawData)
            {
                string[] parts = line.Replace("=", ",").Replace(" ", ",").Split(",");
                robots.Add((new Point(int.Parse(parts[1]), int.Parse(parts[2])), new Point(int.Parse(parts[4]), int.Parse(parts[5]))));
            }
        }
    }
}