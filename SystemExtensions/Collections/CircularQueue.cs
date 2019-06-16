using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Implementation of a queue based on a circular array. Efficient implementation when capacity is known from before.
    /// </summary>
    /// <typeparam name="T">Provided type.</typeparam>
    class CircularQueue<T> : IQueue<T>
    {
        #region Fields
        int count;
        int capacity;
        int insertLocation;
        int readLocation;
        T[] items;
        #endregion
        #region Properties
        /// <summary>
        /// Returns the number of items in the queue.
        /// </summary>
        public int Count { get => count; }
        /// <summary>
        /// Capacity of the queue.
        /// </summary>
        public int Capacity { get => capacity;
            set
            {
                capacity = value;
                ///TO IMPLEMENT CREATION OF A NEW BUFFER
                throw new NotImplementedException();
            }
        }
        #endregion
        #region Constructors
        public CircularQueue()
        {
            count = 0;
            capacity = 10;
            items = new T[capacity];
        }
        public CircularQueue(int capacity)
        {
            this.capacity = capacity;
            count = 0;
            items = new T[capacity];
        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Clears the queue.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Checks if provided item is present into the queue.
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True if item is present. False otherwise.</returns>
        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Removes and returns the first item from the queue.
        /// </summary>
        /// <returns>First item from the queue.</returns>
        public T Dequeue()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Inserts provided item into the queue.
        /// </summary>
        /// <param name="item">Item to be inserted.</param>
        public void Enqueue(T item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Checks and returns the first item from the queue without removing it.
        /// </summary>
        /// <returns>The first item from the queue.</returns>
        public T Peek()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Private Methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
