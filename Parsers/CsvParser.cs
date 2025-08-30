using Library;
using Spectre.Console;
namespace Parsers
{
    /// <summary>
    /// Static csv (txt) parser.
    /// </summary>
    public static class CsvParser
    {
        /// <summary>
        /// Current separator char.
        /// </summary>
        private static char _separator = ';';
        
        /// <summary>
        /// A method that converts a line into a list of field values and put it into a queue.
        /// </summary>
        /// <param name="filepath">File path.</param>
        /// <returns>Queue.</returns>
        private static Queue<List<string>> CsvToQueue(string filepath)
        {
            Queue<List<string>> result = new Queue<List<string>>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                string? line;
                bool firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLine) { firstLine = !firstLine; continue; }
                    List<string> row = line.Split(_separator).ToList();
                    result.Enqueue(row);
                }
            }
            
            return result;
        }

        /// <summary>
        /// A method that converts data from the file into ToDo Collection.
        /// </summary>
        /// <param name="filepath">File path.</param>
        /// <returns>ToDo Collection.</returns>
        public static  ToDoCollection CsvToCollection(string filepath)
        {
            ToDoCollection result = new ToDoCollection();
            Queue<List<string>> queue = CsvToQueue(filepath);
            while (queue.Count > 0  )
            {
                List<string> strings = queue.Dequeue();
                try
                {
                    ToDo todo = new ToDo(strings);
                    result.AddToDo(todo);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteException(ex
                    );
                }
            }
            return result;
        }


        /// <summary>
        /// A method that writes a ToDo Collection to file.
        /// </summary>
        /// <param name="toDoCollection">ToDo collection.</param>
        /// <param name="filename">File path.</param>
        public static void WriteToCsv(ToDoCollection toDoCollection, string filename)
        {
            using StreamWriter sw = new StreamWriter(filename);
            TextWriter current = Console.Out;
            Console.SetOut(sw);
            Console.WriteLine("Id;Name;Description;Category;Created;DeadLineDate;Status;Priority");
            foreach (ToDo todo in toDoCollection)
            {
                Console.WriteLine(todo.ToCsv());
            }
            Console.SetOut(current);
        }
    }
    
    
}