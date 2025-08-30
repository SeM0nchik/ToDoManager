using Library;
using Spectre.Console;

namespace MenuLibrary
{
    /// <summary>
    /// Represents a menu for managing ToDo data, including adding, editing, and deleting ToDo items.
    /// </summary>
    public sealed class ManagerMenuData : ManagerMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerMenuData"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        public ManagerMenuData(string name) : base(name, new[] { "Add ToDo", "Edit ToDo", "Delete ToDo", "Return" })
        {
            Actions = new Action[] { AddToDo, EditId, DeleteId, () => { } };
        }

        /// <summary>
        /// Edits the properties of a specified ToDo item.
        /// </summary>
        /// <param name="todo">The ToDo item to edit.</param>
        public void EditToDo(ToDo todo)
        {
            List<string> lines = typeof(ToDo).GetProperties().Select(x => x.Name).ToList();
            lines.Add("Notify Date");
            lines.Add("Finish");
            lines = lines[1..]; // Skip the first element (Id)
            bool flag = true;

            while (flag)
            {
                todo.Print();
                string result = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What information would you like to edit?")
                        .PageSize(10)
                        .AddChoices(lines));

                switch (result)
                {
                    case "Status":
                        {
                            string status = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Choose the new status:")
                                    .PageSize(10)
                                    .AddChoices(new[] { "Active", "Completed", "Postponed" }));

                            todo.Edit("Status", status);
                        }
                        break;

                    case "Priority":
                        {
                            string priority = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Choose the new priority:")
                                    .PageSize(10)
                                    .AddChoices(new[] { "High", "Medium", "Low" }));

                            todo.Edit("Priority", priority);
                        }
                        break;

                    case "DeadlineDate":
                        {
                            string res = AnsiConsole.Prompt(
                                new TextPrompt<string>("Enter the new deadline (dd/mm/yyyy HH:mm:ss):"));

                            if (DateTime.TryParse(res, out DateTime date) && date > DateTime.Now)
                            {
                                todo.Edit("DeadlineDate", date);
                            }
                            else if (date <= DateTime.Now)
                            {
                                AnsiConsole.MarkupLine("[red]You cannot set a deadline in the past![/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Invalid date format![/]");
                            }
                        }
                        break;

                    case "Created":
                        {
                            AnsiConsole.MarkupLine("[yellow]You cannot modify the creation date.[/]");
                        }
                        break;

                    case "Finish":
                        {
                            flag = false; // Exit the editing loop
                        }
                        break;

                    case "Notify Date":
                        {
                            string res = AnsiConsole.Prompt(
                                new TextPrompt<string>("Enter the notification date (dd/mm/yyyy HH:mm:ss):"));

                            if (DateTime.TryParse(res, out DateTime date) && date > DateTime.Now)
                            {
                                todo.AddNotify(date);
                            }
                            else if (date <= DateTime.Now)
                            {
                                AnsiConsole.MarkupLine("[red]You cannot set a notification date in the past![/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]Invalid date format![/]");
                            }
                        }
                        break;

                    default:
                        {
                            string res = AnsiConsole.Prompt(
                                new TextPrompt<string>($"Enter the new value for {result}:"));

                            todo.Edit(result, res);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a new ToDo item to the collection.
        /// </summary>
        public void AddToDo()
        {
            ToDo toDo = new ToDo();
            EditToDo(toDo); // Edit the new ToDo item
            CurrentToDoCol.AddToDo(toDo); // Add it to the collection
        }

        /// <summary>
        /// Deletes a ToDo item by its ID.
        /// </summary>
        public void DeleteId()
        {
            string result = AnsiConsole.Prompt(new TextPrompt<string>("Enter the ID of the ToDo item to delete: "));
            if (CurrentToDoCol.Select(x => x.Id).Contains(result))
            {
                CurrentToDoCol.DeleteId(result);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid ID![/]");
            }
        }

        /// <summary>
        /// Edits a ToDo item by its ID.
        /// </summary>
        public void EditId()
        {
            string result = AnsiConsole.Prompt(new TextPrompt<string>("Enter the ID of the ToDo item to edit: "));
            if (CurrentToDoCol.Select(x => x.Id).Contains(result))
            {
                ToDo? todo = CurrentToDoCol.FindId(result);
                if (todo != null)
                {
                    EditToDo(todo); // Edit the found ToDo item
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid ID![/]");
            }
        }
    }
}