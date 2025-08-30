using Parsers;
using ToDoManagerStatistics;
namespace MenuLibrary
{
    /// <summary>
    /// Exit menu.
    /// </summary>
    public sealed class ManagerMenuExit : ManagerMenu
    {

        
        /// <summary>
        /// Primary constructor.
        /// </summary>
        public ManagerMenuExit(string name) : base(name, new []{"Exit", "Return"})
        {
            Actions = new []{WriteToFile, () => {} };
        }
        
        /// <summary>
        /// Writes current updated data to file.
        /// </summary>
        private void WriteToFile()
        {
            if (FileName.EndsWith(".txt") || FileName.EndsWith(".csv"))
            {
                CsvParser.WriteToCsv(CurrentToDoCol, FileName);
            }
            else if (FileName.EndsWith(".json"))
            {
                JsonParser.WriteToJson(CurrentToDoCol, FileName);
            }
            
            //Stop bot before we stop our program.
            TelegramBotManager.StopBot();
            
            Environment.Exit(0);
        }
    }
}