namespace AdventOfCode2024.Extensions
{
    public static class StringExtensions
    {
        public static long ToLong(this string str)
        {
            return long.Parse(str);
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }
}