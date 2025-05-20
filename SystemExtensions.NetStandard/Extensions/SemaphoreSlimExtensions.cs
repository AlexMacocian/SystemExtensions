using System.Threading;
using System.Threading.Tasks;

namespace System.Extensions;
public static class SemaphoreSlimExtensions
{
    public static async Task<SemaphoreSlimContext> Acquire(this SemaphoreSlim semaphore)
    {
        return await SemaphoreSlimContext.Create(semaphore);
    }

    public static async Task<SemaphoreSlimContext> Acquire(this SemaphoreSlim semaphore, CancellationToken cancellationToken)
    {
        return await SemaphoreSlimContext.Create(semaphore, cancellationToken);
    }
}
