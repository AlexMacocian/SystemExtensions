using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests;

[TestClass()]
public class BinaryHeapTests
{
    private BinaryHeap<int> binaryHeap = new BinaryHeap<int>();
    [TestMethod()]
    public void BinaryHeapTest()
    {
        this.binaryHeap = new BinaryHeap<int>();
    }

    [TestMethod()]
    public void BinaryHeapTest1()
    {
        this.binaryHeap = new BinaryHeap<int>(100);
        if (this.binaryHeap.Count != 0 && this.binaryHeap.Capacity != 100)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    [Timeout(500)]
    public void InsertTest()
    {
        try
        {
            for (var i = 0; i < 100; i++)
            {
                this.binaryHeap.Add(i * 20 - (50 + i));
            }
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message + "\n" + e.StackTrace);
        }

        if (this.binaryHeap.Count != 100)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    [Timeout(500)]
    public void DeleteMinTest()
    {
        try
        {
            var tries = this.binaryHeap.Count;
            while (this.binaryHeap.Count > 0)
            {
                if (tries == 0)
                {
                    Assert.Fail();
                }

                System.Diagnostics.Debug.WriteLine(this.binaryHeap.Remove());
                tries--;
            }
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message + "\n" + e.StackTrace);
        }
    }

    [TestMethod()]
    public void MinMaxTest()
    {
        for (var i = 0; i < 1000; i++)
        {
            this.binaryHeap.Add(i);
        }

        if (this.binaryHeap.Min != 0 || this.binaryHeap.Max != 999)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ClearTest()
    {
        for (var i = 0; i < 100; i++)
        {
            this.binaryHeap.Add(i);
        }

        this.binaryHeap.Clear();
        if (this.binaryHeap.Count > 0 || this.binaryHeap.Capacity != 160)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ClearTest2()
    {
        for (var i = 100; i < 200; i++)
        {
            this.binaryHeap.Add(i);
        }

        this.binaryHeap.Clear(false);
        var array = this.binaryHeap.ToArray();
        if (this.binaryHeap.Count > 0 || this.binaryHeap.Capacity == 10)
        {
            Assert.Fail();
        }

        if (array.Length != 0)
        {
            Assert.Fail();
        }

        this.binaryHeap.Clear(true);
        if (this.binaryHeap.Capacity != 10 || this.binaryHeap.Count != 0)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ContainsTest()
    {
        this.binaryHeap.Add(100);
        if (!this.binaryHeap.Contains(100))
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ToArrayTest()
    {
        for(var i = 0; i < 1000; i++)
        {
            this.binaryHeap.Add(i);
        }

        var array = this.binaryHeap.ToArray();
        if(array.Length != this.binaryHeap.Count)
        {
            Assert.Fail();
        }

        if(array[0] != 0 && array[999] != 999)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void GetEnumeratorTest()
    {
        for (var i = 0; i < 1000; i++)
        {
            this.binaryHeap.Add(i);
        }

        var count = 0;
        foreach(var value in this.binaryHeap)
        {
            System.Diagnostics.Debug.WriteLine(value);
            count++;
        }

        if(count == this.binaryHeap.Count)
        {
            return;
        }

        Assert.Fail();
    }
}