using Library;
using Spectre.Console;
using ToDoManagerStatistics;

namespace MenuLibrary
{
    /// <summary>
    /// Represents a menu for printing and visualizing ToDo data in various formats, such as tables, calendars, and charts.
    /// </summary>
    public sealed class ManagerMenuPrint : ManagerMenu
    {
        /// <summary>
        /// The file path where the printed results are saved.
        /// </summary>
        public string Filepath = Path.GetFullPath(@"..\..\..\..\WorkingFiles\PrintedResult.json".Replace('\\', Path.DirectorySeparatorChar));

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerMenuPrint"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        public ManagerMenuPrint(string name) : base(name, new[] { "Table", "Calendar", "Priority statistics", "Status statistics", "Return" })
        {
            Actions = new Action[] { () => new TableManagerMenu("Table Menu").Run(), PrintCalendar, PriorityStatistics, StatusStatistics, () => { } };
        }

        /// <summary>
        /// Displays a calendar view of ToDo items for a selected year and month.
        /// </summary>
        private void PrintCalendar()
        {
            try
            {
                // Create a layout for the calendar and ToDo list
                Layout layout = new Layout("Calendar").SplitColumns(new Layout("Left").Ratio(6), new Layout("Right"));

                // Prompt the user to choose a year and month
                DateChooser(out int year, out int month);

                // Create the calendar and get the days with ToDo deadlines
                Calendar calendar = CreateCalendar(year, month, out List<int> days);
                Panel calendarPanel = new Panel(calendar).Header("Calendar").Padding(0, 0, 0, 0).Expand();

                // Filter ToDo items for the selected month and days
                List<ToDo> toDo = CurrentToDoCol
                    .Where(x => (x.GetDeadLine().Item1, x.GetDeadLine().Item2) == (year, month))
                    .Where(x => days.Contains(x.GetDeadLine().Item3)).ToList();

                // Update the layout with the calendar and ToDo list
                layout["Left"].Update(calendarPanel).MinimumSize(48).Size(48);
                layout["Right"].Update(CreateToDoPanel(toDo).Expand()).MinimumSize(36).Size(80);

                // Display the layout
                AnsiConsole.Write(layout);
                AnsiConsole.WriteLine();
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine("[red]Error: " + e.Message + "[/]");
            }
        }

        /// <summary>
        /// Prompts the user to choose a year and month.
        /// </summary>
        /// <param name="year">The selected year.</param>
        /// <param name="month">The selected month.</param>
        private void DateChooser(out int year, out int month)
        {
            string res = AnsiConsole.Prompt(new TextPrompt<string>("Enter year: "));

            if (!(int.TryParse(res, out year) && year >= 0 && year <= DateTime.Now.Year + 100))
            {
                throw new InvalidDataException("Invalid year");
            }

            string result = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose month: ").AddChoices(Enum.GetNames(typeof(Months))));
            month = (int)Enum.Parse(typeof(Months), result);
        }

        /// <summary>
        /// Creates a calendar for the specified year and month and retrieves the days with ToDo deadlines.
        /// </summary>
        /// <param name="year">The year for the calendar.</param>
        /// <param name="month">The month for the calendar.</param>
        /// <param name="toDoDays">A list of days with ToDo deadlines.</param>
        /// <returns>A calendar for the specified year and month.</returns>
        private Calendar CreateCalendar(int year, int month, out List<int> toDoDays)
        {
            Calendar calendar = new Calendar(year, month).LeftAligned();
            calendar.HeaderStyle(Style.Parse("blue bold"));

            // Get the days with ToDo deadlines
            toDoDays = CurrentToDoCol.ToDoByDeadLine.Where(x => (x.Key.Item1, x.Key.Item2) == (year, month))
                .Select(x => x.Key.Item3).ToList();

            // Highlight the days with ToDo deadlines
            toDoDays.ForEach(x => calendar.AddCalendarEvent(year, month, x));
            calendar.HighlightStyle(Style.Parse("yellow bold"));

            return calendar;
        }

        /// <summary>
        /// Creates a panel displaying ToDo items for the specified days.
        /// </summary>
        /// <param name="todos">The list of ToDo items to display.</param>
        /// <returns>A panel containing the ToDo items.</returns>
        private Panel CreateToDoPanel(List<ToDo> todos)
        {
            int count = todos.Count;
            Markup heading = new Markup($"[bold lightseagreen]At this month there are [bold green]{count}[/] days on which there is a ToDo deadline[/]");
            Table table = new Table().AddColumn("Id").AddColumn("Name").AddColumn("[bold yellow]Deadline[/]").AddColumn("Status");
            foreach (ToDo todo in todos)
            {
                table.AddRow(todo.Id, todo.Name, todo.DeadlineDate.Day.ToString(), todo.Status.ToString());
            }
            Panel p = new Panel(new Rows(heading, table.Expand()).Expand()).Header("ToDo by Date");

            return p;
        }

        /// <summary>
        /// Displays a bar chart showing the distribution of ToDo items by priority.
        /// </summary>
        private void PriorityStatistics()
        {
            Func<string, int> priorityFunc = (s) => CurrentToDoCol.Count(x => x.Priority == (ToDoPriority)Enum.Parse(typeof(ToDoPriority), s));

            int high = priorityFunc("High");
            int low = priorityFunc("Low");
            int medium = priorityFunc("Medium");

            BarChart chart = new BarChart()
                .Width(40)
                .Label("[teal bold underline]Number of todo with corresponding priority[/]")
                .AddItem("High", high, Color.Red)
                .AddItem("Medium", medium, Color.Yellow)
                .AddItem("Low", low, Color.Green);

            AnsiConsole.Write(chart);
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// Displays a breakdown chart showing the distribution of ToDo items by status.
        /// </summary>
        private void StatusStatistics()
        {
            Func<string, int> statusFunc = (s) => CurrentToDoCol.Count(x => x.Status == (ToDoStatus)Enum.Parse(typeof(ToDoStatus), s));

            int active = statusFunc("Active");
            int completed = statusFunc("Completed");
            int postponed = statusFunc("Postponed");

            AnsiConsole.MarkupLine("[teal bold underline]Number of todo with corresponding status[/]");
            AnsiConsole.WriteLine();

            BreakdownChart chart = new BreakdownChart()
                .Width(40)
                .AddItem("Postponed", postponed, Color.Magenta3)
                .AddItem("Active", active, Color.Yellow3)
                .AddItem("Completed", completed, Color.Cyan3);

            AnsiConsole.Write(chart);
            AnsiConsole.WriteLine();
        }
    }
}