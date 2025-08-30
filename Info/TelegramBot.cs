
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ToDoManagerStatistics
{
    /// <summary>
    /// Manages the Telegram bot for handling user interactions and notifications.
    /// </summary>
    public static class TelegramBotManager
    {
        /// <summary>
        /// The API token for the Telegram bot.
        /// </summary>
        private static readonly string Api = "7856546848:AAGRmfkeBDI258rLxNCBgJ4KYdMeo8gt7_I";

        /// <summary>
        /// The Telegram bot client instance.
        /// </summary>
        private static ITelegramBotClient? _botClient;

        /// <summary>
        /// The cancellation token source for stopping the bot.
        /// </summary>
        private static CancellationTokenSource? _cts;

        /// <summary>
        /// Starts the Telegram bot and begins processing incoming messages.
        /// </summary>
        public static void StartBot()
        {
            _botClient = new TelegramBotClient(Api);
            _cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions();

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            Console.WriteLine("Bot started.");
        }

        /// <summary>
        /// Stops the Telegram bot.
        /// </summary>
        public static void StopBot()
        {
            _cts?.Cancel();
            Console.WriteLine("Bot stopped.");
        }

        /// <summary>
        /// Handles incoming updates (messages) from users.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="update">The incoming update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message != null && update.Message.Type == MessageType.Text)
            {
                string? messageText = update.Message.Text;
                long chatId = update.Message.Chat.Id;
                string username = "@" + update.Message.Chat.Username;

                if (messageText == "/start")
                {
                    // Find the user by Telegram username
                    User? user = UserDataManager.Users.FirstOrDefault(u => u.TelegramNickname == username);
                    if (user != null)
                    {
                        if (user.IsSubscribedToTelegram)
                        {
                            // Update or add the chat link
                            UserDataManager.UpdateUserChatLink(username, chatId);
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "You have successfully subscribed to Telegram notifications!",
                                cancellationToken: cancellationToken
                            );
                        }
                        else
                        {
                            // Subscription not allowed
                            await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Please enable Telegram notifications in the application.",
                                cancellationToken: cancellationToken
                            );
                        }
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: "User not found. Please register in the application first.",
                            cancellationToken: cancellationToken
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Handles errors that occur during bot operation.
        /// </summary>
        /// <param name="botClient">The Telegram bot client.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API error: {apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}