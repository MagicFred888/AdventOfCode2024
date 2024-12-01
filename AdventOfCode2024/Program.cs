using AdventOfCode2024.Solver;

namespace AdventOfCode2024;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        int currentDay = DateTime.Now.Day;
        bool currentDayIsSolved = false;

        do
        {
            // For fun
            // Thanks to https://patorjk.com/software/taag/#p=display&v=3&f=Big&t=Type%20Something%20
            Console.Clear();
            Console.WriteLine(@"                           _                 _    ____   __  _____          _        ___   ___ ___  _  _    ");
            Console.WriteLine(@"                  /\      | |               | |  / __ \ / _|/ ____|        | |      |__ \ / _ \__ \| || |   ");
            Console.WriteLine(@"                 /  \   __| |_   _____ _ __ | |_| |  | | |_| |     ___   __| | ___     ) | | | | ) | || |_  ");
            Console.WriteLine(@"                / /\ \ / _` \ \ / / _ \ '_ \| __| |  | |  _| |    / _ \ / _` |/ _ \   / /| | | |/ /|__   _| ");
            Console.WriteLine(@"               / ____ \ (_| |\ V /  __/ | | | |_| |__| | | | |___| (_) | (_| |  __/  / /_| |_| / /_   | |   ");
            Console.WriteLine(@"              /_/    \_\__,_| \_/ \___|_| |_|\__|\____/|_|  \_____\___/ \__,_|\___| |____|\___/____|  |_|   ");
            Console.WriteLine("");

            // Pre-select day (to save time during challenge)
            int day = -1;
            if (DateTime.Now.Year == 2024 && DateTime.Now.Month == 12) day = DateTime.Now.Day;
            if (day == currentDay && currentDayIsSolved) day = -2;

            // Create solver object
            BaseSolver? solver = null;
            do
            {
                Type? targetType = Type.GetType($"AdventOfCode2024.Solver.Day{day:00}");
                if (targetType != null)
                {
                    object? newInstance = null;
                    try
                    {
                        newInstance = Activator.CreateInstance(targetType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Oups...{ex.Message}");
                        Console.WriteLine("");
                    }
                    if (newInstance != null)
                    {
                        solver = (BaseSolver)newInstance;
                    }
                }
                else if (day != -2)
                {
                    Console.WriteLine($"Unable to find solver for day {day:00}!");
                    Console.WriteLine("");
                }

                if (solver == null)
                {
                    Console.Write("Please select day challenge you want: ");
                    string? input = Console.ReadLine();
                    day = input != null && Microsoft.VisualBasic.Information.IsNumeric(input) ? int.Parse(input) : -1;
                    if (day == -1) return;
                }
            } while (solver == null);

            // Solve all
            do
            {
                // Sample 1
                Console.WriteLine("");
                Console.WriteLine("Solving sample 1:");
                bool status = solver.SolveSample(RoundId.FirstRound, out string[] answers);
                Console.WriteLine($"--> {string.Join("\r\n--> ", answers)}");
                if (!status) break;

                // Challenge 1
                Console.WriteLine("");
                Console.WriteLine("Solving challenge 1:");
                status = solver.SolveChallenge(RoundId.FirstRound, out string answer);
                Console.WriteLine($"--> {answer}");
                if (!status) break;

                // Sample 2
                Console.WriteLine("");
                Console.WriteLine("Solving sample 2:");
                status = solver.SolveSample(RoundId.SecondRound, out answers);
                Console.WriteLine($"--> {string.Join("\r\n--> ", answers)}");
                if (!status) break;

                // Challenge 2
                Console.WriteLine("");
                Console.WriteLine("Solving challenge 2:");
                status = solver.SolveChallenge(RoundId.SecondRound, out answer);
                Console.WriteLine($"--> {answer}");
                if (!status) break;

                // All solved for today, let ask user choose another day
                if (day == currentDay) currentDayIsSolved = true;
            } while (false);

            // Wait to quit
            Console.WriteLine("");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        } while (true);
    }
}