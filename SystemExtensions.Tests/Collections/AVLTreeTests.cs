using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace System.Collections.Tests
{
    [TestClass()]
    public class AVLTreeTests
    {
        private AVLTree<int> avlTree = new AVLTree<int>();
        [TestMethod()]
        public void AVLTreeTest()
        {
            avlTree = new AVLTree<int>();
        }

        [TestMethod()]
        public void AddTest()
        {
            try
            {
                for (var i = 0; i < 100; i++)
                {
                    avlTree.Add(i * 20 - (50 + i));
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.StackTrace);
            }
            if (avlTree.Count != 100)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ContainsTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }

            if (!avlTree.Contains(50))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }

            avlTree.Remove(50);

            if (avlTree.Contains(50))
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ClearTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }
            avlTree.Clear();
            if(avlTree.Count > 0)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void CopyToTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }

            var array = new int[100];
            avlTree.CopyTo(array, 0);
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }
            var count = 0;
            foreach(var value in avlTree)
            {
                count++;
                System.Diagnostics.Debug.WriteLine(value);
            }

            if(count != avlTree.Count)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ToArrayTest()
        {
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }

            _ = avlTree.ToArray();
        }

        [TestMethod()]
        public void Serialize()
        {
            var avlTree2 = new AVLTree<int>();
            for (var i = 0; i < 100; i++)
            {
                avlTree.Add(i);
            }
            var serializer = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, avlTree);
                stream.Position = 0;
                avlTree2 = (AVLTree<int>)serializer.Deserialize(stream);
            }
            var avlTreeEnum = avlTree.GetEnumerator();
            var avlTree2Enum = avlTree2.GetEnumerator();
            
            for(var i = 0; i < 100; i++)
            {
                if(avlTreeEnum.Current != avlTree2Enum.Current)
                {
                    Assert.Fail();
                }
                avlTreeEnum.MoveNext();
                avlTree2Enum.MoveNext();
            }
        }
    }
}