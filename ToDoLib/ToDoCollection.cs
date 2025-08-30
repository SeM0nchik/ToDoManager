using Spectre.Console;
using System.Collections;

namespace Library
{
    /// <summary>
    /// Represents a collection of ToDo items with functionality for managing and displaying them.
    /// </summary>
    public class ToDoCollection : IEnumerable<ToDo>
    {
        /// <summary>
        /// The underlying list of ToDo items.
        /// </summary>
        private List<ToDo> _toDos;

        /// <summary>
        /// Gets the number of ToDo items in the collection.
        /// </summary>
        public int Count => _toDos.Count;

        /// <summary>
        /// Gets a list of unique deadlines from the ToDo items.
        /// </summary>
        private List<Tuple<int, int, int>> Deadlines => _toDos.Select(x => x.GetDeadLine()).ToHashSet().ToList();

        /// <summary>
        /// Gets a dictionary grouping ToDo items by their deadlines.
        /// </summary>
        public Dictionary<Tuple<int, int, int>, List<ToDo>> ToDoByDeadLine
        {
            get
            {
                Dictionary<Tuple<int, int, int>, List<ToDo>> result = new();

                foreach (Tuple<int, int, int> deadLine in Deadlines)
                {
                    result.Add(deadLine, _toDos.Where(x => x.GetDeadLine().Equals(deadLine)).ToList());
                }
                return result;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCollection"/> class with an empty list of ToDo items.
        /// </summary>
        public ToDoCollection()
        {
            _toDos = new List<ToDo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoCollection"/> class with a specified list of ToDo items.
        /// </summary>
        /// <param name="toDos">The list of ToDo items to initialize the collection with.</param>
        public ToDoCollection(List<ToDo> toDos)
        {
            _toDos = toDos;
        }

        /// <summary>
        /// Adds a ToDo item to the collection.
        /// </summary>
        /// <param name="toDo">The ToDo item to add.</param>
        public void AddToDo(ToDo toDo)
        {
            _toDos.Add(toDo);
        }

        /// <summary>
        /// Displays the ToDo items in a formatted table using Spectre.Console.
        /// </summary>
        public void GetTable()
        {
            Table result = new Table()
                .Centered()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.LightCyan3)
                .Title("[underline bold yellow]ToDo List[/]")
                .Caption("[grey]This is a sample ToDo list[/]");

            IEnumerable<string> fields = typeof(ToDo).GetProperties().Select(x => x.Name).ToList();
            AnsiConsole.Live(result).Start(ctx =>
            {
                foreach (string field in fields)
                {
                    result.AddColumn(new TableColumn($"[bold indianred1_1]{field}[/]").Centered());
                    ctx.Refresh();
                    Thread.Sleep(100);
                }

                foreach (ToDo todo in _toDos)
                {
                    result.AddRow(todo.ToRow().ToArray());
                    ctx.Refresh();
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// Finds a ToDo item by its ID.
        /// </summary>
        /// <param name="id">The ID of the ToDo item to find.</param>
        /// <returns>The ToDo item with the specified ID, or <c>null</c> if not found.</returns>
        public ToDo? FindId(string id)
        {
            foreach (ToDo toDo in _toDos)
            {
                if (toDo.Id == id)
                {
                    return toDo;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes a ToDo item by its ID.
        /// </summary>
        /// <param name="id">The ID of the ToDo item to delete.</param>
        public void DeleteId(string id)
        {
            _toDos = _toDos.Where(x => x.Id != id).Select(x => x).ToList();
        }

        /// <summary>
        /// Creates a copy of the ToDoCollection.
        /// </summary>
        /// <returns>A new instance of <see cref="ToDoCollection"/> with the same ToDo items.</returns>
        public ToDoCollection Copy()
        {
            return new ToDoCollection(_toDos);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the ToDo items.
        /// </summary>
        /// <returns>An enumerator for the ToDo items.</returns>
        public IEnumerator<ToDo> GetEnumerator()
        {
            return _toDos.GetEnumerator();
        }

        /// <summary>
        /// Returns a non-generic enumerator that iterates through the ToDo items.
        /// </summary>
        /// <returns>A non-generic enumerator for the ToDo items.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _toDos.GetEnumerator();
        }
    }
}