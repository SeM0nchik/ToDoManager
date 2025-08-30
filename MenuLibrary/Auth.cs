using System.Text.RegularExpressions;
using Spectre.Console;
using ToDoManagerStatistics;

namespace MenuLibrary
{
    /// <summary>
    /// Class responsible for user authentication (registration and login).
    /// </summary>
    public class Auth : Menu
    {
        /// <summary>
        /// Event triggered when the user subscribes or unsubscribes from Telegram notifications.
        /// </summary>
        private event EventHandler<bool>? OnTelegramSubscribe =
            (x, y) => UserDataManager.CurrentUser.TelegramSubscribeToTelegram(x, y);

        /// <summary>
        /// Initializes a new instance of the <see cref="Auth"/> class.
        /// </summary>
        /// <param name="name">The name of the authentication menu.</param>
        public Auth(string name) : base(name, new[] { "Sign up", "Sign in" })
        {
            Actions = new Action[] { Register, Login };
        }

        /// <summary>
        /// Registers a new user by prompting for their name, Telegram nickname, email, and password.
        /// </summary>
        public void Register()
        {
            AnsiConsole.MarkupLine("[underline green]Registering a new user[/]");

            string name = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your name:")
                    .PromptStyle("blue")
                    .ValidationErrorMessage("[red]Name cannot be empty[/]")
                    .Validate(nickname => !string.IsNullOrWhiteSpace(nickname)));

            string telegramNickname = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your Telegram nickname:")
                    .PromptStyle("blue")
                    .ValidationErrorMessage("[red]Nickname cannot be empty[/]")
                    .Validate(x => !string.IsNullOrWhiteSpace(x) && x.Contains('@')));

            string email = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your email:")
                    .PromptStyle("blue")
                    .ValidationErrorMessage("[red]Email is incorrect[/]")
                    .Validate(x => !string.IsNullOrWhiteSpace(x) && IsValidEmail(x)));

            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your password:")
                    .PromptStyle("blue")
                    .Secret()
                    .ValidationErrorMessage("[red]Password cannot be empty[/]")
                    .Validate(password => !string.IsNullOrWhiteSpace(password)));

            User user = new User(name, telegramNickname, email, password);

            UserDataManager.CurrentUser = user;
            UserDataManager.Users.Add(user);

            _ = user.EmailSubscribeToEmail();

            AnsiConsole.MarkupLine("[green]Registration completed successfully![/]");
            AnsiConsole.WriteLine();

            OfferTelegramSubscription();
        }

        /// <summary>
        /// Logs in an existing user by verifying their email and password.
        /// </summary>
        public void Login()
        {
            AnsiConsole.MarkupLine("[underline green]User login[/]");

            string email = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your email:")
                    .PromptStyle("blue")
                    .ValidationErrorMessage("[red]Email cannot be empty[/]")
                    .Validate(email => !string.IsNullOrWhiteSpace(email)));

            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter your password:")
                    .PromptStyle("blue")
                    .Secret()
                    .ValidationErrorMessage("[red]Password cannot be empty[/]")
                    .Validate(password => !string.IsNullOrWhiteSpace(password)));

            User? user = UserDataManager.Users.Find(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                AnsiConsole.MarkupLine($"[green]Welcome, {user.Name}![/]");
                UserDataManager.CurrentUser = user;
                OnTelegramSubscribe += user.TelegramSubscribeToTelegram;

                // Offer Telegram subscription
                OfferTelegramSubscription();
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid email or password[/]");
            }
            AnsiConsole.WriteLine();
        }

        /// <summary>
        /// Validates an email address using a regular expression.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email is valid; otherwise, false.</returns>
        private bool IsValidEmail(string email)
        {
            Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
            return emailRegex.IsMatch(email);
        }

        /// <summary>
        /// Prompts the user to subscribe to Telegram notifications and triggers the corresponding event.
        /// </summary>
        private void OfferTelegramSubscription()
        {
            bool wantsSubscription = AnsiConsole.Confirm("Do you want to subscribe to Telegram notifications?");
            if (wantsSubscription)
            {
                OnTelegramSubscribe?.Invoke(this, true);
                AnsiConsole.MarkupLine($"[green]Please press /start in the Telegram bot to confirm your subscription.[/]");
            }
            else
            {
                OnTelegramSubscribe?.Invoke(this, false);
                AnsiConsole.MarkupLine("[yellow]Telegram notifications are disabled.[/]");
            }
        }
    }
}