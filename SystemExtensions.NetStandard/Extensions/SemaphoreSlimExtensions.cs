using System.Threading;
using System.Threading.Tasks;

namespace System.Extensions;
public static class SemaphoreSlimExtensions
{
    public static async Task<SemaphoreSlimContext> Acquire(this SemaphoreSlim semaphore)
    {
        return await SemaphoreSlimContext.Create(semaphore);
    }
}
