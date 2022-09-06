using Microsoft.Extensions.Options;

namespace System.Configuration;

public interface ILiveOptions<T> : IOptions<T>
    where T : class
{
}
