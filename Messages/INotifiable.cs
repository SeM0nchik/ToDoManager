namespace Messages
{
    /// <summary>
    /// Defines a contract for objects that can be notified, providing a title, body, notification date, and a method to send the notification.
    /// </summary>
    public interface INotifiable
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
        /// Sends the notification asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task Send();
    }
}