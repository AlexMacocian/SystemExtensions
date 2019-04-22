using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Fibonacci Heap implementation. Implements IDisposable to clear the sub structure of the heap.
    /// </summary>
    /// <typeparam name="T">Provided type</typeparam>
    public class FibonacciHeap<T> : IDisposable
    {
        #region Fields
        private FibonacciNode<T> root;
        private Comparison<T> comparator;
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
                return count;
            }
        }
        /// <summary>
        /// Minimal value contained in the heap.
        /// </summary>
        public T Minimum
        {
            get
            {
                return root.Value;
            }
        }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor for Fibonacci heap data structure.
        /// </summary>
        /// <param name="comparator">Function used to compare the elements.</param>
        public FibonacciHeap(Comparison<T> comparator)
        {
            this.comparator = comparator;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Inserts value into the heap.
        /// </summary>
        /// <param name="value">Value to be inserted.</param>
        public void Insert(T value)
        {
            FibonacciNode<T> node = new FibonacciNode<T>();
            node.Value = value;
            node.Previous = node.Next = node;
            node.Degree = 0;
            node.Marked = false;
            node.Child = null;
            node.Parent = null;
            root = Merge(root, node);
            count++;
        }
        /// <summary>
        /// Merge current heap with another heap. The other heap will be disposed at the end of this method.
        /// </summary>
        /// <param name="otherHeap">The heap to be merged with the current heap.</param>
        public void Merge(FibonacciHeap<T> otherHeap)
        {
            root = Merge(root, otherHeap.root);
            otherHeap.root = null;
            count += otherHeap.count;
            otherHeap.Dispose();
        }
        /// <summary>
        /// Remove the minimum value from the heap.
        /// </summary>
        /// <returns>Minimum value.</returns>
        public T RemoveMinimum()
        {
            FibonacciNode<T> currentRoot = root;
            if (currentRoot != null)
            {
                root = RemoveMinimum(root);
                count--;
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
            FibonacciNode<T> node = Find(root, oldValue);
            root = DecreaseKey(root, node, value);
        }
        /// <summary>
        /// Determines whether the heap contains a specified value.
        /// </summary>
        /// <param name="value">Value to locate in the heap.</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return Find(root, value) != null;
        }
        /// <summary>
        /// Clears the heap.
        /// </summary>
        public void Clear()
        {
            count = 0;
            Remove(root);
            root.Next = root.Previous = root.Parent = root.Child = null;
            root = null;
        }
        /// <summary>
        /// Return the heap structure as an array. Array is not sorted other than the
        /// actual structure of the heap.
        /// </summary>
        /// <returns>Array with values from the heap.</returns>
        public T[] ToArray()
        {
            if(count == 0)
            {
                return null;
            }
            T[] array = new T[count];
            if (count == 1)
            {
                array[0] = root.Value;
                return array;
            }
            else
            {
                int index = 0;
                RecursiveFillArray(root, ref array, ref index);
                return array;
            }
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
            FibonacciNode<T> oldNode = currentNode;
            do
            {
                array[index] = currentNode.Value;
                index++;
                if (currentNode.HasChildren())
                {
                    RecursiveFillArray(currentNode.Child, ref array, ref index);
                }
                currentNode = currentNode.Previous;
            } while (currentNode != oldNode);
        }
        /// <summary>
        /// Recursively remove the node and its children from the heap.
        /// </summary>
        /// <param name="node">Node to be removed.</param>
        private void Remove(FibonacciNode<T> node)
        {
            if(node != null)
            {
                FibonacciNode<T> current = node;
                do
                {
                    Remove(current.Child);
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
            if(node1 == null)
            {
                return node2;
            }
            if(node2 == null)
            {
                return node1;
            }
            if(comparator(node1.Value, node2.Value) > 0)
            {
                FibonacciNode<T> temp = node1;
                node1 = node2;
                node2 = temp;
            }
            FibonacciNode<T> node1Next = node1.Next;
            FibonacciNode<T> node2Prev = node2.Previous;
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
            parent.Child = Merge(parent.Child, child);
        }
        /// <summary>
        /// Removes the parent of the specified node.
        /// </summary>
        /// <param name="node">Node to be removed from its parent.</param>
        private void RemoveParent(FibonacciNode<T> node)
        {
            if(node == null)
            {
                return;
            }
            FibonacciNode<T> current = node;
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
            RemoveParent(node.Child);
            if(node.Next == node)
            {
                node = node.Child;
            }
            else
            {
                node.Next.Previous = node.Previous;
                node.Previous.Next = node.Next;
                node = Merge(node.Next, node.Child);
            }
            if(node == null)
            {
                return node;
            }

            FibonacciNode<T>[] trees = new FibonacciNode<T>[64];
            while (true)
            {
                if(trees[node.Degree] != null)
                {
                    FibonacciNode<T> t = trees[node.Degree];
                    if(t == node)
                    {
                        break;
                    }
                    trees[node.Degree] = null;
                    if(comparator(node.Value, t.Value) < 0)
                    {
                        t.Previous.Next = t.Next;
                        t.Next.Previous = t.Previous;
                        AddChild(node, t);
                    }
                    else
                    {
                        t.Previous.Next = t.Next;
                        t.Next.Previous = t.Previous;
                        if(node.Next == node)
                        {
                            t.Next = t.Previous = t;
                            AddChild(t, node);
                            node = t;
                        }
                        else
                        {
                            node.Previous.Next = t;
                            node.Next.Previous = t;
                            t.Next = node.Next;
                            t.Previous = node.Previous;
                            AddChild(t, node);
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
            FibonacciNode<T> min = node;
            FibonacciNode<T> start = node;
            do
            {
                if(comparator(node.Value, min.Value) < 0)
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
            if(node.Next == node)
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
            return Merge(root, node);
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
            if(comparator(node.Value, value) < 0)
            {
                return root;
            }
            node.Value = value;
            if(node.Parent != null)
            {
                if(comparator(node.Value, node.Parent.Value) < 0)
                {
                    root = Cut(root, node);
                    FibonacciNode<T> parent = node.Parent;
                    node.Parent = null;
                    while(parent != null && parent.Marked)
                    {
                        root = Cut(root, parent);
                        node = parent;
                        parent = node.Parent;
                        node.Parent = null;
                    }
                    if(parent != null && parent.Parent != null)
                    {
                        parent.Marked = true;
                    }
                }
            }
            else
            {
                if(comparator(node.Value, root.Value) < 0)
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
            FibonacciNode<T> node = root;
            if(node == null)
            {
                return null;
            }
            do
            {
                if(comparator(node.Value, value) == 0)
                {
                    return node;
                }
                FibonacciNode<T> ret = Find(node.Child, value);
                if(ret != null)
                {
                    return ret;
                }
                node = node.Next;
            } while (node != root);
            return null;
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false;
        /// <summary>
        /// Disposes of the contents of the heap. Called by the public Dispose().
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (root != null)
                {
                    if (disposing)
                    {
                        Remove(root);
                    }
                    root.Previous = root.Next = root.Parent = root.Child = null;
                    root = null;
                }

                disposedValue = true;
            }
        }
        /// <summary>
        /// Disposes of the contents of the heap.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    internal class FibonacciNode<T>
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
                return previous;
            }
            set
            {
                previous = value;
            }
        }
        public FibonacciNode<T> Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }
        public FibonacciNode<T> Child
        {
            get
            {
                return child;
            }
            set
            {
                child = value;
            }
        }
        public FibonacciNode<T> Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }
        public bool Marked
        {
            get
            {
                return marked;
            }
            set
            {
                marked = value;
            }
        }
        public T Value
        {
            get
            {
                return value;
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
                return degree;
            }
            set
            {
                degree = value;
            }
        }
        #endregion
        #region Public Methods
        public bool HasChildren()
        {
            return child != null;
        }
        public bool HasParent()
        {
            return parent != null;
        }
        #endregion
    }
}
