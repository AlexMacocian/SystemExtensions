using System;
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
    public class Treap<T>
    {
        #region Fields
        private Random randomGen;
        private Item<T> root;
        private Comparison<T> comparator;
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
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor for treap.
        /// </summary>
        /// <param name="comparator">Comparator method used to compare values.</param>
        public Treap(Comparison<T> comparator)
        {
            randomGen = new Random();
            this.comparator = comparator;
        }
        #endregion
        #region Private Methods
        private Item<T> InsertNode(Item<T> node, T key)
        {
            if(node == null) {
                node = new Item<T>(key, randomGen.Next(0, 100));
                return node;
            }
            else if (comparator(key, node.Key) <= 0)
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
            if(comparator.Invoke(key, node.Key) < 0)
            {
                node.Left = RemoveNode(node.Left, key);
            }
            else if(comparator.Invoke(key, node.Key) > 0)
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
                if(comparator.Invoke(node.Key, key) < 0)
                {
                    Item<T> found = Find(node.Left, key);
                    if(found == null)
                    {
                        found = Find(node.Right, key);
                    }
                    return found;
                }
                else if(comparator.Invoke(node.Key, key) > 0)
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
        #endregion
        #region Public Methods
        /// <summary>
        /// Inserts value into treap.
        /// </summary>
        /// <param name="value">Value to be inserted.</param>
        public void Insert(T value)
        {
            root = InsertNode(root, value);
            count++;
        }
        /// <summary>
        /// Removes value from treap.
        /// </summary>
        /// <param name="value">Value to be removed.</param>
        public void Remove(T value)
        {
            root = RemoveNode(root, value);
            count--;
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
            if(root != null)
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
        #endregion
    }

    internal class Item<T>
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
}
