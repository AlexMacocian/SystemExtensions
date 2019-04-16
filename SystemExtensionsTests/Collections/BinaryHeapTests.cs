using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemExtensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections.Tests
{
    [TestClass()]
    public class BinaryHeapTests
    {
        private static int IntegerComparison(int x, int y)
        {
            if (x == y)
            {
                return 0;
            }
            else if (x > y)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        private BinaryHeap<int> binaryHeap = new BinaryHeap<int>(new Comparison<int>(IntegerComparison));
        [TestMethod()]
        public void BinaryHeapTest()
        {
            binaryHeap = new BinaryHeap<int>(new Comparison<int>(IntegerComparison));
        }

        [TestMethod()]
        public void BinaryHeapTest1()
        {
            binaryHeap = new BinaryHeap<int>(new Comparison<int>(IntegerComparison), 100);
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
                    binaryHeap.Insert(i * 20 - (50 + i));
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
                    System.Diagnostics.Debug.WriteLine(binaryHeap.DeleteMin());
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
                binaryHeap.Insert(i);
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
                binaryHeap.Insert(i);
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
                binaryHeap.Insert(i);
            }
            binaryHeap.Clear(false);
            int[] array = binaryHeap.ToArray();
            if (binaryHeap.Count > 0 || binaryHeap.Capacity != 10)
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
            binaryHeap.Insert(100);
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
                binaryHeap.Insert(i);
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
    }
}