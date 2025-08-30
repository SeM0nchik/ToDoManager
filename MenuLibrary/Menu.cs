using Spectre.Console;
namespace MenuLibrary
{
    /// <summary>
    /// Represents an abstract base class for menus, providing shared functionality for displaying and handling menu options.
    /// </summary>
    public abstract class Menu
    {
        /// <summary>
        /// Gets or sets the name of the menu.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of menu items.
        /// </summary>
        public string[] Items { get; set; }

        /// <summary>
        /// Gets or sets the list of actions corresponding to the menu items.
        /// </summary>
        public Action[]? Actions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="items">The list of menu items.</param>
        public Menu(string name, string[] items)
        {
            Name = name;
            Items = items;
        }

        /// <summary>
        /// Prompts the user to select a choice from the menu items.
        /// </summary>
        /// <returns>The selected menu item as a string.</returns>
        public string ReadChoice()
        {
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[teal bold]" + Name + "[/]")
                    .PageSize(10)
                    .AddChoices(Items));
            return choice;
        }

        /// <summary>
        /// Runs the menu, allowing the user to select an option and execute the corresponding action.
        /// </summary>
        public virtual void Run()
        {
            string choice = ReadChoice();
            int index = Array.IndexOf(Items, choice);

            if (Actions?.Length > index && index >= 0)
            {
                Actions[index].Invoke(); // Execute the corresponding action
            }
            else
            {
                throw new ArgumentException("Invalid choice");
            }
        }
    }
}