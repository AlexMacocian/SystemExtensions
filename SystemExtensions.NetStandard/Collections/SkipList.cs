namespace System.Collections.Generic;

/// <summary>
/// Skip list implementation.
/// </summary>
/// <typeparam name="T">Provided type.</typeparam>
[Serializable]
public sealed class SkipList<T> : ICollection<T> where T : IComparable<T>
{
    #region Fields
    [Serializable]
    private class NodeSet<TKey>
    {
        public TKey Key;
        public int Level;
        public NodeSet<TKey>[] Next;

        public NodeSet(TKey key, int level)
        {
            this.Key = key;
            this.Level = level;
            this.Next = new NodeSet<TKey>[level + 1];
        }
    }
    private int count;
    private readonly Random random;
    private readonly NodeSet<T> head;
    private readonly NodeSet<T> end;
    private readonly int maxLevel = 10;
    private int level;
    #endregion
    #region Properties
    /// <summary>
    /// Number of elements in the list.
    /// </summary>
    public int Count { get => this.count; }
    /// <summary>
    /// Specifies if the collection can be modified.
    /// </summary>
    public bool IsReadOnly { get; set; }
    #endregion
    #region Constructors
    /// <summary>
    /// Creates a new instance of SkipList collection.
    /// </summary>
    /// <param name="maxLevel">Maximum level of the skip list.</param>
    public SkipList(int maxLevel = 10)
    {
        this.maxLevel = maxLevel;
        this.random = new Random();
        this.head = new NodeSet<T>(default!, maxLevel);
        this.end = this.head;
        for (var i = 0; i <= maxLevel; i++)
        {
            this.head.Next[i] = this.end;
        }
    }

    #endregion
    #region Public Methods
    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    public void Add(T item)
    {
        var curNode = this.head;
        var newLevel = 0;
        while (this.random.Next(0, 2) > 0 && newLevel < this.maxLevel)
        {
            newLevel++;
        }

        if (newLevel > this.level)
        {
            this.level = newLevel;
        }

        var newNode = new NodeSet<T>(item, newLevel);
        for (var i = 0; i <= newLevel; i++)
        {
            if (i > curNode.Level)
            {
                curNode = this.head;
            }

            while (curNode.Next[i] != this.end && curNode.Next[i].Key.CompareTo(item) < 0)
            {
                curNode = curNode.Next[i];
            }

            newNode.Next[i] = curNode.Next[i];
            curNode.Next[i] = newNode;
        }

        this.count++;
    }
    /// <summary>
    /// Removes provided item from the collection.
    /// </summary>
    /// <param name="item">Item to be removed.</param>
    /// <returns>True if removal was successful.</returns>
    public bool Remove(T item)
    {
        var removed = false;
        var curNode = this.head;
        for (var i = 0; i <= this.maxLevel; i++)
        {
            if (i > curNode.Level)
            {
                curNode = this.head;
            }

            while (curNode.Next[i] != this.end && curNode.Next[i].Key.CompareTo(item) < 0)
            {
                curNode = curNode.Next[i];
            }

            if (curNode.Next[i].Key.CompareTo(item) == 0) //Item is present on this level
            {
                curNode.Next[i] = curNode.Next[i].Next[i];
                removed = true;
            }
            else
            {
                break;
            }
        }

        if (removed)
        {
            this.count--;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Clears the collection.
    /// </summary>
    public void Clear()
    {
        for (var i = 0; i < this.maxLevel; i++)
        {
            this.head.Next[i] = this.end;
        }

        this.count = 0;
    }
    /// <summary>
    /// Checks if item is present in the collection.
    /// </summary>
    /// <param name="item">Item to be checked.</param>
    /// <returns>True if item is present in the collection.</returns>
    public bool Contains(T item)
    {
        if (this.Find(item) != null)
        {
            return true;
        }

        return false;
    }
    /// <summary>
    /// Copies the skip list contents onto the provided array.
    /// </summary>
    /// <param name="array">Array to hold the values from the list.</param>
    /// <param name="arrayIndex">Index to start insertion in the array.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        var node = this.head.Next[0];
        while (node != this.end)
        {
            array[arrayIndex] = node.Key;
            arrayIndex++;
            node = node.Next[0];
        }
    }
    /// <summary>
    /// Copies the elements from the collection into an array.
    /// </summary>
    /// <returns>Array filled with elements from the collection.</returns>
    public T[] ToArray()
    {
        var array = new T[this.count];
        var index = 0;
        var curNode = this.head.Next[0];
        while (curNode != this.end)
        {
            array[index] = curNode.Key;
            index++;
            curNode = curNode.Next[0];
        }

        return array;
    }
    /// <summary>
    /// Enumerator that iterates over the collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        var curNode = this.head.Next[0];
        while (curNode != this.end)
        {
            yield return curNode.Key;
            curNode = curNode.Next[0];
        }
    }
    #endregion
    #region Private Methods
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    private NodeSet<T> Find(T key)
    {
        var curNode = this.head;

        for (var i = this.level; i >= 0; i--)
        {
            while (curNode.Next[i] != this.end)
            {
                if (curNode.Next[i].Key.CompareTo(key) > 0)
                {
                    break;
                }
                else if (curNode.Next[i].Key.CompareTo(key) == 0)
                {
                    return curNode.Next[i];
                }

                curNode = curNode.Next[i];
            }
        }

        curNode = curNode.Next[0];
        if (curNode != this.end && curNode.Key.CompareTo(key) == 0)
        {
            return curNode;
        }

        return default!;
    }
    #endregion
}
