using Spectre.Console;
using System.Security.Cryptography;
using System.Text;
using ToDoManagerStatistics;
using Messages;

namespace Library
{
    /// <summary>
    /// Represents a ToDo item with properties such as name, description, category, deadline, status, and priority.
    /// </summary>
    public class ToDo
    {
        private string _id;
        private string _name;
        private string _description;
        private string _category;
        private DateTime _created;
        private DateTime _deadlineDate;
        private ToDoStatus _status;
        private ToDoPriority _priority;

        /// <summary>
        /// Gets or sets the unique identifier for the ToDo item.
        /// </summary>
        public string Id { get => _id; set => _id = value; }

        /// <summary>
        /// Gets or sets the name of the ToDo item.
        /// </summary>
        public string Name { get => _name; set => _name = value; }

        /// <summary>
        /// Gets or sets the description of the ToDo item.
        /// </summary>
        public string Description { get => _description; set => _description = value; }

        /// <summary>
        /// Gets or sets the category of the ToDo item. If the category does not exist, it is added to the list of categories.
        /// </summary>
        public string Category
        {
            get => _category;
            set
            {
                if (!ToDoCategories.Categories.Contains(value))
                {
                    ToDoCategories.AddCategory(value);
                }
                _category = value;
            }
        }

        /// <summary>
        /// Event triggered when a notification is sent for the ToDo item.
        /// </summary>
        public event EventHandler<NotificationInfo> Notify;

        /// <summary>
        /// Gets or sets the creation date of the ToDo item.
        /// </summary>
        public DateTime Created { get => _created; set => _created = value; }

        /// <summary>
        /// Gets or sets the deadline date of the ToDo item. Ensures the deadline is after the creation date.
        /// </summary>
        public DateTime DeadlineDate
        {
            get => _deadlineDate;
            set
            {
                if (value > Created)
                {
                    _deadlineDate = value;
                }
                else
                {
                    _deadlineDate = DateTime.Now + TimeSpan.FromDays(1);
                }
            }
        }

        /// <summary>
        /// Gets or sets the status of the ToDo item. Triggers events when the status changes.
        /// </summary>
        public ToDoStatus Status
        {
            get => _status;
            set
            {
                if ((_status == ToDoStatus.Active || _status == ToDoStatus.Postponed || _status == ToDoStatus.Default) && value == ToDoStatus.Completed)
                {
                    OnStatusChanged.Invoke(this, new Report(DateTime.Today, true));
                }
                else if (_status == ToDoStatus.Completed &&
                         (value == ToDoStatus.Active || value == ToDoStatus.Postponed))
                {
                    OnStatusChanged.Invoke(this, new Report(DateTime.Today, false));
                }

                if (value == ToDoStatus.Postponed && _status != ToDoStatus.Completed)
                {
                    OnPostponed.Invoke(this, new ToDoInfo(Name));
                }

                _status = value;
            }
        }

        /// <summary>
        /// Gets or sets the priority of the ToDo item.
        /// </summary>
        public ToDoPriority Priority { get => _priority; set => _priority = value; }

