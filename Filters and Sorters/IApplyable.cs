namespace Filters_and_Sorts
{
    /// <summary>
    /// Defines an interface for applying operations to a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of items to apply operations to.</typeparam>
    public interface IApplyable<T>
    {
        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Applies the operation to the provided collection of items.
        /// </summary>
        /// <param name="items">The collection of items to apply the operation to.</param>
        /// <returns>A modified collection of items based on the operation.</returns>
        public IEnumerable<T> Apply(IEnumerable<T> items);
    }
}