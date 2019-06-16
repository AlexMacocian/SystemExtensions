using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// Interface for queue implementations.
    /// </summary>
    /// <typeparam name="T">Provided type.</typeparam>
    public interface IQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// Returns the number of items in the queue.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Inserts item into the queue.
        /// </summary>
        /// <param name="item">Item to be inserted.</param>
        void Enqueue(T item);
        /// <summary>
        /// Remove the first item in the queue.
        /// </summary>
        /// <returns>First item in the queue.</returns>
        T Dequeue();
        /// <summary>
        /// Looks up the first item from the queue without removing it.
        /// </summary>
        /// <returns>First item from the queue.</returns>
        T Peek();
        /// <summary>
        /// Clears the contents of the queue.
        /// </summary>
        void Clear();
        /// <summary>
        /// Checks if queue contains an item.
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True if queue contains provided item. False otherwise.</returns>
        bool Contains(T item);
    }
}
