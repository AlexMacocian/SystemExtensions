using System;
using System.Collections;
using System.Collections.Generic;

namespace SystemExtensions.Collections
{
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
                Next = new NodeSet<TKey>[level + 1];
            }
        }
        private int count;
        private Random random;
        private NodeSet<T> head;
        private NodeSet<T> end;
        private int maxLevel = 10;
        private int level;
        #endregion
        #region Properties
        /// <summary>
        /// Number of elements in the list.
        /// </summary>
        public int Count { get => count; }
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
            random = new Random();
            head = new NodeSet<T>(default(T), maxLevel);
            end = head;
            for (int i = 0; i <= maxLevel; i++)
            {
                head.Next[i] = end;
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
            NodeSet<T> curNode = head;
            int newLevel = 0;
            while (random.Next(0, 2) > 0 && newLevel < maxLevel)
            {
                newLevel++;
            }
            if (newLevel > level)
            {
                level = newLevel;
            }
            NodeSet<T> newNode = new NodeSet<T>(item, newLevel);
            for (var i = 0; i <= newLevel; i++)
            {
                if (i > curNode.Level)
                {
                    curNode = head;
                }
                while (curNode.Next[i] != end && curNode.Next[i].Key.CompareTo(item) < 0)
                {
                    curNode = curNode.Next[i];
                }
                newNode.Next[i] = curNode.Next[i];
                curNode.Next[i] = newNode;
            }
            count++;
        }
        /// <summary>
        /// Removes provided item from the collection.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns>True if removal was successful.</returns>
        public bool Remove(T item)
        {
            bool removed = false;
            NodeSet<T> curNode = head;
            for (var i = 0; i <= maxLevel; i++)
            {
                if (i > curNode.Level)
                {
                    curNode = head;
                }
                while (curNode.Next[i] != end && curNode.Next[i].Key.CompareTo(item) < 0)
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
                count--;
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
            for (int i = 0; i < maxLevel; i++)
            {
                head.Next[i] = end;
            }
            count = 0;
        }
        /// <summary>
        /// Checks if item is present in the collection.
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True if item is present in the collection.</returns>
        public bool Contains(T item)
        {
            if (Find(item) != null)
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
            NodeSet<T> node = head.Next[0];
            while (node != end)
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
            T[] array = new T[count];
            int index = 0;
            NodeSet<T> curNode = head.Next[0];
            while (curNode != end)
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
            NodeSet<T> curNode = head.Next[0];
            while (curNode != end)
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
            NodeSet<T> curNode = head;

            for (int i = level; i >= 0; i--)
            {
                while (curNode.Next[i] != end)
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
            if (curNode != end && curNode.Key.CompareTo(key) == 0)
            {
                return curNode;
            }
            return null;
        }
        #endregion
    }
}