        private event EventHandler<Report> OnStatusChanged = UserDataManager.CurrentUser.Statistics.OnToDoCompleted;
        private event EventHandler<ToDoInfo> OnPostponed = UserDataManager.CurrentUser.Statistics.OnPostponed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with default values.
        /// </summary>
        public ToDo()
        {
            Notify = (_, info) => UserDataManager.OnNotify(UserDataManager.CurrentUser, info);

            using (MD5 md5 = MD5.Create())
            {
                string input = Guid.NewGuid().ToString();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                _id = BitConverter.ToString(hash).Replace("-", "").Substring(0, 16);
            }
            _name = "MyToDo";
            _description = "This is a new ToDo";
            _category = "Personal";
            Created = DateTime.Now;
            DeadlineDate = DateTime.Now + TimeSpan.FromDays(1);
            Status = ToDoStatus.Default;
            Priority = ToDoPriority.Medium;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with specified values.
        /// </summary>
        /// <param name="name">The name of the ToDo item.</param>
        /// <param name="description">The description of the ToDo item.</param>
        /// <param name="category">The category of the ToDo item.</param>
        /// <param name="deadlineDate">The deadline date of the ToDo item.</param>
        /// <param name="status">The status of the ToDo item.</param>
        /// <param name="priority">The priority of the ToDo item.</param>
        public ToDo(string name, string description, string category, DateTime deadlineDate, ToDoStatus status, ToDoPriority priority) : this()
        {
            Name = name;
            Description = description;
            Category = category;
            DeadlineDate = deadlineDate;
            _status = status;
            Priority = priority;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class with specified values as strings.
        /// </summary>
        /// <param name="name">The name of the ToDo item.</param>
        /// <param name="description">The description of the ToDo item.</param>
        /// <param name="category">The category of the ToDo item.</param>
        /// <param name="deadlineDate">The deadline date of the ToDo item as a string.</param>
        /// <param name="status">The status of the ToDo item as a string.</param>
        /// <param name="priority">The priority of the ToDo item as a string.</param>
        public ToDo(string name, string description, string category, string deadlineDate, string status, string priority) : this()
        {
            Name = name;
            Description = description;
            Category = category;
            DeadlineDate = DateTime.Parse(deadlineDate);
            _status = (ToDoStatus)Enum.Parse(typeof(ToDoStatus), status, ignoreCase: true);
            Priority = (ToDoPriority)Enum.Parse(typeof(ToDoPriority), priority, ignoreCase: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class from a dictionary of properties.
        /// </summary>
        /// <param name="dict">A dictionary containing the properties of the ToDo item.</param>
        public ToDo(Dictionary<string, string> dict) : this()
        {
            if (dict.TryGetValue("Id", out string? id))
            {
                Id = id;
            }
            Name = dict["Name"];
            Description = dict["Description"];
            Category = dict["Category"];

            if (dict.TryGetValue("Created", out string? value))
            {
                Created = DateTime.Parse(value);
            }

            DeadlineDate = DateTime.Parse(dict["DeadlineDate"]);
            _status = (ToDoStatus)Enum.Parse(typeof(ToDoStatus), dict["Status"], ignoreCase: true);
            Priority = (ToDoPriority)Enum.Parse(typeof(ToDoPriority), dict["Priority"], ignoreCase: true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDo"/> class from a list of strings.
        /// </summary>
        /// <param name="strings">A list of strings containing the properties of the ToDo item.</param>
        public ToDo(List<string> strings) : this()
        {
            if (strings.Count != 7 && strings.Count != 8)
            {
                AnsiConsole.WriteLine(strings.Count);
                throw new Exception("Data is incorrect.");
            }
            else if (strings.Count == 8)
            {
                Id = strings[0];
                Name = strings[1];
                Description = strings[2];
                Category = strings[3];
                Created = DateTime.Parse(strings[4]);
                DeadlineDate = DateTime.Parse(strings[5]);
                _status = (ToDoStatus)Enum.Parse(typeof(ToDoStatus), strings[6], ignoreCase: true);
                Priority = (ToDoPriority)Enum.Parse(typeof(ToDoPriority), strings[7], ignoreCase: true);
            }
            else
            {
                Name = strings[0];
                Description = strings[1];
                Category = strings[2];
                Created = DateTime.Parse(strings[3]);
                DeadlineDate = DateTime.Parse(strings[4]);
                _status = (ToDoStatus)Enum.Parse(typeof(ToDoStatus), strings[5], ignoreCase: true);
                Priority = (ToDoPriority)Enum.Parse(typeof(ToDoPriority), strings[6], ignoreCase: true);
            }
        }

        /// <summary>
        /// Edits a specific field of the ToDo item.
        /// </summary>
        /// <param name="fieldName">The name of the field to edit.</param>
        /// <param name="value">The new value for the field.</param>
        public void Edit(string fieldName, object value)
        {
            try
            {
                switch (fieldName)
                {
                    case "DeadlineDate":
                        {
                            DeadlineDate = DateTime.Parse(value.ToString() ?? "");
                            break;
                        }
                    case "Status":
                        {
                            Status = (ToDoStatus)Enum.Parse(typeof(ToDoStatus), value.ToString() ?? "", ignoreCase: true);
                            break;
                        }
                    case "Priority":
                        {
                            Priority = (ToDoPriority)Enum.Parse(typeof(ToDoPriority), value.ToString() ?? "", ignoreCase: true);
                            break;
                        }
                    default:
                        {
                            if (GetType().GetProperties().Any(p => p.Name == fieldName))
                            {
                                GetType().GetProperty(fieldName)?.SetValue(this, value.ToString() ?? "");
                            }
                            else
                            {
                                throw new Exception($"Field {fieldName} is not found.");
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                AnsiConsole.WriteLine($"[red] Field {fieldName} input was incorrect! Please, try again.!");
            }
        }

        /// <summary>
        /// Converts the ToDo item to a list of strings for display in a table.
        /// </summary>
        /// <returns>A list of strings representing the ToDo item.</returns>
        public List<string> ToRow()
        {
            List<string> result = new();

            string[] colors =
            {
                "[grey]","[aqua]", "[royalblue1]", "[maroon]", "[slateblue1]", "[mediumpurple3]",
                "[mediumpurple3_1]", "[indianred]"
            };

            List<string?> fieldsVals = GetType().GetProperties().Select(x => x.Name)
                .Select(x => GetType().GetProperty(x)?.GetValue(this)?.ToString()).ToList();

            for (int i = 0; i < 8; i++)
            {
                result.Add(colors[i] + fieldsVals?.ElementAt(i) + "[/]");
            }

            return result;
        }

        /// <summary>
        /// Prints the ToDo item details in a formatted table.
        /// </summary>
        public void Print()
        {
            Table table = new Table()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[bold]Attribute[/]").Centered())
                .AddColumn(new TableColumn("[bold]Value[/]").Centered());

            table.AddRow("[bold]Name[/]", Name);
            table.AddRow("[bold]Description[/]", Description);
            table.AddRow("[bold]Category[/]", Category);
            table.AddRow("[bold]Created[/]", Created.ToString("g"));
            table.AddRow("[bold]Deadline[/]", DeadlineDate.ToString("g"));
            table.AddRow("[bold]Status[/]", Status.ToString());
            table.AddRow("[bold]Priority[/]", Priority.ToString());

            AnsiConsole.Write(table);
        }

        /// <summary>
        /// Gets the deadline date as a tuple of year, month, and day.
        /// </summary>
        /// <returns>A tuple containing the year, month, and day of the deadline.</returns>
        public Tuple<int, int, int> GetDeadLine()
        {
            return new Tuple<int, int, int>(DeadlineDate.Year, DeadlineDate.Month, DeadlineDate.Day);
        }

        /// <summary>
        /// Converts the ToDo item to a CSV-formatted string.
        /// </summary>
        /// <returns>A CSV-formatted string representing the ToDo item.</returns>
        public string ToCsv()
        {
            List<string?> fieldsVals = GetType().GetProperties().Select(x => x.Name)
                .Select(x => GetType().GetProperty(x)?.GetValue(this)?.ToString()).ToList();

            string result = string.Join(";", fieldsVals);

            return result;
        }

        /// <summary>
        /// Adds a notification for the ToDo item to be sent at a specified date.
        /// </summary>
        /// <param name="date">The date when the notification should be sent.</param>
        public void AddNotify(DateTime date)
        {
            string title = $"Reminder: Complete {Name} by {DeadlineDate}";
            string body = $"Hi {UserDataManager.CurrentUser?.Name}," + Environment.NewLine +
                          $"This is a friendly reminder to complete {Name} by {DeadlineDate}. " + Environment.NewLine +
                          $"Please ensure all necessary steps are taken to finish the task on time. " + Environment.NewLine +
                          $"If you have any questions or need assistance, don’t hesitate to reach out." + Environment.NewLine +
                          $"Thank you for staying on top of your responsibilities!" + Environment.NewLine +
                          $"Best regards," + Environment.NewLine + "Your ToDoManager App";

            Notify.Invoke(this, new NotificationInfo(title, body, date));
        }
    }
}