using System.Linq;

namespace System.Collections.Generic;

/// <summary>
/// Binary heap implementation.
/// </summary>
/// <typeparam name="T">Provided type.</typeparam>
[Serializable]
public sealed class BinaryHeap<T> : IEnumerable<T> where T : IComparable<T>
{
    #region Fields
    T[] items;
    private int capacity;
    private int count;
    private readonly int initialCapacity;
    #endregion
    #region Properties
    /// <summary>
    /// Minimum value from the heap.
    /// </summary>
    public T Min
    {
        get
        {
            return this.items[1];
        }
    }
    /// <summary>
    /// Maximum value from the heap.
    /// </summary>
    public T Max
    {
        get
        {
            return this.items[this.count];
        }
    }
    /// <summary>
    /// Capacity of the heap.
    /// </summary>
    public int Capacity
    {
        get => this.capacity;
        set
        {
            if (value > this.capacity)
            {
                Array.Resize(ref this.items, value);
                this.capacity = value;
            }
        }
    }
    /// <summary>
    /// Number of elements in the heap.
    /// </summary>
    public int Count { get => this.count; }
    #endregion
    #region Constructors
    /// <summary>
    /// Constructor for a binary heap data structure.
    /// </summary>
    public BinaryHeap()
    {
        this.capacity = 10;
        this.initialCapacity = this.capacity;
        this.items = new T[this.capacity];
    }
    /// <summary>
    /// Constructor for a binary heap data structure.
    /// </summary>
    /// <param name="capacity">Initial capacity of the heap. Used for initial alocation of the array.</param>
    public BinaryHeap(int capacity)
    {
        this.capacity = capacity;
        this.initialCapacity = capacity;
        this.items = new T[capacity];
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Adds value to the queue.
    /// </summary>
    /// <param name="value">Value to be added.</param>
    public void Add(T value)
    {
        if (this.count == this.Capacity - 1)
        {
            this.Capacity = 2 * this.Capacity;
        }

        var position = ++this.count;
        for (; position > 1 && value.CompareTo(this.items[position / 2]) < 0; position /= 2)
        {
            this.items[position] = this.items[position / 2];
        }

        this.items[position] = value;
    }
    /// <summary>
    /// Removes the item at the root. Throws exception if there are no items in the heap.
    /// </summary>
    /// <returns>Value removed.</returns>
    public T Remove()
    {
        if (this.count == 0)
        {
            throw new IndexOutOfRangeException("Heap is empty!");
        }

        var min = this.items[1];
        this.items[1] = this.items[this.count--];
        this.BubbleDown(1);
        return min;
    }
    /// <summary>
    /// Peeks at the item at the root. Throws exception if there are no items in the heap.
    /// </summary>
    /// <returns></returns>
    public T Peek()
    {
        if (this.count == 0)
        {
            throw new IndexOutOfRangeException("Heap is empty!");
        }

        var min = this.items[1];
        return min;
    }
    /// <summary>
    /// Return the heap structure as an array
    /// </summary>
    /// <returns>Array with values sorted as in heap</returns>
    public T[] ToArray()
    {
        var newArray = new T[this.count];
        Array.Copy(this.items, 1, newArray, 0, this.count);
        return newArray;
    }
    /// <summary>
    /// Determines whether the heap contains specified value
    /// </summary>
    /// <param name="value">Value to locate in the heap</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        return this.items.Contains(value);
    }
    /// <summary>
    /// Clears the heap
    /// </summary>
    public void Clear()
    {
        this.count = 0;
    }
    /// <summary>
    /// Clears the heap.
    /// </summary>
    /// <param name="completeClear">Specifies if the underlying array should be cleared as well</param>
    public void Clear(bool completeClear)
    {
        this.count = 0;
        if (completeClear)
        {
            this.capacity = this.initialCapacity;
            this.items = new T[this.initialCapacity];
        }
    }
    /// <summary>
    /// Returns an enumerator that iterates over the heap.
    /// </summary>
    /// <returns>Enumerator that iterates over the heap.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (var i = 0; i < this.count; i++)
        {
            yield return this.items[i + 1];
        }
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Bubble the specified element to its position
    /// </summary>
    /// <param name="index">Index of element to bubble</param>
    private void BubbleDown(int index)
    {
        var temp = this.items[index];
        int childIndex;
        for (; 2 * index <= this.count; index = childIndex)
        {
            childIndex = 2 * index;
            if (childIndex != this.Count && this.items[childIndex].CompareTo(this.items[childIndex + 1]) > 0)
            {
                childIndex++;
            }

            if (temp.CompareTo(this.items[childIndex]) > 0)
            {
                this.items[index] = this.items[childIndex];
            }
            else
            {
                break;
            }
        }

        this.items[index] = temp;
    }
    /// <summary>
    /// Implementation of IEnumerator.
    /// </summary>
    /// <returns>Enumerator over the array.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion
}
