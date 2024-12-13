using System.Text.RegularExpressions;

namespace AdventOfCode2024.Solver
{
    internal partial class Day13() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Claw Contraption";

        private readonly List<((long X, long Y) A, (long X, long Y) B, (long X, long Y) Prize)> clawMachines = [];

        public override string GetSolution1()
        {
            LoadData();

            // Calculate cost
            long fullCost = 0;
            foreach (((long X, long Y) A, (long X, long Y) B, (long X, long Y) Prize) in clawMachines)
            {
                (long nbrPressA, long nbrPressB) = ComputeNbrOfPress(A, B, Prize, 0);
                fullCost += 3 * nbrPressA + nbrPressB;
            }
            return fullCost.ToString();
        }

        public override string GetSolution2()
        {
            LoadData();

            // Calculate cost
            long fullCost = 0;
            foreach (((long X, long Y) A, (long X, long Y) B, (long X, long Y) Prize) in clawMachines)
            {
                (long nbrPressA, long nbrPressB) = ComputeNbrOfPress(A, B, Prize, 10000000000000);
                fullCost += 3 * nbrPressA + nbrPressB;
            }
            return fullCost.ToString();
        }

        private static (long nbrPressA, long nbrPressB) ComputeNbrOfPress((long X, long Y) buttonAMove, (long X, long Y) buttonBMove, (long X, long Y) prizeCoordinates, long offset)
        {
            // Add offset if any
            prizeCoordinates = (prizeCoordinates.X + offset, prizeCoordinates.Y + offset);

            // Compute the number of press for A and B by using the formula to solve a two equation system with 2 unknowns
            double nbrPressA = ((double)prizeCoordinates.X * (double)buttonBMove.Y - (double)prizeCoordinates.Y * (double)buttonBMove.X) / ((double)buttonAMove.X * (double)buttonBMove.Y - (double)buttonAMove.Y * (double)buttonBMove.X);
            double nbrPressB = ((double)prizeCoordinates.X - nbrPressA * (double)buttonAMove.X) / (double)buttonBMove.X;

            // Check if the result is an integer within a tolerance range
            const double tolerance = 1e-9;
            if (Math.Abs(nbrPressA - Math.Round(nbrPressA)) > tolerance || Math.Abs(nbrPressB - Math.Round(nbrPressB)) > tolerance)
            {
                return (0, 0);
            }

            // We have a solution
            return ((long)Math.Round(nbrPressA), (long)Math.Round(nbrPressB));
        }

        private void LoadData()
        {
            // Parse the data
            clawMachines.Clear();
            for (int i = 0; i < _rawData.Count; i += 4)
            {
                Match match = RgxMove().Match(_rawData[i]);
                (long X, long Y) buttonAMove = (long.Parse(match.Groups["X"].Value), long.Parse(match.Groups["Y"].Value));
                match = RgxMove().Match(_rawData[i + 1]);
                (long X, long Y) buttonBMove = (long.Parse(match.Groups["X"].Value), long.Parse(match.Groups["Y"].Value));
                match = RgxPrizePosition().Match(_rawData[i + 2]);
                (long X, long Y) prizeLocation = (long.Parse(match.Groups["X"].Value), long.Parse(match.Groups["Y"].Value));
                clawMachines.Add((buttonAMove, buttonBMove, prizeLocation));
            }
        }

        [GeneratedRegex(@"^Button (A|B): X\+(?<X>\d+), Y\+(?<Y>\d+)$")]
        private static partial Regex RgxMove();

        [GeneratedRegex(@"^Prize: X=(?<X>\d+), Y=(?<Y>\d+)$")]
        private static partial Regex RgxPrizePosition();
    }
}