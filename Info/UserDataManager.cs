using Messages;

namespace ToDoManagerStatistics
{
    /// <summary>
    /// Manages user data, notifications, and background tasks for sending notifications.
    /// </summary>
    public static class UserDataManager
    {
        /// <summary>
        /// Gets the list of users.
        /// </summary>
        public static List<User> Users { get; private set; }

        /// <summary>
        /// Gets the dictionary mapping Telegram nicknames to chat IDs.
        /// </summary>
        public static Dictionary<string, long> Chats { get; private set; }

        /// <summary>
        /// Gets the list of notifications to be sent.
        /// </summary>
        public static List<INotifiable> Notifications { get; private set; }

        /// <summary>
        /// Gets or sets the currently logged-in user.
        /// </summary>
        public static User CurrentUser { get; set; } = new User();

        /// <summary>
        /// The cancellation token source for managing the background task.
        /// </summary>
        private static CancellationTokenSource _cts;

        /// <summary>
        /// Gets the background task for checking and sending notifications.
        /// </summary>
        public static Task? BackgroundTask { get; private set; }

        /// <summary>
        /// Initializes the static members of the <see cref="UserDataManager"/> class.
        /// </summary>
        static UserDataManager()
        {
            Users = new List<User>();
            Chats = new Dictionary<string, long>();
            Notifications = new List<INotifiable>();
            _cts = new CancellationTokenSource();

            StartNotificationScheduler();
        }

        /// <summary>
        /// Starts the background task for checking and sending notifications.
        /// </summary>
        public static void StartNotificationScheduler()
        {
            BackgroundTask = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // Check and send notifications
                        await CheckAndSendNotificationsAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in background task: {ex.Message}");
                    }

                    // Wait for 1 minute before the next check
                    await Task.Delay(TimeSpan.FromMinutes(1), _cts.Token);
                }
            }, _cts.Token);
        }

        /// <summary>
        /// Stops the background task.
        /// </summary>
        public static void StopNotificationScheduler()
        {
            _cts.Cancel();
        }

        /// <summary>
        /// Checks and sends notifications if their scheduled time has arrived.
        /// </summary>
        private static async Task CheckAndSendNotificationsAsync()
        {
            DateTime now = DateTime.Now;
            List<INotifiable> notificationsToRemove = new List<INotifiable>();

            foreach (INotifiable notification in Notifications)
            {
                if (notification.NotifyDate <= now)
                {
                    // Send the notification
                    try
                    {
                        await notification.Send();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending notification: {ex.Message}");
                    }

                    // Add the notification to the removal list
                    notificationsToRemove.Add(notification);
                }
            }

            // Remove sent notifications
            foreach (INotifiable notification in notificationsToRemove)
            {
                Notifications.Remove(notification);
            }
        }

        /// <summary>
        /// Adds a notification to be sent.
        /// </summary>
        /// <param name="getter">The user who should receive the notification.</param>
        /// <param name="info">The notification details.</param>
        public static void OnNotify(User? getter, NotificationInfo info)
        {
            ConsoleNotification consoleNotification = new ConsoleNotification(info.Title, info.Body, info.Date);
            EmailNotification emailNotification = new EmailNotification(info.Title, info.Body, getter?.Email ?? "none", info.Date);

            if (getter != null && Chats.ContainsKey(getter.TelegramNickname))
            {
                TelegramNotification telegramNotification = new TelegramNotification(Chats[getter.TelegramNickname], info.Title, info.Body, info.Date);
                Notifications.Add(telegramNotification);
            }

            Notifications.Add(consoleNotification);
            Notifications.Add(emailNotification);
        }

        /// <summary>
        /// Updates the chat ID associated with a user's Telegram nickname.
        /// </summary>
        /// <param name="username">The Telegram nickname of the user.</param>
        /// <param name="chatId">The chat ID to associate with the user.</param>
        public static void UpdateUserChatLink(string username, long chatId)
        {
            // Find the user by Telegram nickname
            User? user = Users.FirstOrDefault(u => u.TelegramNickname == username);

            if (user != null)
            {
                Chats[user.TelegramNickname] = chatId;
            }
        }
    }
}