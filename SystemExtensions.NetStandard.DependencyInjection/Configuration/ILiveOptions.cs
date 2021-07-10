using Microsoft.Extensions.Options;

namespace System.Extensions.Configuration
{
    public interface ILiveOptions<T> : IOptions<T>
        where T : class
    {
    }
}
