namespace AdventOfCode2024.Solver
{
    internal partial class Day09() : BaseSolver()
    {
        public override string PuzzleTitle { get; } = "Disk Fragmenter";

        private List<(List<int> file, List<int> freeSpace)> disk = [];

        public override string GetSolution1()
        {
            GetData();

            // Convert as array
            List<int> diskArray = disk.Aggregate(new List<int>(), (acc, block) =>
            {
                acc.AddRange(block.file);
                acc.AddRange(block.freeSpace);
                return acc;
            });

            // Move files
            for (int i = diskArray.Count - 1; i >= 0; i--)
            {
                if (diskArray[i] == -1) continue;
                int newPos = diskArray.IndexOf(-1);
                if (newPos >= i)
                {
                    break;
                }
                diskArray[diskArray.IndexOf(-1)] = diskArray[i];
                diskArray[i] = -1;
            }

            // Done
            return ValueFromDisk(diskArray).ToString();
        }

        public override string GetSolution2()
        {
            GetData();

            // Move files
            for (int lastPosition = disk.Count - 1; lastPosition >= 0; lastPosition--)
            {
                int lastFileSize = disk[lastPosition].file.Count;
                for (int freePos = 0; freePos < lastPosition; freePos++)
                {
                    if (disk[freePos].freeSpace.Count(i => i == -1) >= lastFileSize)
                    {
                        int freeSubPos = disk[freePos].freeSpace.IndexOf(-1);
                        for (int writePos = 0; writePos < lastFileSize; writePos++)
                        {
                            disk[freePos].freeSpace[freeSubPos + writePos] = disk[lastPosition].file[writePos];
                            disk[lastPosition].file[writePos] = -1;
                        }
                        break;
                    }
                }
            }

            // Convert as array
            List<int> diskArray = disk.Aggregate(new List<int>(), (acc, block) =>
            {
                acc.AddRange(block.file);
                acc.AddRange(block.freeSpace);
                return acc;
            });

            // Done
            return ValueFromDisk(diskArray).ToString();
        }

        private static long ValueFromDisk(List<int> diskArray)
        {
            long result = 0;
            for (int pos = 0; pos < diskArray.Count; pos++)
            {
                if (diskArray[pos] == -1) continue;
                result += (long)pos * (long)diskArray[pos];
            }
            return result;
        }

        private void GetData()
        {
            disk = [];
            string data = _rawData[0] + "0";
            for (int pos = 0; pos < data.Length; pos += 2)
            {
                int fileSize = int.Parse(data.Substring(pos, 1));
                List<int> file = Enumerable.Repeat(pos / 2, fileSize).ToList();
                int freeSize = int.Parse(data.Substring(pos + 1, 1));
                List<int> freeSpace = Enumerable.Repeat(-1, freeSize).ToList();
                disk.Add((file, freeSpace));
            }
        }
    }
}