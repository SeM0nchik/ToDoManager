namespace Filters_and_Sorts
{
    /// <summary>
    /// Represents a filter that can be applied to a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items to filter.</typeparam>
    public class Filter<T> : IApplyable<T>
    {
        /// <summary>
        /// Gets or sets the name of the filter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the function used to filter items.
        /// </summary>
        public Func<T, bool> Function { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter{T}"/> class with default values.
        /// </summary>
        public Filter()
        {
            Name = "My filter";
            Function = _ => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter{T}"/> class with a specified name and filter function.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="function">The function used to filter items.</param>
        public Filter(string name, Func<T, bool> function)
        {
            Name = name;
            Function = function;
        }

        /// <summary>
        /// Applies the filter to the provided collection of items.
        /// </summary>
        /// <param name="items">The collection of items to filter.</param>
        /// <returns>A filtered collection of items.</returns>
        public IEnumerable<T> Apply(IEnumerable<T> items)
        {
            Console.WriteLine(Function);
            return items.Where(x => Function(x));
        }
    }
}