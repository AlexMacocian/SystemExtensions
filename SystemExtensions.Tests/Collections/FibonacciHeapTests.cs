using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests;

[TestClass()]
public class FibonacciHeapTests
{
    FibonacciHeap<int> fibonacciHeap = new FibonacciHeap<int>();
    [TestMethod()]
    public void InsertTest()
    {
        for (var i = 0; i < 1000; i++)
        {
            this.fibonacciHeap.Add(i);
        }
    }

    [TestMethod()]
    public void FibonacciHeapTest()
    {
        this.fibonacciHeap = new FibonacciHeap<int>();
        if(this.fibonacciHeap.Count != 0)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void MergeTest()
    {
        var fibonacciHeap1 = new FibonacciHeap<int>();
        var fibonacciHeap2 = new FibonacciHeap<int>();

        for(var i = 0; i < 100; i++)
        {
            fibonacciHeap1.Add(i);
        }

        for(var i = 100; i < 300; i++)
        {
            fibonacciHeap2.Add(i);
        }

        fibonacciHeap1.Merge(fibonacciHeap2);
        for(var i = 1; i < 300; i++)
        {
            if (!fibonacciHeap1.Contains(i))
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod()]
    public void RemoveMinimumTest()
    {
        try
        {
            this.fibonacciHeap.Remove();
            Assert.Fail();
        }
        catch (IndexOutOfRangeException)
        {

        }
        catch (Exception)
        {
            Assert.Fail();
        }

        for (var i = 0; i < 1000; i++)
        {
            this.fibonacciHeap.Add(i);
        }

        for (var i = 0; i < 1000; i++)
        {
            if (this.fibonacciHeap.Remove() != i)
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod()]
    public void DecreaseKeyTest()
    {
        this.fibonacciHeap.Add(5);
        this.fibonacciHeap.Add(10);
        this.fibonacciHeap.Add(7);
        this.fibonacciHeap.DecreaseKey(5, 3);
        if(this.fibonacciHeap.Contains(5) || !this.fibonacciHeap.Contains(3))
        {
            Assert.Fail();
        }

        this.fibonacciHeap.DecreaseKey(10, 1);
        if(this.fibonacciHeap.Remove() != 1)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ContainsTest()
    {
        for(var i = 0; i < 10000; i++)
        {
            this.fibonacciHeap.Add(i);
        }

        if (!this.fibonacciHeap.Contains(5124))
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ClearTest()
    {
        for(var i = 0; i < 10000; i++)
        {
            this.fibonacciHeap.Add(i);
        }

        this.fibonacciHeap.Remove();
        this.fibonacciHeap.Clear();
        if(this.fibonacciHeap.Count > 0)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ToArrayTest()
    {
        for(var i = 999; i >= 0; i--)
        {
            this.fibonacciHeap.Add(i);
        }

        this.fibonacciHeap.Remove();
        var arr = this.fibonacciHeap.ToArray();

        for(var i = 1; i < 512; i++)
        {
            if(arr[i - 1] != i)
            {
                Assert.Fail();
            }
        }
    }
    
    [TestMethod()]
    public void GetEnumeratorTest()
    {
        for (var i = 999; i >= 0; i--)
        {
            this.fibonacciHeap.Add(i);
        }

        var count = 0;

        foreach(var value in this.fibonacciHeap)
        {
            System.Diagnostics.Debug.WriteLine(value);
            count++;
        }

        if(count == this.fibonacciHeap.Count)
        {
            return;
        }

        Assert.Fail();
    }
}