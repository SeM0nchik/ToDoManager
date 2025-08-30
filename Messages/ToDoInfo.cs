namespace Messages
{
    /// <summary>
    /// Informations about ToDo name.
    /// </summary>
    public sealed class ToDoInfo : EventArgs
    {
        public string Name { get; set; }
        
        public ToDoInfo(string name)
        {
            Name = name;
        }
    }
}