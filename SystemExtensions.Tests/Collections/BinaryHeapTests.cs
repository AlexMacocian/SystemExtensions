﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class BinaryHeapTests
    {
        private BinaryHeap<int> binaryHeap = new BinaryHeap<int>();
        [TestMethod()]
        public void BinaryHeapTest()
        {
            binaryHeap = new BinaryHeap<int>();
        }

        [TestMethod()]
        public void BinaryHeapTest1()
        {
            binaryHeap = new BinaryHeap<int>(100);
            if (binaryHeap.Count != 0 && binaryHeap.Capacity != 100)
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
                for (int i = 0; i < 100; i++)
                {
                    binaryHeap.Add(i * 20 - (50 + i));
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.StackTrace);
            }
            if (binaryHeap.Count != 100)
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
                int tries = binaryHeap.Count;
                while (binaryHeap.Count > 0)
                {
                    if (tries == 0)
                    {
                        Assert.Fail();
                    }
                    System.Diagnostics.Debug.WriteLine(binaryHeap.Remove());
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
            for (int i = 0; i < 1000; i++)
            {
                binaryHeap.Add(i);
            }
            if (binaryHeap.Min != 0 || binaryHeap.Max != 999)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            for (int i = 0; i < 100; i++)
            {
                binaryHeap.Add(i);
            }
            binaryHeap.Clear();
            if (binaryHeap.Count > 0 || binaryHeap.Capacity != 160)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ClearTest2()
        {
            for (int i = 100; i < 200; i++)
            {
                binaryHeap.Add(i);
            }
            binaryHeap.Clear(false);
            int[] array = binaryHeap.ToArray();
            if (binaryHeap.Count > 0 || binaryHeap.Capacity == 10)
            {
                Assert.Fail();
            }
            if (array.Length != 0)
            {
                Assert.Fail();
            }
            binaryHeap.Clear(true);
            if (binaryHeap.Capacity != 10 || binaryHeap.Count != 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            binaryHeap.Add(100);
            if (!binaryHeap.Contains(100))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            for(int i = 0; i < 1000; i++)
            {
                binaryHeap.Add(i);
            }
            int[] array = binaryHeap.ToArray();
            if(array.Length != binaryHeap.Count)
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
            for (int i = 0; i < 1000; i++)
            {
                binaryHeap.Add(i);
            }
            int count = 0;
            foreach(int value in binaryHeap)
            {
                System.Diagnostics.Debug.WriteLine(value);
                count++;
            }

            if(count == binaryHeap.Count)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void Serialize()
        {
            BinaryHeap<int> binaryHeap2 = new BinaryHeap<int>();
            for (int i = 0; i < 100; i++)
            {
                binaryHeap.Add(i);
            }
            BinaryFormatter serializer = new BinaryFormatter();
            string s = string.Empty;
            using (var stream = new MemoryStream()) {
                serializer.Serialize(stream, binaryHeap);
                stream.Position = 0;
                binaryHeap2 = (BinaryHeap<int>)serializer.Deserialize(stream);
            }
            IEnumerator<int> binaryHeapEnum = binaryHeap.GetEnumerator();
            IEnumerator<int> binaryHeapEnum2 = binaryHeap2.GetEnumerator();

            for (int i = 0; i < 100; i++)
            {
                if (binaryHeapEnum.Current != binaryHeapEnum2.Current)
                {
                    Assert.Fail();
                }
                binaryHeapEnum.MoveNext();
                binaryHeapEnum2.MoveNext();
            }
        }
    }
}