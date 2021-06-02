using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class SkipListTests
    {
        SkipList<int> skipList = new SkipList<int>();
        [TestMethod()]
        public void SkipListTest()
        {
            SkipList<int> skipList = new SkipList<int>();
        }

        [TestMethod()]
        public void SkipListTest2()
        {
            SkipList<int> skipList = new SkipList<int>(30);
        }

        [TestMethod()]
        public void AddTest()
        {
            for(int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }
            skipList.Clear();
            if(skipList.Count > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }

            if (!skipList.Contains(50))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void CopyToTest()
        {
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }
            int[] array = new int[skipList.Count];
            skipList.CopyTo(array, 0);
            for (int i = 0; i < 200; i++)
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
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }
            int[] array = skipList.ToArray();

            for (int i = 0; i < 200; i++)
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
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }

            foreach(int i in skipList)
            {
                System.Diagnostics.Debug.WriteLine(i);
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            for (int i = 0; i < 200; i++)
            {
                skipList.Add(i);
            }
            skipList.Remove(50);
            if (skipList.Contains(50))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        [Ignore("Binary serialization is obsolete and should not be used anymore")]
        public void Serialize()
        {
            SkipList<int> skipList2 = new SkipList<int>();
            for (int i = 0; i < 100; i++)
            {
                skipList.Add(i);
            }
            BinaryFormatter serializer = new BinaryFormatter();
            string s = string.Empty;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, skipList);
                stream.Position = 0;
                skipList2 = (SkipList<int>)serializer.Deserialize(stream);
            }
            IEnumerator<int> enum1 = skipList.GetEnumerator();
            IEnumerator<int> enum2 = skipList2.GetEnumerator();

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