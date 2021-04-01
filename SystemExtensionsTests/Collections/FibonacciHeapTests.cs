using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class FibonacciHeapTests
    {
        FibonacciHeap<int> fibonacciHeap = new FibonacciHeap<int>();
        [TestMethod()]
        public void InsertTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                fibonacciHeap.Add(i);
            }
        }

        [TestMethod()]
        public void FibonacciHeapTest()
        {
            fibonacciHeap = new FibonacciHeap<int>();
            if(fibonacciHeap.Count != 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void MergeTest()
        {
            FibonacciHeap<int> fibonacciHeap1 = new FibonacciHeap<int>();
            FibonacciHeap<int> fibonacciHeap2 = new FibonacciHeap<int>();

            for(int i = 0; i < 100; i++)
            {
                fibonacciHeap1.Add(i);
            }

            for(int i = 100; i < 300; i++)
            {
                fibonacciHeap2.Add(i);
            }

            fibonacciHeap1.Merge(fibonacciHeap2);
            for(int i = 1; i < 300; i++)
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
                fibonacciHeap.Remove();
                Assert.Fail();
            }
            catch(IndexOutOfRangeException exception)
            {

            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            for (int i = 0; i < 1000; i++)
            {
                fibonacciHeap.Add(i);
            }
            for (int i = 0; i < 1000; i++)
            {
                if (fibonacciHeap.Remove() != i)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void DecreaseKeyTest()
        {
            fibonacciHeap.Add(5);
            fibonacciHeap.Add(10);
            fibonacciHeap.Add(7);
            fibonacciHeap.DecreaseKey(5, 3);
            if(fibonacciHeap.Contains(5) || !fibonacciHeap.Contains(3))
            {
                Assert.Fail();
            }
            fibonacciHeap.DecreaseKey(10, 1);
            if(fibonacciHeap.Remove() != 1)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            for(int i = 0; i < 10000; i++)
            {
                fibonacciHeap.Add(i);
            }
            if (!fibonacciHeap.Contains(5124))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            for(int i = 0; i < 10000; i++)
            {
                fibonacciHeap.Add(i);
            }
            fibonacciHeap.Remove();
            fibonacciHeap.Clear();
            if(fibonacciHeap.Count > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            for(int i = 999; i >= 0; i--)
            {
                fibonacciHeap.Add(i);
            }
            fibonacciHeap.Remove();
            int[] arr = fibonacciHeap.ToArray();

            for(int i = 1; i < 512; i++)
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
            for (int i = 999; i >= 0; i--)
            {
                fibonacciHeap.Add(i);
            }

            int count = 0;

            foreach(int value in fibonacciHeap)
            {
                System.Diagnostics.Debug.WriteLine(value);
                count++;
            }

            if(count == fibonacciHeap.Count)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void Serialize()
        {
            FibonacciHeap<int> fibonacciHeap2 = new FibonacciHeap<int>();
            for (int i = 0; i < 100; i++)
            {
                fibonacciHeap.Add(i);
            }
            BinaryFormatter serializer = new BinaryFormatter();
            string s = string.Empty;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, fibonacciHeap);
                stream.Position = 0;
                fibonacciHeap2 = (FibonacciHeap<int>)serializer.Deserialize(stream);
            }
            IEnumerator<int> enum1 = fibonacciHeap.GetEnumerator();
            IEnumerator<int> enum2 = fibonacciHeap2.GetEnumerator();

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