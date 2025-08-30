using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Spectre.Console;

namespace Messages
{
    /// <summary>
    /// Represents a notification that can be sent via Telegram.
    /// </summary>
    public class TelegramNotification : EventArgs, INotifiable
    {
        /// <summary>
        /// The bot token used to authenticate with the Telegram API.
        /// </summary>
        public static string BotToken = "7856546848:AAGRmfkeBDI258rLxNCBgJ4KYdMeo8gt7_I";

        /// <summary>
        /// The Telegram bot client used to send messages.
        /// </summary>
        private static TelegramBotClient _botClient;

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
        /// Gets or sets the chat ID where the notification will be sent.
        /// </summary>
        public long ChatId { get; set; }


        static TelegramNotification()
        {
            _botClient = new TelegramBotClient(BotToken);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TelegramNotification"/> class.
        /// </summary>
        /// <param name="chatId">The chat ID where the notification will be sent.</param>
        /// <param name="title">The title of the notification.</param>
        /// <param name="body">The body content of the notification.</param>
        /// <param name="notifyDate">The date and time when the notification should be sent.</param>
        public TelegramNotification(long chatId, string title, string body, DateTime notifyDate)
        {
            ChatId = chatId;
            Title = title;
            Body = body;
            NotifyDate = notifyDate;
        }

        /// <summary>
        /// Sends the notification via Telegram.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Send()
        {
            try
            {
                await _botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: $"*{Title}*\n\n{Body}",
                    parseMode: ParseMode.Markdown
                );
            }
            catch (ApiRequestException ex)
            {
                AnsiConsole.MarkupLine($"[red]Error sending message: {ex.Message}[/]");
            }
        }
    }
}