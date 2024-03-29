﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Extensions;

public static class TaskExtensions
{
    /// <summary>
    /// Mark task as <see cref="TaskCreationOptions.LongRunning"/>.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="task">Task.</param>
    /// <returns>The started <see cref="Task{TResult}"/></returns>
    public static Task<T> LongRunning<T>(this Task<T> task)
    {
        return Task.Factory.StartNew(async () =>
        {
            return await task;
        }, TaskCreationOptions.LongRunning).Unwrap();
    }

    /// <summary>
    /// Mark task as <see cref="TaskCreationOptions.LongRunning"/>.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <returns>The started <see cref="Task"/></returns>
    public static Task LongRunning(this Task task)
    {
        return Task.Factory.StartNew(async () =>
        {
            await task;
        }, TaskCreationOptions.LongRunning).Unwrap();
    }

    /// <summary>
    /// Execute action periodically and asynchronously.
    /// </summary>
    /// <param name="onTick">Action to be executed.</param>
    /// <param name="dueTime">Time to pass before starting to execute.</param>
    /// <param name="interval">Interval between procs.</param>
    /// <param name="token">Cancellation token used to cancel the cycle.</param>
    /// <returns></returns>
    public static async Task RunPeriodicAsync(this Action onTick, TimeSpan dueTime, TimeSpan interval, CancellationToken token)
    {
        if (dueTime > TimeSpan.Zero)
        {
            await Task.Delay(dueTime, token).ConfigureAwait(false);
        }

        while (!token.IsCancellationRequested)
        {
            onTick?.Invoke();
            if (interval > TimeSpan.Zero)
            {
                await Task.Delay(interval, token).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Execute's an async Task<T> method which has a void return value synchronously
    /// </summary>
    /// <param name="task">Task<T> method to execute</param>
    public static void RunSync(this Func<Task> task)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
        {
            try
            {
                await task();
            }
            catch (Exception e)
            {
                synch.InnerException = e;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, default!);
        synch.BeginMessageLoop();

        SynchronizationContext.SetSynchronizationContext(oldContext);
    }

    /// <summary>
    /// Execute's an async Task<T> method which has a T return type synchronously
    /// </summary>
    /// <typeparam name="T">Return Type</typeparam>
    /// <param name="task">Task<T> method to execute</param>
    /// <returns></returns>
    public static T RunSync<T>(this Func<Task<T>> task)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        var ret = default(T);
        synch.Post(async _ =>
        {
            try
            {
                ret = await task();
            }
            catch (Exception e)
            {
                synch.InnerException = e;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, default!);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        return ret!;
    }
    
    private class ExclusiveSynchronizationContext : SynchronizationContext
    {
        private bool done;
        public Exception? InnerException { get; set; }
        readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
        readonly Queue<Tuple<SendOrPostCallback, object>> items =
            new Queue<Tuple<SendOrPostCallback, object>>();

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("We cannot send to our same thread");
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            lock (this.items)
            {
                this.items.Enqueue(Tuple.Create(d, state));
            }

            this.workItemsWaiting.Set();
        }

        public void EndMessageLoop()
        {
            this.Post(_ => this.done = true, default!);
        }

        public void BeginMessageLoop()
        {
            while (!this.done)
            {
                Tuple<SendOrPostCallback, object> task = default!;
                lock (this.items)
                {
                    if (this.items.Count > 0)
                    {
                        task = this.items.Dequeue();
                    }
                }

                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (this.InnerException != null) // the method threw an exeption
                    {
                        throw new AggregateException("AsyncHelpers.Run method threw an exception.", this.InnerException);
                    }
                }
                else
                {
                    this.workItemsWaiting.WaitOne();
                }
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
    }
}
