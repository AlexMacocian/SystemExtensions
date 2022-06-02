using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System;

/// <summary>
/// Async implementation of <see cref="Lazy{T}"/>.
/// </summary>
/// <remarks>
/// Credits: https://devblogs.microsoft.com/pfxteam/asynclazyt/
/// </remarks>
/// <typeparam name="T">Type of value to be returned.</typeparam>
public sealed class AsyncLazy<T> : Lazy<Task<T>>
{
    public TaskAwaiter<T> GetAwaiter() => this.Value.GetAwaiter();

    public AsyncLazy(Func<T> valueFactory)
        : base(() => Task.Factory.StartNew(valueFactory))
    {
    }

    public AsyncLazy(Func<Task<T>> taskFactory)
        : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())
    {
    }
}
