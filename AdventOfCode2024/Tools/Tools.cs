using System.Diagnostics;
using System.Text;

namespace AdventOfCode2024.Tools
{
    public static class Tools
    {
        /// <summary>
        /// Calculates the Least Common Multiple (LCM) of an array of numbers.
        /// </summary>
        /// <param name="numbers">An array of long integers.</param>
        /// <returns>The LCM of the input numbers.</returns>
        public static long LCM(long[] numbers)
        {
            return numbers.Aggregate((long x, long y) => x * y / GCD(x, y));
        }

        /// <summary>
        /// Calculates the Greatest Common Divisor (GCD) of two numbers using the Euclidean algorithm.
        /// </summary>
        /// <param name="a">The first long integer.</param>
        /// <param name="b">The second long integer.</param>
        /// <returns>The GCD of the two input numbers.</returns>
        public static long GCD(long a, long b)
        {
            if (b == 0) return a;
            return GCD(b, a % b);
        }

        /// <summary>
        /// Prints a 2D array of objects to the debug output, converting values using a dictionary if provided.
        /// </summary>
        /// <param name="tmpTable">The 2D array of objects to print.</param>
        /// <param name="convDic">A dictionary for converting object values to strings.</param>
        /// <param name="missing">The string to use if a value is missing in the dictionary.</param>
        public static void DebugPrint(object[,] tmpTable, Dictionary<string, string> convDic, string missing)
        {
            // To visualize Matrix
            Debug.WriteLine("");
            for (int y = 0; y <= tmpTable.GetUpperBound(1); y++)
            {
                StringBuilder line = new();
                for (int x = 0; x <= tmpTable.GetUpperBound(0); x++)
                {
                    string? tmpVal = tmpTable[x, y] == null ? "." : tmpTable[x, y].ToString();

                    if (tmpVal == null)
                    {
                        line.Append("NULL");
                    }
                    else if (convDic == null)
                    {
                        line.Append(tmpVal);
                    }
                    else
                    {
                        if (convDic.TryGetValue(tmpVal, out string? value))
                        {
                            line.Append(value);
                        }
                        else
                        {
                            line.Append(missing);
                        }
                    }
                }
                Debug.WriteLine(line.ToString());
            }
        }

        /// <summary>
        /// Prints a 2D array of characters to the debug output.
        /// </summary>
        /// <param name="tmpTable">The 2D array of characters to print.</param>
        public static void DebugPrint(char[,] tmpTable)
        {
            // To visualize Matrix
            Debug.WriteLine("");
            for (int y = 0; y <= tmpTable.GetUpperBound(1); y++)
            {
                StringBuilder line = new();
                for (int x = 0; x <= tmpTable.GetUpperBound(0); x++)
                {
                    line.Append(tmpTable[x, y]);
                }
                Debug.WriteLine(line.ToString());
            }
        }
    }
}