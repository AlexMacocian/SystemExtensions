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
    }
}