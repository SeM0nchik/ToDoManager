using Spectre.Console;

namespace Messages
{
    /// <summary>
    /// Represents a notification that can be displayed in the console.
    /// </summary>
    public class ConsoleNotification : EventArgs, INotifiable
    {
        /// <summary>
        /// Gets or sets the title of the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body content of the notification.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the notification should be sent.
        /// </summary>
        public DateTime NotifyDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNotification"/> class with default values.
        /// </summary>
        public ConsoleNotification()
        {
            Title = "My notification";
            Body = "This is a new notification";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleNotification"/> class with specified values.
        /// </summary>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body content of the notification.</param>
        /// <param name="notifyDate">The date and time when the notification should be sent.</param>
        public ConsoleNotification(string title, string body, DateTime notifyDate)
        {
            Title = title;
            Body = body;
            NotifyDate = notifyDate;
        }

        /// <summary>
        /// Sends the notification to the console with a simulated delay.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Send()
        {
            await Task.Delay(2000); // Simulate a delay
            AnsiConsole.MarkupLine($"[bold lightcoral]{Title}[/]");
            AnsiConsole.MarkupLine($"[aquamarine1_1]{Body}[/]");
        }
    }
}