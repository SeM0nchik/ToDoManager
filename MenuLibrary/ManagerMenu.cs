using Library;
using ToDoManagerStatistics;

namespace MenuLibrary
{
    /// <summary>
    /// Represents an abstract base class for manager menus, providing shared functionality for managing ToDo collections.
    /// </summary>
    public abstract class ManagerMenu : Menu
    {
        /// <summary>
        /// Gets or sets the file name used for storing or loading ToDo data.
        /// </summary>
        public static string FileName { get; set; }

        /// <summary>
        /// Gets or sets the current collection of ToDo items being displayed or edited.
        /// </summary>
        public static ToDoCollection CurrentToDoCol;

        /// <summary>
        /// Gets or sets the complete collection of ToDo items.
        /// </summary>
        public static ToDoCollection AllToDoCol;

        /// <summary>
        /// Initializes static members of the <see cref="ManagerMenu"/> class.
        /// </summary>
        static ManagerMenu()
        {
            FileName = Path.GetFullPath(@"..\..\..\..\WorkingFiles\ToDo.json");
            AllToDoCol = new ToDoCollection();
            CurrentToDoCol = AllToDoCol;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerMenu"/> class with a specified name and menu items.
        /// </summary>
        /// <param name="name">The name of the menu.</param>
        /// <param name="items">The list of menu items.</param>
        public ManagerMenu(string name, string[] items) : base(name, items)
        {
        }
    }
}