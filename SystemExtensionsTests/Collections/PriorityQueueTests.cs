using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemExtensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemExtensions;

namespace SystemExtensions.Collections.Tests
{
    [TestClass()]
    public class PriorityQueueTests
    {
        private static int IntegerComparison(int x, int y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        private PriorityQueue<int> priorityQueue = new PriorityQueue<int>(new Comparison<int>(IntegerComparison));
        [TestMethod()]
        public void PriorityQueueTest()
        {
            try
            {
                priorityQueue = new PriorityQueue<int>(new Comparison<int>(IntegerComparison));
                if(priorityQueue.Count > 0)
                {
                    Assert.Fail();
                }
            }
            catch(Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.StackTrace);
            }
        }

        [TestMethod()]
        public void EnqueueTest()
        {
            for(int i = 0; i < 100; i++)
            {
                priorityQueue.Enqueue(i);
            }
            if(priorityQueue.Count != 100)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void DequeueTest()
        {
            for(int i = 0; i < 100; i++)
            {
                priorityQueue.Enqueue(i);
            }
            for(int i = 0; i < 100; i++)
            {
                if(i != priorityQueue.Dequeue())
                {
                    Assert.Fail();
                }
            }
            if(priorityQueue.Count > 0)
            {
                Assert.Fail();
            }
            try
            {
                priorityQueue.Dequeue();
                Assert.Fail();
            }
            catch
            {

            }
        }

        [TestMethod()]
        public void PeekTest()
        {
            priorityQueue.Enqueue(1051);
            if(priorityQueue.Peek() != 1051 && priorityQueue.Count != 1)
            {
                Assert.Fail();
            }
        }
    }
}