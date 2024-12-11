using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Solver
{
    internal partial class Day11() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Plutonian Pebbles";

        private Dictionary<(long, long), long> hashDic = [];
        private List<long> stones = [];

        public override string GetSolution1()
        {
            LoadData();

            // Compute nbr of stones
            long result = 0;
            foreach (long stone in stones)
            {
                result += ComputeNbrOfStones(stone, 25);
            }
            return result.ToString();
        }

        public override string GetSolution2()
        {
            // Compute nbr of stones
            long result = 0;
            foreach (long stone in stones)
            {
                result += ComputeNbrOfStones(stone, 75);
            }
            return result.ToString();
        }

        private long ComputeNbrOfStones(long stone, int nbrOfExtraRound)
        {
            // Last step ?
            if (nbrOfExtraRound == 0)
            {
                return 1;
            }

            // Pre-computed ?
            if (hashDic.ContainsKey((stone, nbrOfExtraRound)))
            {
                return hashDic[(stone, nbrOfExtraRound)];
            }

            // Split according to rules
            List<long> newStones = [];
            if (stone == 0)
            {
                newStones.Add(1);
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                string stoneStr = stone.ToString();
                newStones.Add(long.Parse(stoneStr[..(stoneStr.Length / 2)]));
                newStones.Add(long.Parse(stoneStr[(stoneStr.Length / 2)..]));
            }
            else
            {
                newStones.Add(stone * 2024);
            }

            // Compute branch
            long stoneResult = 0;
            foreach (long subStone in newStones)
            {
                stoneResult += ComputeNbrOfStones(subStone, nbrOfExtraRound - 1);
            }
            hashDic.Add((stone, nbrOfExtraRound), stoneResult);

            // Done
            return stoneResult;
        }

        private void LoadData()
        {
            stones = new QuickMatrix(_rawData, " ", true).RowsInt[0].ConvertAll(v => (long)v);
            hashDic = [];
        }
    }
}