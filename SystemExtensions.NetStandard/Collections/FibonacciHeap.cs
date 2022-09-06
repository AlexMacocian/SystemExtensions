namespace System.Collections.Generic;

/// <summary>
/// Fibonacci Heap implementation.
/// </summary>
/// <typeparam name="T">Provided type</typeparam>
[Serializable]
public sealed class FibonacciHeap<T> : IEnumerable<T> where T : IComparable<T>
{
    #region Fields
    private FibonacciNode<T> root;
    private int count;
    #endregion
    #region Properties
    /// <summary>
    /// Count of values in the heap.
    /// </summary>
    public int Count
    {
        get
        {
            return this.count;
        }
    }
    /// <summary>
    /// Minimal value contained in the heap.
    /// </summary>
    public T Minimum
    {
        get
        {
            return this.root.Value;
        }
    }
    #endregion
    #region Constructors
    /// <summary>
    /// Constructor for Fibonacci heap data structure.
    /// </summary>
    public FibonacciHeap()
    {

    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Adds value to the heap.
    /// </summary>
    /// <param name="value">Value to be added.</param>
    public void Add(T value)
    {
        var node = new FibonacciNode<T>
        {
            Value = value,
            Marked = false,
            Child = null,
            Parent = null,
            Degree = 0
        };
        node.Previous = node.Next = node;
        this.root = this.Merge(this.root, node);
        this.count++;
    }
    /// <summary>
    /// Merge current heap with another heap. The other heap will be disposed at the end of this method.
    /// </summary>
    /// <param name="otherHeap">The heap to be merged with the current heap.</param>
    public void Merge(FibonacciHeap<T> otherHeap)
    {
        this.root = this.Merge(this.root, otherHeap.root);
        otherHeap.root = null;
        this.count += otherHeap.count;
    }
    /// <summary>
    /// Remove the minimum value from the heap.
    /// </summary>
    /// <returns>Minimum value.</returns>
    public T Remove()
    {
        var currentRoot = this.root;
        if (currentRoot != null)
        {
            this.root = this.RemoveMinimum(this.root);
            this.count--;
            return currentRoot.Value;
        }
        else
        {
            throw new IndexOutOfRangeException("Heap is empty!");
        }
    }
    /// <summary>
    /// Decrease the old value to a new provided value.
    /// </summary>
    /// <param name="oldValue">Old value used to find the node to have its key decreased.</param>
    /// <param name="value">New value to be assigned to the node.</param>
    public void DecreaseKey(T oldValue, T value)
    {
        var node = this.Find(this.root, oldValue);
        this.root = this.DecreaseKey(this.root, node, value);
    }
    /// <summary>
    /// Determines whether the heap contains a specified value.
    /// </summary>
    /// <param name="value">Value to locate in the heap.</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        return this.Find(this.root, value) != null;
    }
    /// <summary>
    /// Clears the heap.
    /// </summary>
    public void Clear()
    {
        this.count = 0;
        this.Remove(this.root);
        this.root.Next = this.root.Previous = this.root.Parent = this.root.Child = null;
        this.root = null;
    }
    /// <summary>
    /// Return the heap structure as an array. Array is not sorted other than the
    /// actual structure of the heap.
    /// </summary>
    /// <returns>Array with values from the heap.</returns>
    public T[] ToArray()
    {
        if (this.count == 0)
        {
            return null;
        }

        var array = new T[this.count];
        if (this.count == 1)
        {
            array[0] = this.root.Value;
            return array;
        }
        else
        {
            var index = 0;
            this.RecursiveFillArray(this.root, ref array, ref index);
            return array;
        }
    }
    /// <summary>
    /// Enumerator that iterates over the heap. Note that the values are not sorted in any way.
    /// </summary>
    /// <returns>Enumerator that iterates over the heap.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return this.GetEnumerator(this.root);
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Recursively traverse the heap and copy its contents to an array.
    /// </summary>
    /// <param name="currentNode">Current node.</param>
    /// <param name="array">Array to be filled with contents of heap.</param>
    /// <param name="index">Index of the next unintialized element in the array.</param>
    private void RecursiveFillArray(FibonacciNode<T> currentNode, ref T[] array, ref int index)
    {
        var oldNode = currentNode;
        do
        {
            array[index] = currentNode.Value;
            index++;
            if (currentNode.HasChildren())
            {
                this.RecursiveFillArray(currentNode.Child, ref array, ref index);
            }

            currentNode = currentNode.Previous;
        } while (currentNode != oldNode);
    }
    /// <summary>
    /// Recursively enumerates over the tree.
    /// </summary>
    /// <param name="currentNode">Current node in the iteration.</param>
    private IEnumerator<T> GetEnumerator(FibonacciNode<T> currentNode)
    {
        var queue = new Queue<FibonacciNode<T>>();
        queue.Enqueue(currentNode);
        while (queue.Count > 0)
        {
            currentNode = queue.Dequeue();
            var oldNode = currentNode;
            do
            {
                yield return currentNode.Value;
                if (currentNode.HasChildren())
                {
                    queue.Enqueue(currentNode.Child);
                }

                currentNode = currentNode.Previous;
            } while (currentNode != oldNode);
        }
    }
    /// <summary>
    /// Recursively remove the node and its children from the heap.
    /// </summary>
    /// <param name="node">Node to be removed.</param>
    private void Remove(FibonacciNode<T> node)
    {
        if (node != null)
        {
            var current = node;
            do
            {
                this.Remove(current.Child);
                if (current.Parent != null)
                {
                    current.Parent.Child = null;
                }

                current = current.Next;
            } while (current != node);
            current.Next = current.Previous = current.Child = current.Parent = null;
        }
    }
    /// <summary>
    /// Merge two heaps into a larger heap.
    /// </summary>
    /// <param name="node1">Root of first heap.</param>
    /// <param name="node2">Root of second heap.</param>
    private FibonacciNode<T> Merge(FibonacciNode<T> node1, FibonacciNode<T> node2)
    {
        if (node1 == null)
        {
            return node2;
        }

        if (node2 == null)
        {
            return node1;
        }

        if (node1.Value.CompareTo(node2.Value) > 0)
        {
            var temp = node1;
            node1 = node2;
            node2 = temp;
        }

        var node1Next = node1.Next;
        var node2Prev = node2.Previous;
        node1.Next = node2;
        node2.Previous = node1;
        node1Next.Previous = node2Prev;
        node2Prev.Next = node1Next;
        return node1;
    }
    /// <summary>
    /// Adds child to the parent.
    /// </summary>
    /// <param name="parent">Parent node to accept child.</param>
    /// <param name="child">Child node to be added to the parent.</param>
    private void AddChild(FibonacciNode<T> parent, FibonacciNode<T> child)
    {
        child.Previous = child.Next = child;
        child.Parent = parent;
        parent.Degree++;
        parent.Child = this.Merge(parent.Child, child);
    }
    /// <summary>
    /// Removes the parent of the specified node.
    /// </summary>
    /// <param name="node">Node to be removed from its parent.</param>
    private void RemoveParent(FibonacciNode<T> node)
    {
        if (node == null)
        {
            return;
        }

        var current = node;
        do
        {
            current.Marked = false;
            current.Parent = null;
            current = current.Next;
        } while (current != node);
    }
    /// <summary>
    /// Removes the minimum node from the provided tree.
    /// </summary>
    /// <param name="node">Root of the provided tree.</param>
    /// <returns></returns>
    private FibonacciNode<T> RemoveMinimum(FibonacciNode<T> node)
    {
        this.RemoveParent(node.Child);
        if (node.Next == node)
        {
            node = node.Child;
        }
        else
        {
            node.Next.Previous = node.Previous;
            node.Previous.Next = node.Next;
            node = this.Merge(node.Next, node.Child);
        }

        if (node == null)
        {
            return node;
        }

        var trees = new FibonacciNode<T>[64];
        while (true)
        {
            if (trees[node.Degree] != null)
            {
                var t = trees[node.Degree];
                if (t == node)
                {
                    break;
                }

                trees[node.Degree] = null;
                if (node.Value.CompareTo(t.Value) < 0)
                {
                    t.Previous.Next = t.Next;
                    t.Next.Previous = t.Previous;
                    this.AddChild(node, t);
                }
                else
                {
                    t.Previous.Next = t.Next;
                    t.Next.Previous = t.Previous;
                    if (node.Next == node)
                    {
                        t.Next = t.Previous = t;
                        this.AddChild(t, node);
                        node = t;
                    }
                    else
                    {
                        node.Previous.Next = t;
                        node.Next.Previous = t;
                        t.Next = node.Next;
                        t.Previous = node.Previous;
                        this.AddChild(t, node);
                        node = t;
                    }
                }

                continue;
            }
            else
            {
                trees[node.Degree] = node;
            }

            node = node.Next;
        }

        var min = node;
        var start = node;
        do
        {
            if (node.Value.CompareTo(min.Value) < 0)
            {
                min = node;
            }

            node = node.Next;
        } while (node != start);
        return min;
    }
    /// <summary>
    /// Cut node from heap.
    /// </summary>
    /// <param name="root">Root of heap.</param>
    /// <param name="node">Node to be cut.</param>
    /// <returns></returns>
    private FibonacciNode<T> Cut(FibonacciNode<T> root, FibonacciNode<T> node)
    {
        if (node.Next == node)
        {
            node.Parent.Child = null;
        }
        else
        {
            node.Next.Previous = node.Previous;
            node.Previous.Next = node.Next;
            node.Parent.Child = node.Next;
        }

        node.Next = node.Previous = node;
        node.Marked = false;
        return this.Merge(root, node);
    }
    /// <summary>
    /// Decrease value of provided node substituting it with the provided value.
    /// </summary>
    /// <param name="root">Root of the heap.</param>
    /// <param name="node">Node to have value decreased.</param>
    /// <param name="value">New value of the node. It is only applied if the value is lower than the previous value.</param>
    /// <returns></returns>
    private FibonacciNode<T> DecreaseKey(FibonacciNode<T> root, FibonacciNode<T> node, T value)
    {
        if (node.Value.CompareTo(value) < 0)
        {
            return root;
        }

        node.Value = value;
        if (node.Parent != null)
        {
            if (node.Value.CompareTo(node.Parent.Value) < 0)
            {
                root = this.Cut(root, node);
                var parent = node.Parent;
                node.Parent = null;
                while (parent != null && parent.Marked)
                {
                    root = this.Cut(root, parent);
                    node = parent;
                    parent = node.Parent;
                    node.Parent = null;
                }

                if (parent != null && parent.Parent != null)
                {
                    parent.Marked = true;
                }
            }
        }
        else
        {
            if (node.Value.CompareTo(root.Value) < 0)
            {
                root = node;
            }
        }

        return root;
    }
    /// <summary>
    /// Find the node that has the specified value in the heap.
    /// </summary>
    /// <param name="root">Root of the heap.</param>
    /// <param name="value">Value to be found.</param>
    /// <returns></returns>
    private FibonacciNode<T> Find(FibonacciNode<T> root, T value)
    {
        var node = root;
        if (node == null)
        {
            return null;
        }

        do
        {
            if (node.Value.CompareTo(value) == 0)
            {
                return node;
            }

            var ret = this.Find(node.Child, value);
            if (ret != null)
            {
                return ret;
            }

            node = node.Next;
        } while (node != root);
        return null;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion
}
[Serializable]
internal sealed class FibonacciNode<T>
{
    #region Fields
    private FibonacciNode<T> previous;
    private FibonacciNode<T> next;
    private FibonacciNode<T> child;
    private FibonacciNode<T> parent;
    private T value;
    private int degree;
    private bool marked;
    #endregion
    #region Properties
    public FibonacciNode<T> Previous
    {
        get
        {
            return this.previous;
        }
        set
        {
            this.previous = value;
        }
    }
    public FibonacciNode<T> Next
    {
        get
        {
            return this.next;
        }
        set
        {
            this.next = value;
        }
    }
    public FibonacciNode<T> Child
    {
        get
        {
            return this.child;
        }
        set
        {
            this.child = value;
        }
    }
    public FibonacciNode<T> Parent
    {
        get
        {
            return this.parent;
        }
        set
        {
            this.parent = value;
        }
    }
    public bool Marked
    {
        get
        {
            return this.marked;
        }
        set
        {
            this.marked = value;
        }
    }
    public T Value
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value;
        }
    }
    public int Degree
    {
        get
        {
            return this.degree;
        }
        set
        {
            this.degree = value;
        }
    }
    #endregion
    #region Public Methods
    public bool HasChildren()
    {
        return this.child != null;
    }
    public bool HasParent()
    {
        return this.parent != null;
    }
    #endregion
}
