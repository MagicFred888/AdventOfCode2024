namespace AdventOfCode2024.Solver
{
    internal partial class Day23() : BaseSolver
    {
        public override string PuzzleTitle { get; } = "LAN Party";

        private sealed class Computer(string name)
        {
            public string Name { get; init; } = name;
            public List<Computer> Computers { get; set; } = [];
        }

        private List<Computer> allComputers = [];

        public override string GetSolution1()
        {
            ExtractData();

            // Search all computers linked by 3
            List<string> result = [];
            foreach (Computer computer in allComputers)
            {
                List<Computer> lan = computer.Computers;
                for (int i = 0; i < lan.Count; i++)
                {
                    for (int j = i + 1; j < lan.Count; j++)
                    {
                        if (lan[i].Computers.Contains(lan[j]))
                        {
                            List<string> listOfComputers = [computer.Name, lan[i].Name, lan[j].Name];
                            listOfComputers.Sort();
                            string key = string.Join("-", listOfComputers);
                            if (!result.Contains(key))
                            {
                                result.Add(key);
                            }
                        }
                    }
                }
            }

            // Keep only the one with at least one machine starting with t
            result = result.FindAll(c => ("-" + c).Contains("-t"));

            // Done
            return result.Count.ToString();
        }

        public override string GetSolution2()
        {
            ExtractData();

            // Search a computer with most links
            int maxLinks = 0;
            Computer? maxComputer = null;
            foreach (Computer computer in allComputers)
            {
                int nbrOfLinks = 0;
                List<Computer> lan = computer.Computers;
                for (int i = 0; i < lan.Count; i++)
                {
                    Computer computer1 = lan[i];
                    for (int j = i + 1; j < lan.Count; j++)
                    {
                        Computer computer2 = lan[j];
                        if (computer1.Computers.Contains(computer2))
                        {
                            nbrOfLinks++;
                        }
                    }
                }
                if (nbrOfLinks > maxLinks)
                {
                    maxLinks = nbrOfLinks;
                    maxComputer = computer;
                }
            }

            // Rebuild the list and remove computers who may not be part of the network
            List<string> allComputer = maxComputer?.Computers.ConvertAll(c => c.Name) ?? throw new InvalidDataException();
            allComputer.Add(maxComputer.Name);
            allComputer.Sort();

            // Create a dictionnary with number of link for each computer
            int max;
            Dictionary<string, int> computersLinksDic = [];
            foreach (Computer computer in maxComputer.Computers)
            {
                max = computer.Computers.FindAll(c => allComputer.Contains(c.Name)).Count;
                computersLinksDic.Add(computer.Name, max);
            }

            // Get best value and keep only the one with the same value
            max = computersLinksDic.Values.Max();
            allComputer = [];
            allComputer.Add(maxComputer.Name);
            foreach (KeyValuePair<string, int> kvp in computersLinksDic)
            {
                if (kvp.Value == max) allComputer.Add(kvp.Key);
            }

            // Sort and join
            allComputer.Sort();
            return string.Join(",", allComputer);
        }

        private void ExtractData()
        {
            Dictionary<string, Computer> tmp = [];
            foreach (string link in _rawData)
            {
                string[] names = link.Split('-');
                foreach (string name in names)
                {
                    if (!tmp.ContainsKey(name))
                    {
                        tmp.Add(name, new Computer(name));
                    }
                }
                tmp[names[0]].Computers.Add(tmp[names[1]]);
                tmp[names[1]].Computers.Add(tmp[names[0]]);
            }
            allComputers = [.. tmp.Values];
        }
    }
}