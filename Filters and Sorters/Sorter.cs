using Library;

namespace Filters_and_Sorts
{
    /// <summary>
    /// Represents a sorter that can order a collection of items based on a specified field.
    /// </summary>
    /// <typeparam name="T">The type of items to sort.</typeparam>
    public class Sorter<T> : IApplyable<T>
    {
        /// <summary>
        /// Gets or sets the name of the sorter.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the field name to sort the items by.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sort order should be reversed.
        /// </summary>
        public bool Reverse { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sorter{T}"/> class with the specified name, field, and sort order.
        /// </summary>
        /// <param name="name">The name of the sorter.</param>
        /// <param name="field">The field name to sort the items by.</param>
        /// <param name="reverse">A value indicating whether the sort order should be reversed. Default is <c>false</c>.</param>
        public Sorter(string name, string field, bool reverse = false)
        {
            Name = name;
            Field = field;
            Reverse = reverse;
        }

        /// <summary>
        /// Applies the sorting operation to the provided collection of items.
        /// </summary>
        /// <param name="items">The collection of items to sort.</param>
        /// <returns>A sorted collection of items.</returns>
        public IEnumerable<T> Apply(IEnumerable<T> items)
        {
            IOrderedEnumerable<T> res = items.OrderBy(x => typeof(ToDo).GetProperty(Field)?.GetValue(x));
            if (Reverse)
            {
                return res.Reverse();
            }
            return res;
        }
    }
}