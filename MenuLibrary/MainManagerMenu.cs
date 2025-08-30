using Spectre.Console;
using Parsers;
using ToDoManagerStatistics;

namespace MenuLibrary
{
    /// <summary>
    /// Represents the main menu of the ToDo manager application.
    /// </summary>
    public class MainManagerMenu : ManagerMenu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainManagerMenu"/> class with default menu items.
        /// </summary>
        public MainManagerMenu() : base("MainMenu", new[] { "Flick through tasks", "Edit data collection", "Set filters", "Productivity", "Exit" })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainManagerMenu"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="strings">The list of menu items.</param>
        public MainManagerMenu(string name, string[] strings) : base(name, strings) { }

        /// <summary>
        /// Runs the main menu, handling user authentication and file loading.
        /// </summary>
        public override void Run()
        {
            // Ensure the user is authenticated
            
            Auth auth = new Auth("Authorization");
            auth.Run();
            // Display a welcome message
            HelloMessage();

            // Load the file content
            while (!ReadFileContent()) { }

            // Main menu loop
            while (true)
            {
                string choice = ReadChoice();
                ManagerMenu[] menus = new ManagerMenu[] {
                    new ManagerMenuPrint("Flick through tasks"),
                    new ManagerMenuData("Work with data"),
                    new ManagerMenuFilter("Set filters"),
                    new ManagerMenuProductivity("Productivity"),
                    new ManagerMenuExit("Exit")
                };

                int index = Array.IndexOf(Items, choice);
                menus[index].Run();
            }
        }

        /// <summary>
        /// Prompts the user to enter a file path and validates it.
        /// </summary>
        private void ReadFilePath()
        {
            string filePath = AnsiConsole.Prompt(new TextPrompt<string>("Enter the file path: "));
            filePath = Path.GetFullPath(filePath.Replace('\\', Path.DirectorySeparatorChar));

            if (Path.Exists(filePath) && (filePath.EndsWith(".txt") || filePath.EndsWith(".csv") || filePath.EndsWith(".json")))
            {
                string result = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("File with this path is found. Do you want to rewrite data in this file?")
                    .AddChoices(new[] { "Yes", "No" }));

                if (result == "No")
                {
                    throw new FileLoadException("File already exists.");
                }
            }
            else if (!Path.Exists(filePath) && (filePath.EndsWith(".txt") || filePath.EndsWith(".csv") || filePath.EndsWith(".json")))
            {
                File.Create(filePath);
                AnsiConsole.MarkupLine(filePath);
            }
            else
            {
                throw new FileLoadException("Incorrect file format. Please enter .txt, .csv, or .json.");
            }

            FileName = filePath;
        }

        /// <summary>
        /// Reads the content of the file and loads it into the ToDo collection.
        /// </summary>
        /// <returns><c>true</c> if the file is loaded successfully; otherwise, <c>false</c>.</returns>
        private bool ReadFileContent()
        {
            try
            {
                ReadFilePath();

                if (FileName.EndsWith(".txt") || FileName.EndsWith(".csv"))
                {
                    AllToDoCol = CsvParser.CsvToCollection(FileName).Copy();
                }
                else if (FileName.EndsWith(".json"))
                {
                    AllToDoCol = JsonParser.JsonToCollection(FileName).Copy();
                }

                CurrentToDoCol = AllToDoCol;
                AnsiConsole.MarkupLine($"[green]File is uploaded successfully.[/]");
                return true;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Error: " + ex.Message + "[/]");
                return false;
            }
        }

        /// <summary>
        /// Displays a welcome message to the user.
        /// </summary>
        private void HelloMessage()
        {
            AnsiConsole.Markup($"[palegreen1]Dear {UserDataManager.CurrentUser?.Name}!" + Environment.NewLine +
                               "Welcome to the ToDo manager. " + Environment.NewLine +
                               "In order to start, enter the file path with the ToDo collection. [/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
        }
    }
}