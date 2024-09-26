using System.Extensions;
using System.Threading.Tasks;

namespace System.Cache;
public sealed class AsyncValueCache<T>(Func<Task<T>> cacheRefreshOperation, TimeSpan cacheDuration)
    where T : class
{
    private readonly Func<Task<T>> cacheRefreshOperation = cacheRefreshOperation.ThrowIfNull(nameof(cacheRefreshOperation));
    private readonly TimeSpan cacheDuration = cacheDuration;

    private T? value;
    private DateTime lastCacheRefresh = DateTime.MinValue;

    public async Task<T> GetValue()
    {
        if (DateTime.Now - this.lastCacheRefresh < this.cacheDuration &&
            this.value is not null)
        {
            return this.value;
        }

        return await this.PerformCacheRefresh();
    }

    public async Task<T> ForceCacheRefresh()
    {
        return await this.PerformCacheRefresh();
    }

    private async Task<T> PerformCacheRefresh()
    {
        this.value = await this.cacheRefreshOperation();
        this.lastCacheRefresh = DateTime.Now;
        return this.value;
    }
}
