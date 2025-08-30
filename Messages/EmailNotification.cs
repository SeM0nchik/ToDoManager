using MailKit.Net.Smtp;
using MimeKit;

namespace Messages
{
    /// <summary>
    /// Represents an email notification that can be sent via SMTP.
    /// </summary>
    public class EmailNotification : INotifiable
    {
        /// <summary>
        /// Gets or sets the title of the email notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body content of the email notification.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the notification should be sent.
        /// </summary>
        public DateTime NotifyDate { get; set; }

        /// <summary>
        /// Gets or sets the recipient's email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotification"/> class with default values.
        /// </summary>
        public EmailNotification()
        {
            Title = "My notification";
            Body = "This is a new notification";
            To = "to@mail.com";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailNotification"/> class with specified values.
        /// </summary>
        /// <param name="title">The title of the email notification.</param>
        /// <param name="body">The body content of the email notification.</param>
        /// <param name="to">The recipient's email address.</param>
        /// <param name="notifyDate">The date and time when the notification should be sent.</param>
        public EmailNotification(string title, string body, string to, DateTime notifyDate)
        {
            Title = title;
            Body = body;
            To = to;
            NotifyDate = notifyDate;
        }

        /// <summary>
        /// Sends the email notification using an SMTP client.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Send()
        {
            // Create the email message
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("ToDo Manager", "todomanager2025@mail.ru"));
            message.To.Add(new MailboxAddress("Recipient Name", To));
            message.Subject = Title;
            message.Body = new TextPart("plain") { Text = Body };

            // Configure and use the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                // Log connection
                Console.WriteLine("Connecting to the server...");
                client.Connect("smtp.mail.ru", 465, true);

                // Log authentication
                Console.WriteLine("Authenticating...");
                client.Authenticate("todomanager2025@mail.ru", "eWCz9ExPHbDyQjQJef8c");

                // Log sending
                Console.WriteLine($"Sending email to {To}");
                await client.SendAsync(message);

                // Disconnect from the server
                client.Disconnect(true);
            }
        }
    }
}