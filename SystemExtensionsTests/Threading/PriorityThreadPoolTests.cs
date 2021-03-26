using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using static System.Threading.PriorityThreadPool;

namespace System.Threading.Tests
{
    [TestClass()]
    public class PriorityThreadPoolTests
    {
        private static PriorityThreadPool threadPool = new PriorityThreadPool();
        private static int lowest = 0, belowAverage = 0, 
            normal = 0, aboveAverage = 0, highest = 0;
        [TestMethod()]
        public void PriorityThreadPoolTest()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool();
            if (threadPool.NumberOfThreads != System.Environment.ProcessorCount)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PriorityThreadPoolTest1()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool(4);
            if (threadPool.NumberOfThreads != 4)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void PriorityThreadPoolTest2()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool(1, 1, 1, 1, 1);
            if (threadPool.NumberOfThreads != 5)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void DisposeTest()
        {
            threadPool.Dispose();
            Thread.Sleep(100);
            try
            {
                int x = threadPool.NumberOfThreads;
                Assert.Fail();
            }
            catch(Exception e)
            {
                
            }
        }

        [TestMethod()]
        public void DefaultQueueTest()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool();
            for (int i = 0; i < 10; i++)
            {
                threadPool.QueueUserWorkItem(o =>
                {
                    for (int x = 0; x < 100; x++)
                    {
                        System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + x);
                    }
                }, null);
            }
            for (int i = 0; i < 10; i++)
            {
                System.Diagnostics.Debug.WriteLine("=====================================");
                System.Diagnostics.Debug.WriteLine("Current threads in threadpool: " + threadPool.NumberOfThreads);
                System.Diagnostics.Debug.WriteLine("=====================================");
                Thread.Sleep(100);
            }
            while (threadPool.NumberOfThreads != System.Environment.ProcessorCount / 4)
            {
                System.Diagnostics.Debug.WriteLine("=====================================");
                System.Diagnostics.Debug.WriteLine("Current threads in threadpool: " + threadPool.NumberOfThreads);
                System.Diagnostics.Debug.WriteLine("=====================================");
                Thread.Sleep(100);
            }
        }

        [TestMethod()]
        public void QueueTestWith4MaxThreads()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool(4);
            if (threadPool.NumberOfThreads != 4)
            {
                Assert.Fail();
            }
            for (int i = 0; i < 10; i++)
            {
                threadPool.QueueUserWorkItem(o =>
                {
                    for (int x = 0; x < 100; x++)
                    {
                        System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + x);
                    }
                }, null);
            }
            while (threadPool.NumberOfThreads != 1)
            {
                System.Diagnostics.Debug.WriteLine("=====================================");
                System.Diagnostics.Debug.WriteLine("Current threads in threadpool: " + threadPool.NumberOfThreads);
                System.Diagnostics.Debug.WriteLine("=====================================");
                Thread.Sleep(100);
            }
        }

        [TestMethod()]
        public void QueueTestWith5ManualThreads()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool(1, 1, 1, 1, 1);
            if (threadPool.NumberOfThreads != 5)
            {
                Assert.Fail();
            }
            for (int i = 0; i < 10; i++)
            {
                threadPool.QueueUserWorkItem(o =>
                {
                    for (int x = 0; x < 100; x++)
                    {
                        switch (Thread.CurrentThread.Priority)
                        {
                            case ThreadPriority.Lowest:
                                lowest++;
                                break;
                            case ThreadPriority.BelowNormal:
                                belowAverage++;
                                break;
                            case ThreadPriority.Normal:
                                normal++;
                                break;
                            case ThreadPriority.AboveNormal:
                                aboveAverage++;
                                break;
                            case ThreadPriority.Highest:
                                highest++;
                                break;
                        }
                    }
                }, null);
            }
            for (int i = 0; i < 50; i++)
            {
                System.Diagnostics.Debug.WriteLine("=====================================");
                System.Diagnostics.Debug.WriteLine("Current threads in threadpool: " + threadPool.NumberOfThreads);
                System.Diagnostics.Debug.WriteLine("=====================================");
                Thread.Sleep(100);
            }
            System.Diagnostics.Debug.WriteLine("\nL: " + lowest + "\nBN: " + belowAverage + "\nN: " + normal + "\nAN:" + aboveAverage + "\nH: " + highest);
        }

        [TestMethod()]
        public void QueueTestWithDifferentPriorityTasks()
        {
            threadPool.Dispose();
            threadPool = new PriorityThreadPool();
            for (int i = 0; i < 1000; i++)
            {
                TaskPriority taskPriority = TaskPriority.Lowest;
                if(i % 10 == 0)
                {
                    taskPriority = TaskPriority.Highest;
                }
                else if(i % 5 == 0)
                {
                    taskPriority = TaskPriority.Normal;
                }
                else if(i % 2 == 0)
                {
                    taskPriority = TaskPriority.BelowNormal;
                }
                threadPool.QueueUserWorkItem(o =>
                {
                    System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.ManagedThreadId + " - " + taskPriority);
                }, null, taskPriority);
            }
            while (threadPool.NumberOfThreads != System.Environment.ProcessorCount / 4)
            {
                System.Diagnostics.Debug.WriteLine("=====================================");
                System.Diagnostics.Debug.WriteLine("Current threads in threadpool: " + threadPool.NumberOfThreads);
                System.Diagnostics.Debug.WriteLine("=====================================");
                Thread.Sleep(100);
            }
        }
    }
}