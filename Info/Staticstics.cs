using Messages;

namespace ToDoManagerStatistics
{
    /// <summary>
    /// Provides functionality to collect and analyze statistics about ToDo tasks.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// A dictionary to track the number of completed tasks by date.
        /// </summary>
        private readonly Dictionary<DateTime, int> _completed;

        /// <summary>
        /// A dictionary to track the number of times tasks have been postponed by task name.
        /// </summary>
        private readonly Dictionary<string, int> _mostPostponed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Statistics"/> class.
        /// </summary>
        public Statistics()
        {
            _completed = new Dictionary<DateTime, int>();
            _mostPostponed = new Dictionary<string, int>();
        }

        /// <summary>
        /// Updates statistics when a task is postponed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="info">The information about the postponed task.</param>
        public void OnPostponed(object? sender, ToDoInfo info)
        {
            string name = info.Name;
            if (!_mostPostponed.TryAdd(name, 1))
            {
                _mostPostponed[name]++;
            }
        }

        /// <summary>
        /// Updates statistics when a task is completed or uncompleted.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="report">The report containing completion details.</param>
        public void OnToDoCompleted(object? sender, Report report)
        {
            DateTime date = report.Date;
            bool isCompleted = report.IsDone;

            if (isCompleted)
            {
                if (!_completed.TryAdd(date, 1))
                {
                    _completed[date] += 1;
                }
            }
            else
            {
                if (!_completed.TryAdd(date, -1))
                {
                    _completed[date]--;
                }
            }
        }

        /// <summary>
        /// Retrieves statistics for the specified year.
        /// </summary>
        /// <param name="date">The date representing the year to analyze.</param>
        /// <returns>A dictionary where the key is the month and the value is the number of completed tasks.</returns>
        public Dictionary<Months, int> YearStatistics(DateTime date)
        {
            return _completed.Where(x => x.Key.Year == date.Year)
                .ToDictionary(x => (Months)x.Key.Month, x => x.Value);
        }

        /// <summary>
        /// Retrieves statistics for the specified month.
        /// </summary>
        /// <param name="date">The date representing the month to analyze.</param>
        /// <returns>A dictionary where the key is the day and the value is the number of completed tasks.</returns>
        public Dictionary<int, int> MonthStatistics(DateTime date)
        {
            return _completed.Where(x => x.Key.Month == date.Month)
                .ToDictionary(x => x.Key.Day, x => x.Value);
        }

        /// <summary>
        /// Retrieves statistics for the week containing the specified date.
        /// </summary>
        /// <param name="date">The date representing the week to analyze.</param>
        /// <returns>A dictionary where the key is the day of the week and the value is the number of completed tasks.</returns>
        public Dictionary<DayOfWeek, int> WeekStatistics(DateTime date)
        {
            DateTime startOfWeek = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Sunday);

            Dictionary<DayOfWeek, int> result = new Dictionary<DayOfWeek, int>();

            foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
            {
                int todo;
                _completed.TryGetValue(startOfWeek, out todo);
                result.Add(day, todo);

                startOfWeek = startOfWeek.AddDays(1);
            }

            return result;
        }

        /// <summary>
        /// Retrieves the most postponed tasks.
        /// </summary>
        /// <returns>A dictionary where the key is the task name and the value is the number of times it was postponed.</returns>
        public Dictionary<string, int> GetMostPostponed()
        {
            return _mostPostponed;
        }
    }
}