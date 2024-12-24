namespace AdventOfCode2024.Solver
{
    internal partial class Day24() : BaseSolver
    {
        public override string PuzzleTitle { get; } = "Crossed Wires";

        private enum GateType
        {
            AND = 0,
            OR = 1,
            XOR = 2,
        }

        private enum WireState
        {
            Unknow = -1,
            False = 0,
            True = 1,
        }

        private sealed class Wire(string name, WireState state)
        {
            public string Name { get; set; } = name;
            public WireState State { get; set; } = state;
            public List<Gate> Connections { get; set; } = [];
        }

        private sealed class Gate(int id, GateType type, Wire input1, Wire input2, Wire output)
        {
            public int Id { get; init; } = id;
            public GateType Type { get; init; } = type;
            public Wire Input1 { get; set; } = input1;
            public Wire Input2 { get; set; } = input2;
            public Wire Output { get; set; } = output;

            public void Compute()
            {
                if (Input1.State == WireState.Unknow || Input2.State == WireState.Unknow)
                {
                    Output.State = WireState.Unknow;
                }
                else
                {
                    Output.State = (Type) switch
                    {
                        GateType.AND => Input1.State == WireState.True && Input2.State == WireState.True ? WireState.True : WireState.False,
                        GateType.OR => Input1.State == WireState.True || Input2.State == WireState.True ? WireState.True : WireState.False,
                        GateType.XOR => Input1.State != Input2.State ? WireState.True : WireState.False,
                        _ => throw new NotImplementedException(),
                    };
                }
            }
        }

        private readonly Dictionary<string, Wire> allWire = [];
        private readonly List<Gate> allGates = [];

        public override string GetSolution1()
        {
            ExtractData();

            while (allWire.Any(w => w.Value.State == WireState.Unknow))
            {
                foreach (Gate gate in allGates)
                {
                    gate.Compute();
                }
            }

            // Convert binary into long
            return GetNumberFromWires("z").ToString();
        }

        public override string GetSolution2()
        {
            /*
             * THANKS TO markG200005 on https://www.reddit.com/r/adventofcode/comments/1hl698z/2024_day_24_solutions/
             *
             * The key insight is that the circuit is trying to perform binary addition between x and y inputs. The gates are swapped in pairs, causing incorrect outputs. The solution:
             *
             * FIRST: looks for z-wire gates that should be XOR gates (for addition) but aren't
             *
             * SECOND: checks internal gates that incorrectly feed into XOR operations
             *
             * THIRD: For gates with x/y inputs:
             *  - XOR gates should feed into other XOR gates (for addition)
             *  - AND gates should feed into OR gates (for carry propagation)
             *
             * First bit (00) gates are handled separately as they start the carry chain
            */

            ExtractData();

            // Search for faulty gates
            List<Gate> gateWithError = [];
            List<Wire> inputWire = allWire.Values.ToList().FindAll(w => w.Name.StartsWith('x') || w.Name.StartsWith('y'));
            Gate resultGates = allGates.Where(g => g.Output.Name.StartsWith('z')).OrderByDescending(g => g.Output.Name).First();

            // Scan each
            foreach (Gate gate in allGates)
            {
                bool isFaulty = false;
                if (gate.Output.Name.StartsWith('z') && gate.Output != resultGates.Output)
                {
                    isFaulty = gate.Type != GateType.XOR;
                }
                else if (!gate.Output.Name.StartsWith('z') && !inputWire.Contains(gate.Input1) && !inputWire.Contains(gate.Input2))
                {
                    isFaulty = gate.Type == GateType.XOR;
                }
                else if (inputWire.Contains(gate.Input1) && inputWire.Contains(gate.Input2) && !AreInputsFirstBit(gate.Input1.Name, gate.Input2.Name))
                {
                    Wire outputWire = gate.Output;
                    GateType expectedNextGateType = gate.Type == GateType.XOR ? GateType.XOR : GateType.OR;
                    isFaulty = !allGates.Any(other => other != gate && (other.Input1 == outputWire || other.Input2 == outputWire) && other.Type == expectedNextGateType);
                }
                if (isFaulty)
                {
                    gateWithError.Add(gate);
                }
            }

            // Done
            return string.Join(",", gateWithError.ConvertAll(g => g.Output.Name).OrderBy(n => n));
        }

        private static bool AreInputsFirstBit(string input1, string input2)
        {
            return input1.EndsWith("00") && input2.EndsWith("00");
        }

        private long GetNumberFromWires(string wireCode)
        {
            List<Wire> result = allWire.Values.ToList().FindAll(w => w.Name.StartsWith(wireCode));
            result.Sort((a, b) => b.Name.CompareTo(a.Name));
            return Convert.ToInt64(result.Aggregate("", (acc, w) => acc + (w.State == WireState.True ? "1" : "0")), 2);
        }

        private void ExtractData()
        {
            // Extract all wires
            Dictionary<string, WireState> wireInfo = [];
            foreach (string data in _rawData)
            {
                if (data.Contains(':'))
                {
                    string[] parts = [.. data.Split(':').ToList().ConvertAll(x => x.Trim())];
                    wireInfo.Add(parts[0], parts[1] == "1" ? WireState.True : WireState.False);
                }
                if (data.Contains("->"))
                {
                    string[] parts = [.. data.Split("->").ToList().ConvertAll(x => x.Trim())];
                    wireInfo.TryAdd(parts[1], WireState.Unknow);
                }
            }

            // Create all wires
            allWire.Clear();
            foreach (KeyValuePair<string, WireState> kvp in wireInfo)
            {
                allWire.Add(kvp.Key, new(kvp.Key, kvp.Value));
            }

            // Create all gates
            allGates.Clear();
            int gateId = 0;
            foreach (string data in _rawData.FindAll(l => l.Contains("->")))
            {
                string output = data.Split("->")[1].Trim();
                string[] parts = data.Split("->")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                allGates.Add(new Gate(gateId, (GateType)Enum.Parse(typeof(GateType), parts[1]), allWire[parts[0]], allWire[parts[2]], allWire[output]));
                gateId++;
            }
        }
    }
}