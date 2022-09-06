namespace System.Collections.Generic;

/// <summary>
/// Treap implementation.
/// </summary>
/// <typeparam name="T">Provided type.</typeparam>
[Serializable]
public sealed class Treap<T> : ICollection<T> where T : IComparable<T>
{
    #region Fields
    [Serializable]
    private class Node<TKey>
    {
        public TKey Key;
        public int Priority;
        public Node<TKey> Left, Right;
        public Node(TKey key, int priority)
        {
            this.Key = key;
            this.Priority = priority;
            this.Left = null;
            this.Right = null;
        }
    }
    private readonly Random randomGen;
    private Node<T> root;
    private int count;
    #endregion
    #region Properties
    /// <summary>
    /// Count of values in the treap.
    /// </summary>
    public int Count
    {
        get
        {
            return this.count;
        }
    }
    /// <summary>
    /// Not implemented.
    /// </summary>
    public bool IsReadOnly => false;
    #endregion
    #region Constructors
    /// <summary>
    /// Constructor for treap.
    /// </summary>
    public Treap()
    {
        this.randomGen = new Random();
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Adds value to the treap.
    /// </summary>
    /// <param name="value">Value to be added.</param>
    public void Add(T value)
    {
        this.root = this.InsertNode(this.root, value);
        this.count++;
    }
    /// <summary>
    /// Removes value from treap.
    /// </summary>
    /// <param name="value">Value to be removed.</param>
    public bool Remove(T value)
    {
        this.root = this.RemoveNode(this.root, value);
        this.count--;
        return true;
    }
    /// <summary>
    /// Clears the treap.
    /// </summary>
    public void Clear()
    {
        this.Clear(this.root);
        this.root = null;
        this.count = 0;
    }
    /// <summary>
    /// Determines whether the treap contains the specified value.
    /// </summary>
    /// <param name="value">Value to locate in the treap.</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        return this.Find(this.root, value) != null;
    }
    /// <summary>
    /// Returns the treap structure as an ordered array.
    /// </summary>
    /// <returns>Ordered array containing the values stored in the treap.</returns>
    public T[] ToArray()
    {
        if (this.root != null)
        {
            var array = new T[this.count];
            var index = 0;
            this.ToArray(this.root, ref array, ref index);
            return array;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Copy the treap into the provided array.
    /// </summary>
    /// <param name="array">Array to be populated with the values contained in the array.</param>
    /// <param name="arrayIndex">Starting index of the array.</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        this.ToArray(this.root, ref array, ref arrayIndex);
    }
    /// <summary>
    /// Enumerator that iterates over the treap. Note that the values are not guaranteed to be sorted.
    /// </summary>
    /// <returns>Enumerator that iterates over the treap.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return this.GetEnumerator(this.root);
    }
    #endregion
    #region Private Methods
    private Node<T> InsertNode(Node<T> node, T key)
    {
        if (node == null)
        {
            node = new Node<T>(key, this.randomGen.Next(0, 100));
            return node;
        }
        else if (key.CompareTo(node.Key) <= 0)
        {
            node.Left = this.InsertNode(node.Left, key);
            if (node.Left.Priority > node.Priority)
            {
                node = this.RotateRight(node);
            }
        }
        else
        {
            node.Right = this.InsertNode(node.Right, key);
            if (node.Right.Priority > node.Priority)
            {
                node = this.RotateLeft(node);
            }
        }

        return node;
    }
    private Node<T> RemoveNode(Node<T> node, T key)
    {
        if (node == null)
        {
            return node;
        }

        if (key.CompareTo(node.Key) < 0)
        {
            node.Left = this.RemoveNode(node.Left, key);
        }
        else if (key.CompareTo(node.Key) > 0)
        {
            node.Right = this.RemoveNode(node.Right, key);
        }
        else if (node.Left == null)
        {
            node = node.Right;
        }
        else if (node.Right == null)
        {
            node = node.Left;
        }
        else if (node.Left.Priority < node.Right.Priority)
        {
            node = this.RotateLeft(node);
            node.Left = this.RemoveNode(node.Left, key);
        }
        else
        {
            node = this.RotateRight(node);
            node.Right = this.RemoveNode(node.Right, key);
        }

        return node;
    }
    private Node<T> RotateRight(Node<T> node)
    {
        Node<T> temp = node.Left, temp2 = temp.Right;
        temp.Right = node;
        node.Left = temp2;
        return temp;
    }
    private Node<T> RotateLeft(Node<T> node)
    {
        Node<T> temp = node.Right, temp2 = temp.Left;
        temp.Left = node;
        node.Right = temp2;
        return temp;
    }
    private void Clear(Node<T> node)
    {
        if (node.Left != null)
        {
            this.Clear(node.Left);
        }

        if (node.Right != null)
        {
            this.Clear(node.Right);
        }

        node.Left = node.Right = null;
    }
    private Node<T> Find(Node<T> node, T key)
    {
        if (node == null)
        {
            return node;
        }
        else
        {
            if (node.Key.CompareTo(key) < 0)
            {
                var found = this.Find(node.Left, key);
                if (found == null)
                {
                    found = this.Find(node.Right, key);
                }

                return found;
            }
            else if (node.Key.CompareTo(key) > 0)
            {
                var found = this.Find(node.Right, key);
                if (found == null)
                {
                    found = this.Find(node.Left, key);
                }

                return found;
            }
            else
            {
                return node;
            }
        }
    }
    private void ToArray(Node<T> node, ref T[] array, ref int index)
    {
        if (node != null)
        {
            this.ToArray(node.Left, ref array, ref index);
            array[index] = node.Key;
            index++;
            this.ToArray(node.Right, ref array, ref index);
        }
    }
    private IEnumerator<T> GetEnumerator(Node<T> currentNode)
    {
        var queue = new Queue<Node<T>>();
        queue.Enqueue(currentNode);
        while (queue.Count > 0)
        {
            currentNode = queue.Dequeue();
            yield return currentNode.Key;
            if (currentNode.Left != null)
            {
                queue.Enqueue(currentNode.Left);
            }

            if (currentNode.Right != null)
            {
                queue.Enqueue(currentNode.Right);
            }
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion
}
