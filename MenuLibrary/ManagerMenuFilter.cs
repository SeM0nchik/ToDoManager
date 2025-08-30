using Spectre.Console;
using Library;

namespace MenuLibrary
{
    /// <summary>
    /// Represents a menu for filtering ToDo items by status or priority.
    /// </summary>
    public sealed class ManagerMenuFilter : ManagerMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerMenuFilter"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        public ManagerMenuFilter(string name) : base(name,
            new[] { "Filter Status", "Filter Priority", "Cancel", "Return" })
        {
            Actions = new[] { FilterStatus, FilterPriority, () => { CurrentToDoCol = AllToDoCol; }, () => { } };
        }

        /// <summary>
        /// Filters the current ToDo collection by status.
        /// </summary>
        private void FilterStatus()
        {
            string result = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose ToDo Status")
                    .PageSize(10)
                    .AddChoices(new[] { "Active", "Completed", "Postponed" }));

            CurrentToDoCol = new ToDoCollection(CurrentToDoCol.Where(x => x.Status.ToString() == result).ToList());
        }

        /// <summary>
        /// Filters the current ToDo collection by priority.
        /// </summary>
        private void FilterPriority()
        {
            string result = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose ToDo Priority")
                    .PageSize(10)
                    .AddChoices(new[] { "High", "Medium", "Low" }));

            CurrentToDoCol = new ToDoCollection(CurrentToDoCol.Where(x => x.Priority.ToString() == result).ToList());
        }
    }
}