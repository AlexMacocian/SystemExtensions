using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    public class PriorityQueue<T>
    {
        #region Fields
        private BinaryHeap<T> binaryHeap;
        #endregion

        #region Properties
        public int Count
        {
            get
            {
                return binaryHeap.Count;
            }
        }
        #endregion

        #region Constructors
        public PriorityQueue(Comparison<T> comparator)
        {
            binaryHeap = new BinaryHeap<T>(comparator);
        }
        #endregion

        #region Public Methods
        public void Enqueue(T value)
        {
            binaryHeap.Insert(value);
        }

        public T Dequeue()
        {
            if (Count > 0)
            {
                return binaryHeap.RemoveMin();
            }
            else
            {
                throw new IndexOutOfRangeException("Queue is empty!");
            }
        }

        public T Peek()
        {
            return binaryHeap.Min;
        }

        public void Clear()
        {
            binaryHeap.Clear();
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
