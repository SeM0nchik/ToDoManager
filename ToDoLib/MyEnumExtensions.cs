namespace Library
{
    public static class MyEnumExtensions
    {
        public static string ToString(this ToDoPriority priority)
        {

            Dictionary<ToDoPriority, string> dict =
                new Dictionary<ToDoPriority, string>()
                {
                    { ToDoPriority.Low, $"[green]{ToDoPriority.Low}[/]" },
                    { ToDoPriority.Medium, $"[yellow]{ToDoPriority.Medium}[/]" },
                    { ToDoPriority.High, $"[red]{ToDoPriority.High}[/]" }
                };

            return dict[priority];
        }

        public static string ToString(this ToDoStatus status)
        {
            Dictionary<ToDoStatus, string> dict =
                new Dictionary<ToDoStatus, string>()
                {
                    { ToDoStatus.Completed, $"[green]{ToDoStatus.Completed}[/]" },
                    { ToDoStatus.Active, $"[yellow]{ToDoStatus.Active}[/]" },
                    { ToDoStatus.Postponed, $"[red]{ToDoStatus.Postponed}[/]" },
                    { ToDoStatus.Default, $"[gray]{ToDoStatus.Default}[/]" }
                };

            return dict[status];
        }
    }
}