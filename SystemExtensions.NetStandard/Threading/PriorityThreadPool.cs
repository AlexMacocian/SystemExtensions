using System.Collections.Generic;

namespace System.Threading;

/// <summary>
/// Threadpool implementation that uses a priority queue as the queue for the tasks to execute.
/// Features both an automated algorithm to calibrate the number of active threads and a Constructor for constructing
/// a threadpool manually, without the auto-managed pool algorithm.
/// </summary>
public class PriorityThreadPool : IDisposable
{
    private class QueueEntry : IComparable<QueueEntry>
    {
        public TaskPriority TaskPriority { get; }
        public WaitCallback WaitCallback { get; }
        public object Object { get; }
        public QueueEntry(TaskPriority priority, WaitCallback callback, object obj)
        {
            this.TaskPriority = priority;
            this.WaitCallback = callback;
            this.Object = obj;
        }

        public int CompareTo(QueueEntry other)
        {
            int priority1 = 0, priority2 = 0;
            switch (this.TaskPriority)
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

            switch (other.TaskPriority)
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
            else if (priority1 > priority2)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

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
    private volatile PriorityQueue<QueueEntry> tasks;
    private Thread? observer;
    private CancellationTokenSource observerCancellationTokenSource = new CancellationTokenSource();
    private readonly object tasksLock = new object();
    private struct WorkerThread
    {
        public Thread Thread { get; set; }
        public bool Running { get; set; }
        public bool Working { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
    private struct Statistics
    {
        public bool Initialized { get; set; }
        public int PerformanceCounter { get; set; }
        public DateTime LastUpdate { get; set; }
        public double LoopFrequency { get; set; }
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
            return this.threadpool is null ? 0 : this.threadpool.Count;
        }
    }
    /// <summary>
    /// Returns true if there are no tasks queued.
    /// </summary>
    public bool Empty
    {
        get
        {
            return this.tasks is null ? true : this.tasks.Count == 0;
        }
    }
    /// <summary>
    /// Maximum amount of threads that the pool can utilize
    /// </summary>
    public int MaxThreads { set; get; }
    #endregion
    #region Constructors
    /// <summary>
    /// Constructor that initializes a threadpool using default values. All threads run at the same priority.
    /// The maximum number of threads is equal to System.Environment.ProcessorCount
    /// </summary>
    public PriorityThreadPool()
    {
        this.threadpool = new List<WorkerThread>();
        this.tasks = new PriorityQueue<QueueEntry>();
        this.MaxThreads = System.Environment.ProcessorCount;
        for (var i = 0; i < this.MaxThreads; i++)
        {
            this.CreateAndStartWorkerThread();
        }

        this.observer = new Thread(() => this.ObserverLoop())
        {
            Name = "ThreadPool ObserverThread",
            Priority = ThreadPriority.BelowNormal
        };
        this.observer.Start();
    }
    /// <summary>
    /// Constructor that initializes a threadpool with a number of threads, all of the same priority.
    /// </summary>
    /// <param name="maxThreads">Maximum number of threads</param>
    public PriorityThreadPool(int maxThreads)
    {
        this.threadpool = new List<WorkerThread>();
        this.tasks = new PriorityQueue<QueueEntry>();
        this.MaxThreads = Math.Max(maxThreads, 1);
        for (var i = 0; i < maxThreads; i++)
        {
            this.CreateAndStartWorkerThread();
        }

        this.observer = new Thread(() => this.ObserverLoop());
        this.observer.Name = "ThreadPool ObserverThread";
        this.observer.Priority = ThreadPriority.BelowNormal;
        this.observer.Start();
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
        this.threadpool = new List<WorkerThread>();
        this.tasks = new PriorityQueue<QueueEntry>();
        for (var i = 0; i < lowest; i++)
        {
            this.CreateAndStartWorkerThread(ThreadPriority.Lowest);
        }

        for (var i = 0; i < belowNormal; i++)
        {
            this.CreateAndStartWorkerThread(ThreadPriority.BelowNormal);
        }

        for (var i = 0; i < normal; i++)
        {
            this.CreateAndStartWorkerThread(ThreadPriority.Normal);
        }

        for (var i = 0; i < aboveNormal; i++)
        {
            this.CreateAndStartWorkerThread(ThreadPriority.AboveNormal);
        }

        for (var i = 0; i < highest; i++)
        {
            this.CreateAndStartWorkerThread(ThreadPriority.Highest);
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
        while (!Monitor.TryEnter(this.tasksLock))
        {
            ;
        }

        this.tasks.Enqueue(new QueueEntry(taskPriority, waitCallback, callbackState));
        Monitor.Exit(this.tasksLock);
    }
    /// <summary>
    /// Add a work item into the queue.
    /// </summary>
    /// <param name="waitCallback">WaitCallBack delegate that will be invoked by the threads.</param>
    /// <param name="callbackState">State used as parameter during invoke.</param>
    public void QueueUserWorkItem(WaitCallback waitCallback, object callbackState)
    {
        this.QueueUserWorkItem(waitCallback, callbackState, TaskPriority.Normal);
    }
    #endregion
    #region Private Methods
    private void CreateAndStartWorkerThread(ThreadPriority threadPriority)
    {
        var worker = new WorkerThread();
        worker.Thread = new Thread(() => this.ThreadMainLoop(ref worker));
        worker.Thread.Name = "ThreadPool WorkerThread";
        worker.Thread.Priority = threadPriority;
        worker.Running = true;
        worker.CancellationTokenSource = new CancellationTokenSource();
        worker.Thread.Start();
        this.threadpool.Add(worker);
    }
    private void CreateAndStartWorkerThread()
    {
        var worker = new WorkerThread();
        worker.Thread = new Thread(() => this.ThreadMainLoop(ref worker));
        worker.Thread.Name = "ThreadPool WorkerThread";
        worker.Running = true;
        worker.CancellationTokenSource = new CancellationTokenSource();
        worker.Thread.Start();
        this.threadpool.Add(worker);
    }
    /// <summary>
    /// Main loop that a thread from the pool is running.
    /// </summary>
    /// <threadId>Id of thread</threadId>
    private void ThreadMainLoop(ref WorkerThread thisWorkerThread)
    {
        thisWorkerThread.Running = true;
        while (thisWorkerThread.Running)
        {
            if (thisWorkerThread.CancellationTokenSource.Token.IsCancellationRequested)
            {
                thisWorkerThread.Running = false;
                return;
            }

            //Check if there are tasks
            if (this.tasks.Count > 0)
            {
                //If there are tasks, acquire a lock onto the list
                //and try to dequeue the highest priority task.
                //Finally, release the lock and invoke the task.

                thisWorkerThread.Working = true;
                QueueEntry task = default!;
                while (!Monitor.TryEnter(this.tasksLock))
                {
                    ;
                }

                if (this.tasks.Count > 0)
                {
                    task = this.tasks.Dequeue();
                }

                Monitor.Exit(this.tasksLock);
                if (task != null)
                {
                    System.Diagnostics.Debug.WriteLine(Thread.CurrentThread.Name + " - Running task!");
                    var waitCallback = task.WaitCallback;
                    waitCallback.Invoke(task.Object);
                }

                thisWorkerThread.Working = false;
            }
        }
    }
    /// <summary>
    /// Loop of the thread that adjusts and manipulates the threadpool
    /// </summary>
    private void ObserverLoop()
    {
        var statistics = new Statistics();
        while (true)
        {
            //Observer operates on a 100ms loop. Due to the low priority of the thread itself, this loop will almost always take
            //considerably longer than 100ms.
            //Checks if the queue is empty. If yes, counter is decremented, else, counter is incremented.
            //If counter exceeds 5, it will try to add another thread to the thread, unless the threadpool has reached max size.
            //If counter is under -10, it will try to remove a thread from the threadpool, unless the threadpool has reached less than 
            //max size / 4.

            if (this.observerCancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            //This part of code updates the statistics of the threadpool.
            if (statistics.Initialized)
            {
                var loopDuration = (DateTime.Now - statistics.LastUpdate).TotalMilliseconds;
                if (statistics.LoopFrequency == 0)
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
            if (this.tasks.Count > 0)
            {
                statistics.PerformanceCounter++;
                if (statistics.PerformanceCounter > 5)
                {
                    statistics.PerformanceCounter = 5;
                }
            }
            else
            {
                statistics.PerformanceCounter--;
                if (statistics.PerformanceCounter < -10)
                {
                    statistics.PerformanceCounter = -10;
                }
            }

            //This part of code adjusts thread priorities based on the current performance of the threadpool
            if (this.tasks.Count > 0)
            {
                //If there are tasks pending, find a thread with priority under Normal and upgrade its priority.
                var t = this.FindThreadWithLowPriority();
                if (t != null)
                {
                    this.UpgradeThreadPriority(t);
                }
            }
            else
            {
                //If there are no tasks pending, find a thread with priority above Lowest and downgrade its priority.
                var t = this.FindThreadWithAcceptablePriority();
                if (t != null)
                {
                    this.DowngradeThreadPriority(t);
                }
            }


            //This part of code modifies the number of active threads based on the current performance of the threadpool
            if (statistics.PerformanceCounter >= 5)
            {
                if (this.threadpool.Count < this.MaxThreads)
                {
                    //Add a thread to the threadpool.
                    //Reset counter to 0.
                    statistics.PerformanceCounter = 0;
                    this.CreateAndStartWorkerThread();
                }
            }
            else if (statistics.PerformanceCounter <= -10)
            {
                if (this.threadpool.Count > this.MaxThreads / 4)
                {
                    //Remove the last thread in the threadpool.
                    //If thread is currently working, notify it to close.
                    //Else, abort the thread.
                    //Reset counter to 0.
                    var worker = this.threadpool[this.threadpool.Count - 1];
                    if (worker.Working)
                    {
                        worker.Running = false;
                    }
                    else
                    {
                        worker.CancellationTokenSource.Cancel();
                    }

                    this.threadpool.RemoveAt(this.threadpool.Count - 1);
                    statistics.PerformanceCounter = 0;
                }
            }
        }
    }
    /// <summary>
    /// Find a thread with Lowest or BelowNormal priority.
    /// </summary>
    /// <returns>Thread with low priority.</returns>
    private Thread? FindThreadWithLowPriority()
    {
        foreach (var t in this.threadpool)
        {
            if (t.Thread.Priority == ThreadPriority.Lowest || t.Thread.Priority == ThreadPriority.BelowNormal)
            {
                return t.Thread;
            }
        }

        return default;
    }
    /// <summary>
    /// Find a thread with BelowNormal or Normal priority.
    /// </summary>
    /// <returns>Thread with BelowNormal or Normal priority.</returns>
    private Thread? FindThreadWithAcceptablePriority()
    {
        foreach (var t in this.threadpool)
        {
            if (t.Thread.Priority == ThreadPriority.Normal || t.Thread.Priority == ThreadPriority.BelowNormal)
            {
                return t.Thread;
            }
        }

        return default;
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
        if (!this.disposedValue)
        {
            if (disposing)
            {
                if (this.observer != null)
                {
                    this.observerCancellationTokenSource.Cancel();
                    this.observer.Join();
                }

                foreach (var worker in this.threadpool)
                {
                    worker.CancellationTokenSource.Cancel();
                    worker.Thread.Join();
                }

                this.threadpool.Clear();
                this.tasks.Clear();
            }

            this.threadpool = default!;
            this.tasks = default!;
            this.observer = default;
            this.disposedValue = true;
        }
    }
    /// <summary>
    /// Disposes of the tasks as well as aborts all threads.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
    }
    #endregion
}
