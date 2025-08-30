//**Студент**: Павлычев Семён Михайлович
//**Вариант**: 2
//**Проект**: B side 
//**Группа** : БПИ244

using MenuLibrary;
using ToDoManagerStatistics;
namespace Project_4_B_Side
{
    public class Progarm
    {
        public static void Main(string[] args)
        {
            TelegramBotManager.StartBot();
            
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MainManagerMenu managerMenu = new MainManagerMenu();
            managerMenu.Run();
            
            
        }
    }
}