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

    [TestMethod()]
    [Ignore("Binary serialization is obsolete and should not be used anymore")]
    public void Serialize()
    {
        var skipList2 = new SkipList<int>();
        for (var i = 0; i < 100; i++)
        {
            this.skipList.Add(i);
        }

        var serializer = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            serializer.Serialize(stream, this.skipList);
            stream.Position = 0;
            skipList2 = (SkipList<int>)serializer.Deserialize(stream);
        }

        var enum1 = this.skipList.GetEnumerator();
        var enum2 = skipList2.GetEnumerator();

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