using System.Net.Http;

namespace System.DependencyInjection.Models;

internal sealed class TypedHttpClientFactory<T>
{
    public Func<Slim.IServiceProvider, HttpMessageHandler> Factory { get; set; }
}
