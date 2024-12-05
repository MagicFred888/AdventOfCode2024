using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Solver
{
    internal partial class Day05() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Print Queue";

        List<List<int>> manuals = [];
        List<List<int>> rules = [];

        public override string GetSolution1()
        {
            GetData();
            return manuals.Aggregate(0, (acc, manual) => acc + (ManualIsOk(manual) ? manual[(manual.Count - 1) / 2] : 0)).ToString();
        }

        public override string GetSolution2()
        {
            GetData();
            return manuals.Aggregate(0, (acc, manual) => acc + (!ManualIsOk(manual) ? GetOrderedManual(manual)[(manual.Count - 1) / 2] : 0)).ToString();
        }

        private List<int> GetOrderedManual(List<int> manual)
        {
            List<int> result = new(manual);
            List<List<int>> validRules = rules.FindAll(r => manual.Contains(r[0]) && manual.Contains(r[1]));

            // Order the items within the manual
            bool changed;
            do
            {
                changed = false;
                for (int i = 0; i < validRules.Count; i++)
                {
                    int firstIndex = result.IndexOf(validRules[i][0]);
                    int secondIndex = result.IndexOf(validRules[i][1]);
                    if (firstIndex > secondIndex)
                    {
                        result.RemoveAt(firstIndex);
                        result.Insert(secondIndex, validRules[i][0]);
                        changed = true;
                    }
                }
            } while (changed);

            // Done
            return result;
        }

        private bool ManualIsOk(List<int> manual)
        {
            int nbrOk = 0;
            foreach (List<int> rule in rules)
            {
                int leftPos = manual.FindIndex(p => p == rule[0]);
                int rightPos = manual.FindIndex(p => p == rule[1]);

                if (leftPos == -1 || rightPos == -1)
                {
                    nbrOk++;
                    continue;
                }

                if (leftPos > rightPos)
                    break;

                nbrOk++;
            }
            return nbrOk == rules.Count;
        }

        private void GetData()
        {
            int splitIndex = _rawData.IndexOf("");
            rules = QuickList.ListOfListInt(_rawData.GetRange(0, splitIndex), "|");
            manuals = QuickList.ListOfListInt(_rawData.GetRange(splitIndex + 1, _rawData.Count - splitIndex - 1), ",");
        }
    }
}