using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Spectre.Console;
using Library;
namespace Parsers
{
    /// <summary>
    /// Static json parser.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Method that parses file into ToDoCollection.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Parsed ToDoCollection.</returns>
        public static ToDoCollection JsonToCollection(string filePath)
        {
            ToDoCollection result = new ToDoCollection();
            
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                
                List<JsonElement> jsonElements;

                jsonElements = JsonSerializer.Deserialize<List<JsonElement>>(fs) ?? new List<JsonElement>();
                
                foreach (JsonElement jsonElement in jsonElements)
                {
                    
                    try
                    {
                        if (jsonElement.TryGetProperty("Name", out JsonElement name) &&
                            jsonElement.TryGetProperty("Description", out JsonElement description) &&
                            jsonElement.TryGetProperty("Category", out  JsonElement category) && 
                            jsonElement.TryGetProperty("DeadlineDate",  out JsonElement deadline )
                            && jsonElement.TryGetProperty("Status", out JsonElement status)
                            && jsonElement.TryGetProperty("Priority", out JsonElement priority))
                        {
                            Dictionary<string, string> dict = new Dictionary<string, string>()
                            {
                                {"Name", name.ToString()},{"Description", description.ToString()} ,
                                {"Category", category.ToString() },{"DeadlineDate", deadline.ToString() },
                                {"Status", status.ToString() }, { "Priority", priority.ToString() }
                            };

                            if (jsonElement.TryGetProperty("Created", out JsonElement created))
                            {
                                dict.Add("Created", created.ToString());
                            }

                            if (jsonElement.TryGetProperty("ID", out JsonElement id))
                            {
                                dict.Add("ID", id.ToString());
                            }
                            
                            ToDo toDo = new ToDo(dict);
                            
                            result.AddToDo(toDo);
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// A method that writes ToDo Collection to the file.
        /// </summary>
        /// <param name="collection">ToDo cCollection.</param>
        /// <param name="filename">File path.</param>
        public static void WriteToJson(ToDoCollection collection, string filename)
        {
            Console.OutputEncoding = Encoding.UTF8;
            using StreamWriter sw = new StreamWriter(filename);
            TextWriter current = Console.Out;
            Console.SetOut(sw);
            
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            
            string result  = JsonSerializer.Serialize(collection, options);
            Console.Write(result);
            Console.SetOut(current);
        }
    }
}

