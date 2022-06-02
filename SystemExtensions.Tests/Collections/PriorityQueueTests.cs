using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class PriorityQueueTests
    {
        private PriorityQueue<int> priorityQueue = new PriorityQueue<int>();
        [TestMethod()]
        public void PriorityQueueTest()
        {
            try
            {
                priorityQueue = new PriorityQueue<int>();
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
            for(var i = 0; i < 100; i++)
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
            for(var i = 0; i < 100; i++)
            {
                priorityQueue.Enqueue(i);
            }
            for(var i = 0; i < 100; i++)
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

        [TestMethod()]
        public void Serialize()
        {
            var priorityQueue2 = new PriorityQueue<int>();
            for (var i = 0; i < 100; i++)
            {
                priorityQueue.Enqueue(i);
            }
            var serializer = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, priorityQueue);
                stream.Position = 0;
                priorityQueue2 = (PriorityQueue<int>)serializer.Deserialize(stream);
            }
            var enum1 = priorityQueue.GetEnumerator();
            var enum2 = priorityQueue2.GetEnumerator();

            for (var i = 0; i < 100; i++)
            {
                if (enum1.Current != enum2.Current)
                {
                    Assert.Fail();
                }
                enum1.MoveNext();
                enum2.MoveNext();
            }
        }
    }
}