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
    public class TreapTests
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
        Treap<int> treap = new Treap<int>(new Comparison<int>(IntegerComparison));
        [TestMethod()]
        public void TreapTest()
        {
            treap = new Treap<int>(new Comparison<int>(IntegerComparison));
        }

        [TestMethod()]
        public void InsertTest()
        {
            Random random = new Random();
            for(int i = 0; i < 1000; i++)
            {
                treap.Insert(random.Next(0, 5000));
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            treap.Insert(60);
            treap.Insert(6);
            treap.Insert(5);
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
                treap.Insert(random.Next(0, 5000));
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
            treap.Insert(50);
            treap.Insert(25);
            treap.Insert(991142);
            treap.Insert(12313);
            treap.Insert(24);
            treap.Insert(23);
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
                treap.Insert(i);
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
    }
}