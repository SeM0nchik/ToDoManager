namespace Messages
{
    /// <summary>
    /// A report about todo beeing done or not.
    /// </summary>
    public sealed class Report : EventArgs
    {
        public DateTime Date {  get; private set; }
        public bool IsDone { get; private set; }
        public Report(DateTime date, bool isDone)
        {
            Date = date;
            IsDone = isDone;
        }
    }
}