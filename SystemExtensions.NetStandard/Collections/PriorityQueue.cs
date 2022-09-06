namespace System.Collections.Generic;

/// <summary>
/// Priority Queue data structure. The implementation is based on an array-based implementation of Binary Heap.
/// Exposes some of the functionality of the Binary Heap as a queue.
/// </summary>
/// <typeparam name="T">Provided type.</typeparam>
[Serializable]
public sealed class PriorityQueue<T> : IQueue<T> where T : IComparable<T>
{
    #region Fields
    private readonly BinaryHeap<T> binaryHeap;
    #endregion
    #region Properties
    /// <summary>
    /// Returns the number of elements stored into the queue.
    /// </summary>
    public int Count
    {
        get
        {
            return this.binaryHeap.Count;
        }
    }
    #endregion
    #region Constructors
    /// <summary>
    /// Constructor for priority queue data structure.
    /// </summary>
    public PriorityQueue()
    {
        this.binaryHeap = new BinaryHeap<T>();
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Add provided value to the queue.
    /// </summary>
    /// <param name="value">Value to be added to the queue.</param>
    public void Enqueue(T value)
    {
        this.binaryHeap.Add(value);
    }
    /// <summary>
    /// Pops the queue and removes the highest priority value from the queue.
    /// </summary>
    /// <returns>Highest priority value from the queue</returns>
    public T Dequeue()
    {
        if (this.Count > 0)
        {
            return this.binaryHeap.Remove();
        }
        else
        {
            throw new IndexOutOfRangeException("Queue is empty!");
        }
    }
    /// <summary>
    /// Looks up the highest priority value from the queue. Doesn't alter the queue in any way.
    /// </summary>
    /// <returns>Highest priority value from the queue.</returns>
    public T Peek()
    {
        return this.binaryHeap.Min;
    }
    /// <summary>
    /// Clears the queue contents, removing any value stored into the queue.
    /// </summary>
    public void Clear()
    {
        this.binaryHeap.Clear();
    }
    /// <summary>
    /// Checks if queue contains provided item.
    /// </summary>
    /// <param name="item">Item to be checked.</param>
    /// <returns>True if queue contains the provided item. False otherwise.</returns>
    public bool Contains(T item)
    {
        return this.binaryHeap.Contains(item);
    }
    /// <summary>
    /// Returns an enumerator that iterates over the queue.
    /// </summary>
    /// <returns>Enumerator that iterates over the queue.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return this.binaryHeap.GetEnumerator();
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Necesarry for the implementatio of IQueue.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion
}
