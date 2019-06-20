using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Treap implementation.
    /// </summary>
    /// <typeparam name="T">Provided type.</typeparam>
    public class Treap<T> : ICollection<T> where T : IComparable<T>
    {
        #region Fields
        private class Item<T>
        {
            public T Key;
            public int Priority;
            public Item<T> Left, Right;
            public Item(T key, int priority)
            {
                Key = key;
                Priority = priority;
                Left = null;
                Right = null;
            }
        }
        private Random randomGen;
        private Item<T> root;
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
                return count;
            }
        }

        public bool IsReadOnly => throw new NotImplementedException();
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor for treap.
        /// </summary>
        /// <param name="comparator">Comparator method used to compare values.</param>
        public Treap()
        {
            randomGen = new Random();
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Adds value to the treap.
        /// </summary>
        /// <param name="value">Value to be added.</param>
        public void Add(T value)
        {
            root = InsertNode(root, value);
            count++;
        }
        /// <summary>
        /// Removes value from treap.
        /// </summary>
        /// <param name="value">Value to be removed.</param>
        public bool Remove(T value)
        {
            root = RemoveNode(root, value);
            count--;
            return true;
        }
        /// <summary>
        /// Clears the treap.
        /// </summary>
        public void Clear()
        {
            Clear(root);
            root = null;
            count = 0;
        }
        /// <summary>
        /// Determines whether the treap contains the specified value.
        /// </summary>
        /// <param name="value">Value to locate in the treap.</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return Find(root, value) != null;
        }
        /// <summary>
        /// Returns the treap structure as an ordered array.
        /// </summary>
        /// <returns>Ordered array containing the values stored in the treap.</returns>
        public T[] ToArray()
        {
            if (root != null)
            {
                T[] array = new T[count];
                int index = 0;
                ToArray(root, ref array, ref index);
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
            ToArray(root, ref array, ref arrayIndex);
        }
        /// <summary>
        /// Enumerator that iterates over the treap. Note that the values are not guaranteed to be sorted.
        /// </summary>
        /// <returns>Enumerator that iterates over the treap.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(root);
        }
        #endregion
        #region Private Methods
        private Item<T> InsertNode(Item<T> node, T key)
        {
            if(node == null) {
                node = new Item<T>(key, randomGen.Next(0, 100));
                return node;
            }
            else if (key.CompareTo(node.Key) <= 0)
            {
                node.Left = InsertNode(node.Left, key);
                if(node.Left.Priority > node.Priority)
                {
                    node = RotateRight(node);
                }
            }
            else
            {
                node.Right = InsertNode(node.Right, key);
                if(node.Right.Priority > node.Priority)
                {
                    node = RotateLeft(node);
                }
            }
            return node;
        }
        private Item<T> RemoveNode(Item<T> node, T key)
        {
            if(node == null)
            {
                return node; 
            }
            if(key.CompareTo(node.Key) < 0)
            {
                node.Left = RemoveNode(node.Left, key);
            }
            else if(key.CompareTo(node.Key) > 0)
            {
                node.Right = RemoveNode(node.Right, key);
            }      
            else if (node.Left == null)
            {
                node = node.Right;
            }
            else if(node.Right == null)
            {
                node = node.Left;
            }
            else if(node.Left.Priority < node.Right.Priority)
            {
                node = RotateLeft(node);
                node.Left = RemoveNode(node.Left, key);
            }
            else
            {
                node = RotateRight(node);
                node.Right = RemoveNode(node.Right, key);
            }
            return node;
        }
        private Item<T> RotateRight(Item<T> node)
        {
            Item<T> temp = node.Left, temp2 = temp.Right;
            temp.Right = node;
            node.Left = temp2;
            return temp;
        }
        private Item<T> RotateLeft(Item<T> node)
        {
            Item<T> temp = node.Right, temp2 = temp.Left;
            temp.Left = node;
            node.Right = temp2;
            return temp;
        }
        private void Clear(Item<T> node)
        {
            if(node.Left != null)
            {
                Clear(node.Left);
            }
            if(node.Right != null)
            {
                Clear(node.Right);
            }
            node.Left = node.Right = null;
        }
        private Item<T> Find(Item<T> node, T key)
        {
            if(node == null)
            {
                return node;
            }
            else
            {
                if(node.Key.CompareTo(key) < 0)
                {
                    Item<T> found = Find(node.Left, key);
                    if(found == null)
                    {
                        found = Find(node.Right, key);
                    }
                    return found;
                }
                else if(node.Key.CompareTo(key) > 0)
                {
                    Item<T> found = Find(node.Right, key);
                    if (found == null)
                    {
                        found = Find(node.Left, key);
                    }
                    return found;
                }
                else
                {
                    return node;
                }
            }
        }
        private void ToArray(Item<T> node, ref T[] array, ref int index)
        {
            if(node != null)
            {
                ToArray(node.Left, ref array, ref index);
                array[index] = node.Key;
                index++;
                ToArray(node.Right, ref array, ref index);
            }
        }
        private IEnumerator<T> GetEnumerator(Item<T> currentNode)
        {
            Queue<Item<T>> queue = new Queue<Item<T>>();
            queue.Enqueue(currentNode);
            while(queue.Count > 0)
            {
                currentNode = queue.Dequeue();
                yield return currentNode.Key;
                if(currentNode.Left != null)
                {
                    queue.Enqueue(currentNode.Left);
                }
                if(currentNode.Right != null)
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
}
