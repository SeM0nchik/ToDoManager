namespace Library
{
    /// <summary>
    /// ToDo Categories
    /// </summary>
    public static class ToDoCategories
    {
        private static  List<string> _categories;
        
        public static List<string> Categories => _categories;
        
        static ToDoCategories()
        {
            _categories = new List<string> { "Работа", "Личное", "Учёба" };
        }

        /// <summary>
        /// Method that adds category.
        /// </summary>
        /// <param name="category">New category to add.</param>
        public static void AddCategory(string category)
        {
            _categories.Add(category);
        }
    }
}