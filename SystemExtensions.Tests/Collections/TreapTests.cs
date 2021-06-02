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
            Random random = new Random();
            for(int i = 0; i < 1000; i++)
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
            Random random = new Random();
            for (int i = 0; i < 100; i++)
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
            for(int i = 0; i < 1000; i++)
            {
                treap.Add(i);
            }
            int[] arr = treap.ToArray();
            for(int i = 0; i < 1000; i++)
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
            for (int i = 0; i < 1000; i++)
            {
                treap.Add(i);
            }

            int count = 0;
            foreach(int value in treap)
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
            Treap<int> treap2 = new Treap<int>();
            for (int i = 0; i < 100; i++)
            {
                treap.Add(i);
            }
            BinaryFormatter serializer = new BinaryFormatter();
            string s = string.Empty;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, treap);
                stream.Position = 0;
                treap2 = (Treap<int>)serializer.Deserialize(stream);
            }
            IEnumerator<int> enum1 = treap.GetEnumerator();
            IEnumerator<int> enum2 = treap2.GetEnumerator();

            for (int i = 0; i < 100; i++)
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