using System.Security.Cryptography;
using System.Text;
using Messages;

namespace ToDoManagerStatistics
{
    /// <summary>
    /// Represents a user in the system, including their personal information and statistics.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Telegram nickname of the user.
        /// </summary>
        public string TelegramNickname { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the statistics associated with the user.
        /// </summary>
        public Statistics Statistics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is subscribed to Telegram notifications.
        /// </summary>
        public bool IsSubscribedToTelegram { get; set; }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with default values.
        /// </summary>
        public User()
        {
            using (MD5 md5 = MD5.Create())
            {
                string input = Guid.NewGuid().ToString();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                Id = BitConverter.ToString(hash).Replace("-", "").Substring(0, 16);
            }
            Statistics = new Statistics();
            Name = "None";
            TelegramNickname = "None";
            Email = "None";
            Password = "None";
            IsSubscribedToTelegram = false; // Subscription is disabled by default
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with specified details.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="telegramNickname">The Telegram nickname of the user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        public User(string name, string telegramNickname, string email, string password) : this()
        {
            Name = name;
            TelegramNickname = telegramNickname;
            Email = email;
            Password = password;
        }

        /// <summary>
        /// Updates the user's subscription status for Telegram notifications.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="isSubscribedToTelegram">A value indicating whether the user is subscribed to Telegram notifications.</param>
        public void TelegramSubscribeToTelegram(object? sender, bool isSubscribedToTelegram)
        {
            IsSubscribedToTelegram = isSubscribedToTelegram;
        }
        
        /// <summary>
        /// Updates the user's subscription status for Telegram notifications.
        /// </summary>
        public async Task EmailSubscribeToEmail()
        {
            string title = "Subscribtion";
            string body = $"Hi {Name}," + Environment.NewLine + " You’re now subscribed to task notifications!" +
                          Environment.NewLine + "Stay informed about your tasks and deadlines. " + Environment.NewLine +
                          "Thanks, The ToDo Manager Team";
            
            EmailNotification notification = new EmailNotification(title, body,Email, DateTime.Now);
            await notification.Send();
            Task.Delay(1000).Wait();
        }
    }
}