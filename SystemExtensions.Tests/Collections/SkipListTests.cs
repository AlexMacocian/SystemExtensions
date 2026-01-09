using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests;

[TestClass()]
public class SkipListTests
{
    SkipList<int> skipList = new SkipList<int>();
    [TestMethod()]
    public void SkipListTest()
    {
        _ = new SkipList<int>();
    }

    [TestMethod()]
    public void SkipListTest2()
    {
        _ = new SkipList<int>(30);
    }

    [TestMethod()]
    public void AddTest()
    {
        for(var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }
    }

    [TestMethod()]
    public void ClearTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        this.skipList.Clear();
        if(this.skipList.Count > 0)
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void ContainsTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        if (!this.skipList.Contains(50))
        {
            Assert.Fail();
        }
    }

    [TestMethod()]
    public void CopyToTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        var array = new int[this.skipList.Count];
        this.skipList.CopyTo(array, 0);
        for (var i = 0; i < 200; i++)
        {
            if(array[i] != i)
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod()]
    public void ToArrayTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        var array = this.skipList.ToArray();

        for (var i = 0; i < 200; i++)
        {
            if(array[i] != i)
            {
                Assert.Fail();
            }
        }
    }

    [TestMethod()]
    public void GetEnumeratorTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        foreach(var i in this.skipList)
        {
            System.Diagnostics.Debug.WriteLine(i);
        }
    }

    [TestMethod()]
    public void RemoveTest()
    {
        for (var i = 0; i < 200; i++)
        {
            this.skipList.Add(i);
        }

        this.skipList.Remove(50);
        if (this.skipList.Contains(50))
        {
            Assert.Fail();
        }
    }
}