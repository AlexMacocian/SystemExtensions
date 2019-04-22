using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemExtensions.Collections;

namespace SystemExtensions.Threading
{
    /// <summary>
    /// Threadpool implementation that uses a priority queue as the queue for the tasks to execute.
    /// Features both an automated algorithm to calibrate the number of active threads and a Constructor for constructing
    /// a threadpool manually, without the auto-managed pool algorithm.
    /// </summary>
    public class PriorityThreadPool : IDisposable
    {
        #region Enum
        /// <summary>
        /// Enum for priority of task scheduled
        /// </summary>
        public enum TaskPriority
        {
            /// <summary>
            /// Highest priority of a task.
            /// </summary>
            Highest,
            /// <summary>
            /// Priority level above the default level.
            /// </summary>
            AboveNormal,
            /// <summary>
            /// Default priority level of all tasks without a specified priority level.
            /// </summary>
            Normal,
            /// <summary>
            /// Priority level below the default level.
            /// </summary>
            BelowNormal,
            /// <summary>
            /// Lowest priority level.
            /// </summary>
            Lowest
        }
        #endregion
        #region Fields
        /// <summary>
        /// List that contains current running threads and their status.
        /// </summary>
        private volatile List<WorkerThread> threadpool;
        private volatile PriorityQueue<Tuple<TaskPriority, WaitCallback, object>> tasks;
        private Thread observer;
        private int maxThreads;
        private object tasksLock = new object();
        private struct WorkerThread
        {
            public Thread thread;
            public bool running, working;
        }
        private struct Statistics
        {
            public bool Initialized;
            public int PerformanceCounter;
            public DateTime LastUpdate;
            public double LoopFrequency;
        }
        #endregion
        #region Properties
        /// <summary>
        /// Returns the number of active threads in the threadpool.
        /// </summary>
        public int NumberOfThreads
        {
            get
            {
                return threadpool.Count;
            }
        }
        /// <summary>
        /// Returns true if there are no tasks queued.
        /// </summary>
        public bool Empty
        {
            get
            {
                return tasks.Count == 0;
            }
        }
        /// <summary>
        /// Maximum amount of threads that the pool can utilize
        /// </summary>
        public int MaxThreads
        {
            set
            {
                maxThreads = value;
            }
            get
            {
                return maxThreads;
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
            maxThreads = System.Environment.ProcessorCount;
            for (int i = 0; i < maxThreads; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            observer = new Thread(() =>
            {
                ObserverLoop();
            });
            observer.Name = "ThreadPool ObserverThread";
            observer.Priority = ThreadPriority.BelowNormal;
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
            for (int i = 0; i < maxThreads; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            observer = new Thread(() =>
            {
                ObserverLoop();
            });
            observer.Name = "ThreadPool ObserverThread";
            observer.Priority = ThreadPriority.BelowNormal;
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
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Lowest;
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < belowNormal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.BelowNormal;
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < normal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Normal;
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < aboveNormal; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.AboveNormal;
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
            for (int i = 0; i < highest; i++)
            {
                WorkerThread worker = new WorkerThread();
                worker.thread = new Thread(() =>
                {
                    ThreadMainLoop(ref worker);
                });
                worker.thread.Name = "ThreadPool WorkerThread";
                worker.thread.Priority = ThreadPriority.Highest;
                worker.running = true;
                worker.thread.Start();
                threadpool.Add(worker);
            }
        }        
        #endregion
        #region Public Methods
        /// <summary>
        /// Add a work item into the queue.
        /// </summary>
        /// <param name="waitCallback">WaitCallBack delegate that will be invoked by the threads.</param>
        /// <param name="callbackState">State used as parameter during invoke.</param>
        /// <param name="taskPriority">Priority of task. Affects its position into the queue.</param>
        public void QueueUserWorkItem(WaitCallback waitCallback, object callbackState, TaskPriority taskPriority)
        {
            while (!Monitor.TryEnter(tasksLock));
            tasks.Enqueue(new Tuple<TaskPriority, WaitCallback, object>(taskPriority, waitCallback, callbackState));
            Monitor.Exit(tasksLock);
        }
        /// <summary>
        /// Add a work item into the queue.
        /// </summary>
        /// <param name="waitCallback">WaitCallBack delegate that will be invoked by the threads.</param>
        /// <param name="callbackState">State used as parameter during invoke.</param>
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
        private void ThreadMainLoop(ref WorkerThread thisWorkerThread)
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
                        System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.Name + " - Running task!");
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
            Statistics statistics = new Statistics();
            while (true)
            {
                //Observer operates on a 100ms loop. Due to the low priority of the thread itself, this loop will almost always take
                //considerably longer than 100ms.
                //Checks if the queue is empty. If yes, counter is decremented, else, counter is incremented.
                //If counter exceeds 5, it will try to add another thread to the thread, unless the threadpool has reached max size.
                //If counter is under -10, it will try to remove a thread from the threadpool, unless the threadpool has reached less than 
                //max size / 4.

                //This part of code updates the statistics of the threadpool.
                if (statistics.Initialized)
                {
                    double loopDuration = (DateTime.Now - statistics.LastUpdate).TotalMilliseconds;
                    if(statistics.LoopFrequency == 0)
                    {
                        statistics.LoopFrequency = loopDuration;
                    }
                    else
                    {
                        statistics.LoopFrequency = (statistics.LoopFrequency + loopDuration) / 2;
                    }
                }
                else
                {
                    statistics.Initialized = true;
                }
                statistics.LastUpdate = DateTime.Now;

                Thread.Sleep(100);


                //This part of code adjusts the performance counter. Ideally, the value should be 0.
                if(tasks.Count > 0)
                {
                    statistics.PerformanceCounter++;
                    if(statistics.PerformanceCounter > 5)
                    {
                        statistics.PerformanceCounter = 5;
                    }
                }
                else
                {
                    statistics.PerformanceCounter--;
                    if(statistics.PerformanceCounter < -10)
                    {
                        statistics.PerformanceCounter = -10;
                    }
                }

                //This part of code adjusts thread priorities based on the current performance of the threadpool
                if(tasks.Count > 0)
                {
                    //If there are tasks pending, find a thread with priority under Normal and upgrade its priority.
                    Thread t = FindThreadWithLowPriority();
                    if(t != null)
                    {
                        UpgradeThreadPriority(t);
                    }
                }
                else
                {
                    //If there are no tasks pending, find a thread with priority above Lowest and downgrade its priority.
                    Thread t = FindThreadWithAcceptablePriority();
                    if(t != null)
                    {
                        DowngradeThreadPriority(t);
                    }
                }


                //This part of code modifies the number of active threads based on the current performance of the threadpool
                if (statistics.PerformanceCounter >= 5)
                {
                    if (threadpool.Count < this.maxThreads)
                    {
                        //Add a thread to the threadpool.
                        //Reset counter to 0.
                        statistics.PerformanceCounter = 0;
                        WorkerThread worker = new WorkerThread();
                        worker.thread = new Thread(() =>
                        {
                            ThreadMainLoop(ref worker);
                        });
                        worker.thread.Name = "ThreadPool WorkerThread";
                        worker.running = true;
                        worker.thread.Start();
                        threadpool.Add(worker);
                    }
                }
                else if(statistics.PerformanceCounter <= -10)
                {
                    if (threadpool.Count > maxThreads / 4)
                    {
                        //Remove the last thread in the threadpool.
                        //If thread is currently working, notify it to close.
                        //Else, abort the thread.
                        //Reset counter to 0.
                        WorkerThread worker = threadpool[threadpool.Count - 1];
                        if (worker.working)
                        {
                            worker.running = false;
                        }
                        else
                        {
                            worker.thread.Abort();
                        }
                        threadpool.RemoveAt(threadpool.Count - 1);
                        statistics.PerformanceCounter = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Find a thread with Lowest or BelowNormal priority.
        /// </summary>
        /// <returns>Thread with low priority.</returns>
        private Thread FindThreadWithLowPriority()
        {
            foreach(WorkerThread t in threadpool)
            {
                if(t.thread.Priority == ThreadPriority.Lowest || t.thread.Priority == ThreadPriority.BelowNormal)
                {
                    return t.thread;
                }
            }
            return null;
        }
        /// <summary>
        /// Find a thread with BelowNormal or Normal priority.
        /// </summary>
        /// <returns>Thread with BelowNormal or Normal priority.</returns>
        private Thread FindThreadWithAcceptablePriority()
        {
            foreach (WorkerThread t in threadpool)
            {
                if (t.thread.Priority == ThreadPriority.Normal || t.thread.Priority == ThreadPriority.BelowNormal)
                {
                    return t.thread;
                }
            }
            return null;
        }
        /// <summary>
        /// Downgrades the priority of a thread one level.
        /// </summary>
        /// <param name="t">Thread to have its priority level downgraded.</param>
        /// <returns></returns>
        private void DowngradeThreadPriority(Thread t)
        {
            switch (t.Priority)
            {
                case ThreadPriority.Highest:
                    t.Priority = ThreadPriority.AboveNormal;
                    break;
                case ThreadPriority.AboveNormal:
                    t.Priority = ThreadPriority.Normal;
                    break;
                case ThreadPriority.Normal:
                    t.Priority = ThreadPriority.BelowNormal;
                    break;
                case ThreadPriority.BelowNormal:
                    t.Priority = ThreadPriority.Lowest;
                    break;
                case ThreadPriority.Lowest:
                    t.Priority = ThreadPriority.Lowest;
                    break;
            }
        }
        /// <summary>
        /// Upgrades the priority of a thread one level.
        /// </summary>
        /// <param name="t">Thread to have its priority level upgraded.</param>
        /// <returns></returns>
        private void UpgradeThreadPriority(Thread t)
        {
            switch (t.Priority)
            {
                case ThreadPriority.Highest:
                    t.Priority = ThreadPriority.Highest;
                    break;
                case ThreadPriority.AboveNormal:
                    t.Priority = ThreadPriority.Highest;
                    break;
                case ThreadPriority.Normal:
                    t.Priority = ThreadPriority.AboveNormal;
                    break;
                case ThreadPriority.BelowNormal:
                    t.Priority = ThreadPriority.Normal;
                    break;
                case ThreadPriority.Lowest:
                    t.Priority = ThreadPriority.BelowNormal;
                    break;
            }
        }
        #endregion
        #region IDisposable Support
        private bool disposedValue = false;
        /// <summary>
        /// Disposes of the tasks as well as aborts all threads. Called by the public Dispose() method.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (observer != null)
                    {
                        observer.Abort();
                    }
                    foreach (WorkerThread worker in threadpool)
                    {
                        worker.thread.Abort();
                    }
                    threadpool.Clear();
                    tasks.Clear();
                }
                threadpool = null;
                tasks = null;
                observer = null;
                disposedValue = true;
            }
        }
        /// <summary>
        /// Disposes of the tasks as well as aborts all threads.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
