using AdventOfCode2024.Tools;

namespace AdventOfCode2024.Solver
{
    internal partial class Day17() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Chronospatial Computer";

        private List<long> program = [];
        private List<long> buffers = [];

        public override string GetSolution1()
        {
            LoadData();
            return string.Join(",", RunProgram());
        }

        public override string GetSolution2()
        {
            LoadData();

            // Search Thanks to the pattern we found
            long valueRegisterA = 0;
            do
            {
                // Update registers and make calculation
                valueRegisterA++;
                buffers = [valueRegisterA, 0, 0];
                List<long> output = RunProgram();

                // We check if the answer we found is matching the last digits of the program (even if shorter)
                if (valueRegisterA > 0 && program[(program.Count - output.Count)..].SequenceEqual(output))
                {
                    if (output.Count == program.Count)
                    {
                        // We have the solution
                        break;
                    }
                    valueRegisterA = (valueRegisterA * 8) - 1; // => Remove 1 because we will add 1 at the begining of the loop
                }
            } while (true);

            // Done
            return valueRegisterA.ToString();
        }

        private List<long> RunProgram()
        {
            List<long> result = [];
            long programPointerPos = 0;
            do
            {
                // Are we at the end of the program
                if (programPointerPos >= program.Count)
                {
                    return result;
                }

                // Get value for current cycle
                long instruction = program[(int)programPointerPos];
                long operand = program[(int)programPointerPos + 1];

                // Action
                if (instruction == 3)
                {
                    // Compute new position
                    if (buffers[0] == 0)
                    {
                        programPointerPos += 2;
                    }
                    else
                    {
                        programPointerPos = operand;
                    }
                }
                else
                {
                    switch (instruction)
                    {
                        case 0:
                            buffers[0] = buffers[0] / (long)Math.Pow(2, GetComboOperand(operand));
                            break;

                        case 1:
                            buffers[1] = buffers[1] ^ operand;
                            break;

                        case 2:
                            buffers[1] = GetComboOperand(operand) % 8;
                            break;

                        case 4:
                            buffers[1] = buffers[1] ^ buffers[2];
                            break;

                        case 5:
                            result.Add(GetComboOperand(operand) % 8);
                            break;

                        case 6:
                            buffers[1] = buffers[0] / (long)Math.Pow(2, GetComboOperand(operand));
                            break;

                        case 7:
                            buffers[2] = buffers[0] / (long)Math.Pow(2, GetComboOperand(operand));
                            break;

                        default:
                            throw new NotImplementedException();
                    }

                    // Next step
                    programPointerPos += 2;
                }
            } while (true);
        }

        private long GetComboOperand(long operand)
        {
            if (operand > 3)
            {
                return buffers[(int)operand - 4];
            }
            return operand;
        }

        private void LoadData()
        {
            long A = long.Parse(_rawData[0].Split(": ")[1]);
            long B = long.Parse(_rawData[1].Split(": ")[1]);
            long C = long.Parse(_rawData[2].Split(": ")[1]);
            buffers = [A, B, C];
            program = new QuickMatrix([_rawData[4].Split(": ")[1]], ",").Rows[0].ConvertAll(i => long.Parse(i.Value));
        }
    }
}