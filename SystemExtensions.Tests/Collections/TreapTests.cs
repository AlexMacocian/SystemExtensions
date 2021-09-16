using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class TreapTests
    {
        Treap<int> treap = new Treap<int>();
        [TestMethod()]
        public void TreapTest()
        {
            treap = new Treap<int>();
        }

        [TestMethod()]
        public void InsertTest()
        {
            var random = new Random();
            for(var i = 0; i < 1000; i++)
            {
                treap.Add(random.Next(0, 5000));
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            treap.Add(60);
            treap.Add(6);
            treap.Add(5);
            treap.Remove(60);
            if(treap.Contains(60) || treap.Count > 2)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                treap.Add(random.Next(0, 5000));
            }
            treap.Clear();
            if(treap.Count > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            treap.Add(50);
            treap.Add(25);
            treap.Add(991142);
            treap.Add(12313);
            treap.Add(24);
            treap.Add(23);
            if (!treap.Contains(24))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            for(var i = 0; i < 1000; i++)
            {
                treap.Add(i);
            }
            var arr = treap.ToArray();
            for(var i = 0; i < 1000; i++)
            {
                if(arr[i] != i)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                treap.Add(i);
            }

            var count = 0;
            foreach(var value in treap)
            {
                System.Diagnostics.Debug.WriteLine(value);
                count++;
            }

            if(count == treap.Count)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        [Ignore("Binary serialization is obsolete and should not be used anymore")]
        public void Serialize()
        {
            var treap2 = new Treap<int>();
            for (var i = 0; i < 100; i++)
            {
                treap.Add(i);
            }
            var serializer = new BinaryFormatter();
            var s = string.Empty;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, treap);
                stream.Position = 0;
                treap2 = (Treap<int>)serializer.Deserialize(stream);
            }
            var enum1 = treap.GetEnumerator();
            var enum2 = treap2.GetEnumerator();

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