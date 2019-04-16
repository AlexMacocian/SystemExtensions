using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Binary heap implementation
    /// </summary>
    public class BinaryHeap<T>
    {
        #region Fields
        T[] items;
        Comparison<T> comparator;
        int capacity, count, initialCapacity;
        #endregion
        #region Properties
        public T Min
        {
            get
            {
                return items[1];
            }
        }
        public T Max
        {
            get
            {
                return items[count];
            }
        }
        public int Capacity { get => capacity;
            set
            {
                if(value > capacity)
                {
                    Array.Resize(ref items, value);
                    capacity = value;
                }
            }
        }
        public int Count { get => count; }
        #endregion
        #region Constructors
        public BinaryHeap(Comparison<T> comparator)
        {
            capacity = 10;
            initialCapacity = capacity;
            items = new T[capacity];
            this.comparator = comparator;
        }
        public BinaryHeap(Comparison<T> comparator, int capacity)
        {
            this.capacity = capacity;
            initialCapacity = capacity;
            items = new T[capacity];
            this.comparator = comparator;
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Insertion operation into the queue
        /// </summary>
        /// <param name="value">Value to be inserted</param>
        public void Insert(T value)
        {
            if(count == Capacity - 1)
            {
                Capacity = 2 * Capacity;
            }
            int position = ++count;
            for (; position > 1 && comparator.Invoke(value, items[position / 2]) < 0; position = position / 2)
            {
                items[position] = items[position / 2];
            }
            items[position] = value;
        }
        /// <summary>
        /// Deletes the item at the root. Throws exception if there are no items in the heap.
        /// </summary>
        /// <returns></returns>
        public T DeleteMin()
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
        /// Function to return the heap structure as an array
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
            for(; 2*index <= count; index = childIndex)
            {
                childIndex = 2 * index;
                if(childIndex != Count && comparator.Invoke(items[childIndex], items[childIndex + 1]) > 0)
                {
                    childIndex++;
                }
                if(comparator.Invoke(temp, items[childIndex]) > 0)
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
            for(int i = count / 2; i > 0; i--)
            {
                BubbleDown(i);
            }
        }    
        #endregion
    }
}
