using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests;

[TestClass()]
public class PriorityQueueTests
{
    private PriorityQueue<int> priorityQueue = new PriorityQueue<int>();
    [TestMethod()]
    public void PriorityQueueTest()
    {
        try
        {
            this.priorityQueue = new PriorityQueue<int>();
            if(this.priorityQueue.Count > 0)
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
            this.priorityQueue.Enqueue(i);
        }

        if(this.priorityQueue.Count != 100)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void DequeueTest()
    {
        for(var i = 0; i < 100; i++)
        {
            this.priorityQueue.Enqueue(i);
        }

        for(var i = 0; i < 100; i++)
        {
            if(i != this.priorityQueue.Dequeue())
            {
                Assert.Fail();
            }
        }

        if(this.priorityQueue.Count > 0)
        {
            Assert.Fail();
        }

        try
        {
            this.priorityQueue.Dequeue();
            Assert.Fail();
        }
        catch
        {

        }
    }

    [TestMethod()]
    public void PeekTest()
    {
        this.priorityQueue.Enqueue(1051);
        if(this.priorityQueue.Peek() != 1051 && this.priorityQueue.Count != 1)
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
            this.priorityQueue.Enqueue(i);
        }

        var serializer = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, this.priorityQueue);
            stream.Position = 0;
            priorityQueue2 = (PriorityQueue<int>)serializer.Deserialize(stream);
        }

        var enum1 = this.priorityQueue.GetEnumerator();
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