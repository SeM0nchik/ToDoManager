namespace Messages
{
    /// <summary>
    /// Class that represents all valuable information about notifications.
    /// </summary>
    public class NotificationInfo : EventArgs
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        
        public NotificationInfo(string title, string body, DateTime date)
        {
            Title = title;
            Body = body;
            Date = date;
        }
        
    }
}