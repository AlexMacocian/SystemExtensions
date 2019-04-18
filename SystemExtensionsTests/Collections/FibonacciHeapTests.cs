using Microsoft.VisualStudio.TestTools.UnitTesting;
using SystemExtensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SystemExtensions.Collections.Tests
{
    [TestClass()]
    public class FibonacciHeapTests
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
        FibonacciHeap<int> fibonacciHeap = new FibonacciHeap<int>(new Comparison<int>(IntegerComparison));
        [TestMethod()]
        public void InsertTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                fibonacciHeap.Insert(i);
            }
            if (fibonacciHeap.RemoveMinimum() != 0)
            {
                Assert.Fail();
            }
            for (int i = 1; i < 1000; i++)
            {
                if (!fibonacciHeap.Contains(i))
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void FibonacciHeapTest()
        {
            fibonacciHeap.Dispose();
            fibonacciHeap = new FibonacciHeap<int>(new Comparison<int>(IntegerComparison));
            if(fibonacciHeap.Count != 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void MergeTest()
        {
            fibonacciHeap.Dispose();
            FibonacciHeap<int> fibonacciHeap1 = new FibonacciHeap<int>(new Comparison<int>(IntegerComparison));
            FibonacciHeap<int> fibonacciHeap2 = new FibonacciHeap<int>(new Comparison<int>(IntegerComparison));

            for(int i = 0; i < 100; i++)
            {
                fibonacciHeap1.Insert(i);
            }

            for(int i = 100; i < 300; i++)
            {
                fibonacciHeap2.Insert(i);
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
                fibonacciHeap.RemoveMinimum();
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
                fibonacciHeap.Insert(i);
            }
            for (int i = 0; i < 1000; i++)
            {
                if (fibonacciHeap.RemoveMinimum() != i)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod()]
        public void DecreaseKeyTest()
        {
            fibonacciHeap.Insert(5);
            fibonacciHeap.Insert(10);
            fibonacciHeap.Insert(7);
            fibonacciHeap.DecreaseKey(5, 3);
            if(fibonacciHeap.Contains(5) || !fibonacciHeap.Contains(3))
            {
                Assert.Fail();
            }
            fibonacciHeap.DecreaseKey(10, 1);
            if(fibonacciHeap.RemoveMinimum() != 1)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            for(int i = 0; i < 10000; i++)
            {
                fibonacciHeap.Insert(i);
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
                fibonacciHeap.Insert(i);
            }
            fibonacciHeap.RemoveMinimum();
            fibonacciHeap.Clear();
            if(fibonacciHeap.Count > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void DisposeTest()
        {
            List<FibonacciHeap<int>> heaps = new List<Collections.FibonacciHeap<int>>();
            for(int i = 0; i < 10; i++)
            {
                heaps.Add(new Collections.FibonacciHeap<int>(new Comparison<int>(IntegerComparison)));
                for(int j = 0; j < 10000; j++)
                {
                    heaps[heaps.Count - 1].Insert(j);
                }
            }
            for(int i = 0; i < 10; i++)
            {
                heaps[i].Dispose();
            }
            GC.Collect();
            heaps.Clear();
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            for(int i = 999; i >= 0; i--)
            {
                fibonacciHeap.Insert(i);
            }
            fibonacciHeap.RemoveMinimum();
            int[] arr = fibonacciHeap.ToArray();

            for(int i = 1; i < 512; i++)
            {
                if(arr[i - 1] != i)
                {
                    Assert.Fail();
                }
            }
        }
    }
}