using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemExtensions.Collections;

namespace SystemExtensions.Threading
{
    public class PriorityThreadPool : IDisposable
    {
        #region Enum
        /// <summary>
        /// Enum for priority of task scheduled
        /// </summary>
        public enum TaskPriority
        {
            Highest,
            AboveNormal,
            Normal,
            BelowNormal,
            Lowest
        }
        #endregion
        #region Fields
        /// <summary>
        /// List that contains current running threads and their status.
        /// </summary>
        private List<WorkerThread> threadpool;
        private PriorityQueue<Tuple<TaskPriority, WaitCallback, object>> tasks;
        private Thread observer;
        private int maxThreads;
        private object tasksLock = new object();
        private struct WorkerThread
        {
            public Thread thread;
            public bool running, working;
        }
        #endregion

        #region Properties
        public int NumberOfThreads
        {
            get
            {
                return threadpool.Count;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that initializes a threadpool using default values. All threads run at the same priority.
        /// The maximum number of threads is equal to System.Environment.ProcessorCount
        /// </summary>
        public PriorityThreadPool()
        {
            threadpool = new List<WorkerThread>();
            tasks = new PriorityQueue<Tuple<TaskPriority, WaitCallback, object>>(PriorityCompare);
            int nrThreads = (int)Math.Ceiling((double)System.Environment.ProcessorCount / 4);
            maxThreads = System.Environment.ProcessorCount;
            for (int i = 0; i < nrThreads; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Start();
                threadpool.Add(worker);
            }
            observer = new Thread(() =>
            {
                ObserverLoop();
            });
            observer.Priority = ThreadPriority.Normal;
            observer.Start();
        }
        /// <summary>
        /// Constructor that initializes a threadpool with a number of threads, all of the same priority.
        /// </summary>
        /// <param name="maxThreads">Maximum number of threads</param>
        public PriorityThreadPool(int maxThreads)
        {
            threadpool = new List<WorkerThread>();
            tasks = new PriorityQueue<Tuple<TaskPriority, WaitCallback, object>>(PriorityCompare);
            this.maxThreads = Math.Max(maxThreads, 1);
            int nrThreads = (int)Math.Ceiling((double)this.maxThreads / 4);
            for (int i = 0; i < nrThreads; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Start();
                threadpool.Add(worker);
            }
            observer = new Thread(() =>
            {
                ObserverLoop();
            });
            observer.Priority = ThreadPriority.Normal;
            observer.Start();
        }
        /// <summary>
        /// Constructor for a manually-configured threadpool that runs a specified number of threads.
        /// This threadpool with not be automatically managed and will always run the specified amount of threads
        /// </summary>
        /// <param name="lowest">Number of threads with Lowest priority.</param>
        /// <param name="belowNormal">Number of threads with BelowNormal priority.</param>
        /// <param name="normal">Number of threads with Normal priority.</param>
        /// <param name="aboveNormal">Number of threads with AboveNormal priority.</param>
        /// <param name="highest">Number of threads with Highest priority.</param>
        public PriorityThreadPool(int lowest, int belowNormal, int normal, int aboveNormal, int highest)
        {
            threadpool = new List<WorkerThread>();
            tasks = new PriorityQueue<Tuple<TaskPriority, WaitCallback, object>>(PriorityCompare);
            for(int i = 0; i < lowest; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Lowest;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < belowNormal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.BelowNormal;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < normal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Normal;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < aboveNormal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.AboveNormal;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < highest; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Highest;
                worker.thread.Start();
                threadpool.Add(worker);
            }
        }
        #endregion

        #region Public Methods
        public void QueueUserWorkItem(WaitCallback waitCallback, object callbackState, TaskPriority taskPriority)
        {
            while (!Monitor.TryEnter(tasksLock));
            tasks.Enqueue(new Tuple<TaskPriority, WaitCallback, object>(taskPriority, waitCallback, callbackState));
            Monitor.Exit(tasksLock);
        }

        public void QueueUserWorkItem(WaitCallback waitCallback, object callbackState)
        {
            QueueUserWorkItem(waitCallback, callbackState, TaskPriority.Normal);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Function that compares two tasks based on their priorities
        /// </summary>
        /// <param name="tp1">First tuple to compare</param>
        /// <param name="tp2">Second tuple to compare</param>
        /// <returns>1 if tp1 is bigger. 0 if equal and -1 if tp1 is lower.</returns>
        private static int PriorityCompare(Tuple<TaskPriority, WaitCallback, object> tp1, Tuple<TaskPriority, WaitCallback, object> tp2)
        {
            int priority1 = 0, priority2 = 0;
            switch (tp1.Item1)
            {
                case TaskPriority.Highest:
                    priority1 = 4;
                    break;
                case TaskPriority.AboveNormal:
                    priority1 = 3;
                    break;
                case TaskPriority.Normal:
                    priority1 = 2;
                    break;
                case TaskPriority.BelowNormal:
                    priority1 = 1;
                    break;
                case TaskPriority.Lowest:
                    priority1 = 0;
                    break;
            }
            switch (tp2.Item1)
            {
                case TaskPriority.Highest:
                    priority2 = 4;
                    break;
                case TaskPriority.AboveNormal:
                    priority2 = 3;
                    break;
                case TaskPriority.Normal:
                    priority2 = 2;
                    break;
                case TaskPriority.BelowNormal:
                    priority2 = 1;
                    break;
                case TaskPriority.Lowest:
                    priority2 = 0;
                    break;
            }
            if (priority1 == priority2)
            {
                return 0;
            }
            else if(priority1 > priority2)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// Main loop that a thread from the pool is running.
        /// </summary>
        /// <threadId>Id of thread</threadId>
        private void ThreadMainLoop(WorkerThread thisWorkerThread)
        {
            thisWorkerThread.running = true;
            while (thisWorkerThread.running)
            {
                //Check if there are tasks

                if (tasks.Count > 0)
                {
                    //If there are tasks, acquire a lock onto the list
                    //and try to dequeue the highest priority task.
                    //Finally, release the lock and invoke the task.

                    thisWorkerThread.working = true;
                    Tuple<TaskPriority, WaitCallback, object> task = null;
                    while (!Monitor.TryEnter(tasksLock)) ;
                    if (tasks.Count > 0)
                    {
                        task = tasks.Dequeue();
                    }
                    Monitor.Exit(tasksLock);
                    if (task != null)
                    {
                        WaitCallback waitCallback = task.Item2;
                        waitCallback.Invoke(task.Item3);
                    }
                    thisWorkerThread.working = false;
                }
            }
        }
        /// <summary>
        /// Loop of the thread that adjusts and manipulates the threadpool
        /// </summary>
        private void ObserverLoop()
        {
            int counter = 0;
            while (true)
            {
                //Observer operates on a 100ms loop. 
                //Checks if the queue is empty. If yes, counter is decremented, else, counter is incremented.
                //If counter exceeds 5, it will try to add another thread to the thread, unless the threadpool has reached max size.
                //If counter is under -10, it will try to remove a thread from the threadpool, unless the threadpool has reached less than 
                //max size / 4.
                Thread.Sleep(100);
                if(tasks.Count > 0)
                {
                    counter++;
                    if(counter > 5)
                    {
                        counter = 5;
                    }
                }
                else
                {
                    counter--;
                    if(counter < -10)
                    {
                        counter = -10;
                    }
                }
                if(counter >= 5 && threadpool.Count < this.maxThreads)
                {
                    //Add a thread to the threadpool.
                    //Reset counter to 0.
                    counter = 0;
                    WorkerThread worker = new WorkerThread();
                    worker.thread = new Thread(() =>
                    {
                        ThreadMainLoop(worker);
                    });
                    worker.thread.Start();
                    threadpool.Add(worker);
                }
                if(counter <= -10 && threadpool.Count > maxThreads / 4)
                {
                    //Remove the last thread in the threadpool.
                    //If thread is currently working, notify it to close.
                    //Else, abort the thread.
                    //Reset counter to 0.
                    WorkerThread worker = threadpool[threadpool.Count - 1];
                    threadpool.RemoveAt(threadpool.Count - 1);
                    if (worker.working)
                    {
                        worker.running = false;
                    }
                    else
                    {
                        worker.thread.Abort();
                    }
                    counter = 0;
                }
            }
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach(WorkerThread worker in threadpool)
                    {
                        worker.thread.Abort();
                    }
                    threadpool.Clear();
                    tasks.Clear();
                    if (observer != null)
                    {
                        observer.Abort();
                    }
                }
                threadpool = null;
                tasks = null;
                observer = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
