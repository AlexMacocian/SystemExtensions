using System.Extensions;
using System.Threading.Tasks;

namespace System.Threading;
public readonly struct SemaphoreSlimContext : IDisposable
{
    private readonly SemaphoreSlim semaphore;

    private SemaphoreSlimContext(SemaphoreSlim semaphoreSlim)
    {
        this.semaphore = semaphoreSlim.ThrowIfNull(nameof(semaphoreSlim));
    }

    public void Dispose()
    {
        this.semaphore.Release();
    }

    public static async Task<SemaphoreSlimContext> Create(SemaphoreSlim semaphore)
    {
        await semaphore.ThrowIfNull(nameof(semaphore)).WaitAsync();
        return new SemaphoreSlimContext(semaphore);
    }

    public static async Task<SemaphoreSlimContext> Create(SemaphoreSlim semaphore, CancellationToken cancellationToken)
    {
        await semaphore.ThrowIfNull(nameof(semaphore)).WaitAsync(cancellationToken);
        return new SemaphoreSlimContext(semaphore);
    }
}
