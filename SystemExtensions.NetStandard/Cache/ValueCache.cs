using System.Extensions;

namespace System.Cache;
public sealed class ValueCache<T>(Func<T> cacheRefreshOperation, TimeSpan cacheDuration)
    where T : class
{
    private readonly Func<T> cacheRefreshOperation = cacheRefreshOperation.ThrowIfNull(nameof(cacheRefreshOperation));
    private readonly TimeSpan cacheDuration = cacheDuration;

    private T? value;
    private DateTime lastCacheRefresh = DateTime.MinValue;

    public T GetValue()
    {
        if (DateTime.Now - this.lastCacheRefresh < this.cacheDuration &&
            this.value is not null)
        {
            return this.value;
        }

        return this.PerformCacheRefresh();
    }

    public T ForceCacheRefresh()
    {
        return this.PerformCacheRefresh();
    }

    private T PerformCacheRefresh()
    {
        this.value = this.cacheRefreshOperation();
        this.lastCacheRefresh = DateTime.Now;
        return this.value;
    }
}
