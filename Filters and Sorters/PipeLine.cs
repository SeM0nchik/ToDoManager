using System.Collections;
using Filters_and_Sorts;

/// <summary>
/// Represents a pipeline that applies a series of filters and sorters to a collection of items.
/// </summary>
/// <typeparam name="T">The type of items in the pipeline.</typeparam>
public class Pipeline<T> : IEnumerable
{
    /// <summary>
    /// Gets the list of operations (filters and sorters) in the pipeline.
    /// </summary>
    public List<IApplyable<T>> PipeLine { get; private set; } = new List<IApplyable<T>>();

    /// <summary>
    /// Gets the number of operations in the pipeline.
    /// </summary>
    public int Length => PipeLine.Count;

    /// <summary>
    /// Adds a filter to the pipeline.
    /// </summary>
    /// <param name="filter">The filter to add.</param>
    public void AddFilter(Filter<T> filter)
    {
        PipeLine.Add(filter);
    }

    /// <summary>
    /// Adds a sorter to the pipeline.
    /// </summary>
    /// <param name="sorter">The sorter to add.</param>
    public void AddSorter(Sorter<T> sorter)
    {
        PipeLine.Add(sorter);
    }

    /// <summary>
    /// Removes all operations from the pipeline with the specified name.
    /// </summary>
    /// <param name="name">The name of the operations to remove.</param>
    public void RemoveName(string name)
    {
        PipeLine = PipeLine.Where(x => x.Name != name).ToList();
    }

    /// <summary>
    /// Applies all operations in the pipeline to the provided collection of items.
    /// </summary>
    /// <param name="items">The collection of items to process.</param>
    /// <returns>The modified collection of items after applying all operations.</returns>
    public IEnumerable<T> Apply(IEnumerable<T> items)
    {
        foreach (IApplyable<T> operation in PipeLine)
        {
            if (operation is Filter<T> filter)
            {
                items = filter.Apply(items);
            }
            else if (operation is Sorter<T> sorter)
            {
                items = sorter.Apply(items);
            }
        }
        return items;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the operations in the pipeline.
    /// </summary>
    /// <returns>An enumerator for the pipeline operations.</returns>
    public IEnumerator GetEnumerator()
    {
        return PipeLine.GetEnumerator();
    }
}