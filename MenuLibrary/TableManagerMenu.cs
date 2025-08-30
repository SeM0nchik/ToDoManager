using Spectre.Console;
using Library;
using Filters_and_Sorts;

namespace MenuLibrary
{
    /// <summary>
    /// Represents a menu for managing and displaying ToDo items in a table format with filtering and sorting options.
    /// </summary>
    public class TableManagerMenu : ManagerMenu
    {
        /// <summary>
        /// The collection of ToDo items to be displayed in the table.
        /// </summary>
        private ToDoCollection _printedCollection;

        /// <summary>
        /// The pipeline for applying filters and sorts to the ToDo collection.
        /// </summary>
        private Pipeline<ToDo> _lines;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableManagerMenu"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        public TableManagerMenu(string name) : base(name, new[] { "Set filters", "Set sorts", "Show Filters", "Delete filters", "Return" })
        {
            _printedCollection = CurrentToDoCol.Copy();
            _lines = new Pipeline<ToDo>();
            Actions = new Action[] { SetFilters, SetSorts, ShowFilters, DeleteFilters, () => { } };
        }

        /// <summary>
        /// Displays the currently applied filters and sorts.
        /// </summary>
        private void ShowFilters()
        {
            if (_lines.Length != 0)
            {
                _lines.PipeLine.ForEach(x => AnsiConsole.WriteLine(x.Name));
            }
            else
            {
                AnsiConsole.WriteLine("There are no filters or sorts set.");
            }
        }

        /// <summary>
        /// Prompts the user to set a sorting criterion for the ToDo items.
        /// </summary>
        private void SetSorts()
        {
            string field =
                AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[teal]Available fields:[/]")
                    .AddChoices(typeof(ToDo).GetProperties().Select(x => x.Name)));

            bool confirmation = AnsiConsole.Prompt(
                new SelectionPrompt<bool>().Title("[teal]Reverse sorting? [/]")
                    .AddChoices(true, false));

            Sorter<ToDo> sorter = new Sorter<ToDo>($"Sort by {field}", field, confirmation);
            _lines.AddSorter(sorter);
        }

        /// <summary>
        /// Prompts the user to delete a filter or sort from the pipeline.
        /// </summary>
        private void DeleteFilters()
        {
            if (_lines.Length != 0)
            {
                string result = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("Choose element to delete").AddChoices(_lines.PipeLine.Select(x => x.Name)));

                _lines.RemoveName(result);
            }
            else
            {
                AnsiConsole.WriteLine("There are no filters or sorts set.");
            }
        }

        /// <summary>
        /// Prompts the user to set a filter criterion for the ToDo items.
        /// </summary>
        private void SetFilters()
        {
            string field =
                AnsiConsole.Prompt(new SelectionPrompt<string>().Title("[teal]Available fields:[/]")
                    .AddChoices(typeof(ToDo).GetProperties().Select(x => x.Name)));

            Func<string, string> naming = x => $"Filter is set by {x}";

            Filter<ToDo> filter = new Filter<ToDo>();
            switch (field)
            {
                case "Priority":
                    {
                        string result = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose ToDo Priority").PageSize(10).AddChoices(new[] { "High", "Medium", "Low" }));
                        filter = new Filter<ToDo>(naming(field), x => x.Priority.ToString() == result);
                    }
                    break;

                case "Status":
                    {
                        string result = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Choose ToDo Status").PageSize(10).AddChoices(new[] { "Active", "Completed", "Postponed" }));
                        filter = new Filter<ToDo>(naming(field), x => x.Status.ToString() == result);
                    }
                    break;

                case "Created":
                    {
                        string result = AnsiConsole.Prompt(new TextPrompt<string>("Choose ToDo Creation date in yyyy:mm:dd"));
                        string[] components = result.Split(":");
                        if (components.Length >= 3 && uint.TryParse(components[0], out uint year) &&
                            uint.TryParse(components[1], out uint month) && uint.TryParse(components[2], out uint day))
                        {
                            filter = new Filter<ToDo>(naming(field), x => x.Created == new DateTime((int)year, (int)month, (int)day));
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Error: The parameter format is invalid.[/]");
                        }
                    }
                    break;

                case "DeadlineDate":
                    {
                        string result = AnsiConsole.Prompt(new TextPrompt<string>("Choose ToDo Deadline date in yyyy:mm:dd"));
                        string[] components = result.Split(":");
                        if (components.Length >= 3 && uint.TryParse(components[0], out uint year) &&
                            uint.TryParse(components[1], out uint month) && uint.TryParse(components[2], out uint day))
                        {
                            filter = new Filter<ToDo>(naming(field), x => x.DeadlineDate == new DateTime((int)year, (int)month, (int)day));
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Error: The parameter format is invalid.[/]");
                        }
                        break;
                    }

                default:
                    {
                        string result = AnsiConsole.Prompt(new TextPrompt<string>($"Choose ToDo {field}"));
                        filter = new Filter<ToDo>(naming(field),
                            x => typeof(ToDo).GetProperty(field)?.GetValue(x)?.ToString() == result);
                    }
                    break;
            }
            _lines.AddFilter(filter);
        }

        /// <summary>
        /// Runs the table manager menu, allowing the user to apply filters, sorts, and view the results.
        /// </summary>
        public override void Run()
        {
            while (true)
            {
                // Apply filters and sorts to the current collection
                _printedCollection = new ToDoCollection(_lines.Apply(CurrentToDoCol).ToList());

                // Display the filtered and sorted collection in a table
                _printedCollection.GetTable();

                // Prompt the user to choose an action
                string choice = ReadChoice();
                int index = Array.IndexOf(Items, choice);
                if (Actions?.Length > index && index >= 0)
                {
                    Actions[index].Invoke();
                }
                else
                {
                    throw new ArgumentException("Invalid choice");
                }

                // Exit the loop if the user chooses "Return"
                if (index == Items.Length - 1)
                {
                    break;
                }
            }
        }
    }
}