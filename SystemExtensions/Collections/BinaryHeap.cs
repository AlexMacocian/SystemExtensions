using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Binary heap implementation.
    /// </summary>
    /// <typeparam name="T">Provided type.</typeparam>
    [Serializable]
    public sealed class BinaryHeap<T> : IEnumerable<T> where T : IComparable<T>
    {
        #region Fields
        T[] items;
        int capacity, count, initialCapacity;
        #endregion
        #region Properties
        /// <summary>
        /// Minimum value from the heap.
        /// </summary>
        public T Min
        {
            get
            {
                return items[1];
            }
        }
        /// <summary>
        /// Maximum value from the heap.
        /// </summary>
        public T Max
        {
            get
            {
                return items[count];
            }
        }
        /// <summary>
        /// Capacity of the heap.
        /// </summary>
        public int Capacity
        {
            get => capacity;
            set
            {
                if (value > capacity)
                {
                    Array.Resize(ref items, value);
                    capacity = value;
                }
            }
        }
        /// <summary>
        /// Number of elements in the heap.
        /// </summary>
        public int Count { get => count; }
        #endregion
        #region Constructors
        /// <summary>
        /// Constructor for a binary heap data structure.
        /// </summary>
        public BinaryHeap()
        {
            capacity = 10;
            initialCapacity = capacity;
            items = new T[capacity];
        }
        /// <summary>
        /// Constructor for a binary heap data structure.
        /// </summary>
        /// <param name="capacity">Initial capacity of the heap. Used for initial alocation of the array.</param>
        public BinaryHeap(int capacity)
        {
            this.capacity = capacity;
            initialCapacity = capacity;
            items = new T[capacity];
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Adds value to the queue.
        /// </summary>
        /// <param name="value">Value to be added.</param>
        public void Add(T value)
        {
            if (count == Capacity - 1)
            {
                Capacity = 2 * Capacity;
            }
            int position = ++count;
            for (; position > 1 && value.CompareTo(items[position / 2]) < 0; position = position / 2)
            {
                items[position] = items[position / 2];
            }
            items[position] = value;
        }
        /// <summary>
        /// Removes the item at the root. Throws exception if there are no items in the heap.
        /// </summary>
        /// <returns>Value removed.</returns>
        public T Remove()
        {
            if (count == 0)
            {
                throw new IndexOutOfRangeException("Heap is empty!");
            }
            T min = items[1];
            items[1] = items[count--];
            BubbleDown(1);
            return min;
        }
        /// <summary>
        /// Peeks at the item at the root. Throws exception if there are no items in the heap.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (count == 0)
            {
                throw new IndexOutOfRangeException("Heap is empty!");
            }
            T min = items[1];
            return min;
        }
        /// <summary>
        /// Return the heap structure as an array
        /// </summary>
        /// <returns>Array with values sorted as in heap</returns>
        public T[] ToArray()
        {
            T[] newArray = new T[count];
            Array.Copy(items, 1, newArray, 0, count);
            return newArray;
        }
        /// <summary>
        /// Determines whether the heap contains specified value
        /// </summary>
        /// <param name="value">Value to locate in the heap</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return items.Contains(value);
        }
        /// <summary>
        /// Clears the heap
        /// </summary>
        public void Clear()
        {
            count = 0;
        }
        /// <summary>
        /// Clears the heap.
        /// </summary>
        /// <param name="completeClear">Specifies if the underlying array should be cleared as well</param>
        public void Clear(bool completeClear)
        {
            count = 0;
            capacity = initialCapacity;
            items = new T[initialCapacity];
        }
        /// <summary>
        /// Returns an enumerator that iterates over the heap.
        /// </summary>
        /// <returns>Enumerator that iterates over the heap.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i + 1];
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
            T temp = items[index];
            int childIndex = 0;
            for (; 2 * index <= count; index = childIndex)
            {
                childIndex = 2 * index;
                if (childIndex != Count && items[childIndex].CompareTo(items[childIndex + 1]) > 0)
                {
                    childIndex++;
                }
                if (temp.CompareTo(items[childIndex]) > 0)
                {
                    items[index] = items[childIndex];
                }
                else
                {
                    break;
                }
            }
            items[index] = temp;
        }
        /// <summary>
        /// Build heap from unordered array
        /// </summary>
        private void BuildHeap()
        {
            for (int i = count / 2; i > 0; i--)
            {
                BubbleDown(i);
            }
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
}
