using Spectre.Console;
using ToDoManagerStatistics;

namespace MenuLibrary
{
    /// <summary>
    /// Represents a menu for displaying productivity statistics related to ToDo tasks.
    /// </summary>
    public class ManagerMenuProductivity : ManagerMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerMenuProductivity"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        public ManagerMenuProductivity(string name) : base(name, new string[] { "Most postponed tasks", "Year statistics", "Monthly statistics", "Weekly statistics", "Return" })
        {
            Actions = new Action[] { ShowMostPostponed, ShowYearlyStatistics, ShowMonthlyStatistics, ShowWeeklyStatistics, () => { } };
        }

        /// <summary>
        /// Displays a bar chart of the most postponed tasks.
        /// </summary>
        private void ShowMostPostponed()
        {
            Dictionary<string, int> dict = UserDataManager.CurrentUser.Statistics.GetMostPostponed();
            if (dict.Keys.Count > 0)
            {
                Color[] colors =
                {
                    Color.SandyBrown, Color.LightPink1, Color.IndianRed1, Color.Pink1, Color.DarkSeaGreen2_1
                };

                BarChart chart = new BarChart()
                    .Label("[teal bold underline]Top most postponed tasks:[/]")
                    .Width(40);

                // Display up to the top 5 most postponed tasks
                for (int i = 0; i < (dict.Keys.Count < 5 ? dict.Keys.Count : 5); i++)
                {
                    chart.AddItem(dict.Keys.ElementAt(i), dict.Values.ElementAt(i), colors[i]);
                }

                AnsiConsole.Write(chart);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error: No postponed tasks found.[/]");
            }
        }

        /// <summary>
        /// Displays monthly statistics for ToDo task completion.
        /// </summary>
        private void ShowMonthlyStatistics()
        {
            string resYear = AnsiConsole.Prompt(new TextPrompt<string>("Enter year:"));
            if (int.TryParse(resYear, out int year) && year > 0 && year <= DateTime.Now.Year + 100)
            {
                string resMonth = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose month: ").AddChoices(Enum.GetNames(typeof(Months))));

                DateTime current = new DateTime(year, (int)(Months)Enum.Parse(typeof(Months), resMonth), 1);
                Dictionary<int, int> dates = UserDataManager.CurrentUser.Statistics.MonthStatistics(current);
                AnsiConsole.MarkupLine("[teal bold underline]Monthly statistics:[/]");

                // Create a table to display daily statistics
                Table table = new Table().AddColumn("Day").AddColumn("Completed ToDo").Title(resMonth);
                for (int date = 1; date <= DateTime.DaysInMonth(current.Year, current.Month); date++)
                {
                    int res;
                    dates.TryGetValue(date, out res);

                    table.AddRow(res == 0 ? date.ToString() : "[bold yellow]" + date + "[/]", res.ToString());
                }
                AnsiConsole.Write(table);

                // Calculate and display the average completion rate
                double score = dates.Count != 0 ? dates.Values.Average() : 0;
                AnsiConsole.MarkupLine($"[bold lightseagreen]Average ToDo completion speed for this month is [gold1]{score:f4}.[/][/]");
                AnsiConsole.WriteLine();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error: Year is not valid.[/]");
            }
        }

        /// <summary>
        /// Displays weekly statistics for ToDo task completion.
        /// </summary>
        private void ShowWeeklyStatistics()
        {
            string resYear = AnsiConsole.Prompt(new TextPrompt<string>("Enter year:"));
            string resMonth = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose month: ").AddChoices(Enum.GetNames(typeof(Months))));
            string resDay = AnsiConsole.Prompt(new TextPrompt<string>("Enter day of month:"));

            if (int.TryParse(resYear, out int year) && year > 0 && year <= DateTime.Now.Year + 100)
            {
                DateTime current = new DateTime(year, (int)(Months)Enum.Parse(typeof(Months), resMonth), 1);
                if (int.TryParse(resDay, out int day) && day > 0 &&
                    day <= DateTime.DaysInMonth(current.Year, current.Month))
                {
                    current = new DateTime(current.Year, current.Month, day);
                    BarChart chart = new BarChart().Label("[teal bold underline]Weekly statistics:[/]").Width(40);

                    Color[] weekColors = new Color[] { Color.IndianRed, Color.Orange1, Color.Gold1, Color.Lime,
                        Color.Turquoise2, Color.RoyalBlue1, Color.MediumPurple };

                    int i = 0;
                    Dictionary<DayOfWeek, int> dict = UserDataManager.CurrentUser.Statistics.WeekStatistics(current);
                    foreach (KeyValuePair<DayOfWeek, int> date in dict)
                    {
                        chart.AddItem(date.Key.ToString(), date.Value, weekColors[i++]);
                    }

                    AnsiConsole.Write(chart);

                    // Calculate and display the average completion rate
                    double score = dict.Count != 0 ? dict.Values.Average() : 0;
                    AnsiConsole.MarkupLine($"[bold lightseagreen]Average ToDo completion speed for this week is [gold1]{score:f4}.[/][/]");
                    AnsiConsole.WriteLine();
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Error: Day is not valid.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error: Year is not valid.[/]");
            }
        }

        /// <summary>
        /// Displays yearly statistics for ToDo task completion.
        /// </summary>
        private void ShowYearlyStatistics()
        {
            string res = AnsiConsole.Prompt(new TextPrompt<string>("Enter year:"));
            if (int.TryParse(res, out int year) && year > 0 && year <= DateTime.Now.Year + 100)
            {
                Dictionary<Months, int> dict = UserDataManager.CurrentUser.Statistics.YearStatistics(new DateTime(year, 1, 1));
                BarChart chart = new BarChart().Label("[teal bold underline]Year statistics:[/]").Width(40);

                Color[] colors = new Color[] { Color.LightCoral, Color.Salmon1, Color.LightYellow3, Color.LightGreen, Color.Aquamarine1,
                    Color.LightSkyBlue1, Color.SlateBlue1, Color.MediumPurple, Color.LightPink1, Color.HotPink, Color.Pink1, Color.LightGoldenrod1 };

                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    int result;
                    dict.TryGetValue(month, out result);
                    chart.AddItem(month.ToString(), result, colors[(int)month - 1]);
                }

                AnsiConsole.Write(chart);

                // Calculate and display the average completion rate
                double score = dict.Count != 0 ? dict.Values.Average() : 0;
                AnsiConsole.MarkupLine($"[bold lightseagreen]Average ToDo completion speed for this year is [gold1]{score:f4}.[/][/]");
                AnsiConsole.WriteLine();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error: Year is not valid.[/]");
            }
        }
    }
}