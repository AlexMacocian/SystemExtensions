using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.WebSockets;

namespace System.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register a scoped <see cref="IHttpClient{TScope}"/> to be used by the DI engine.
    /// </summary>
    /// <typeparam name="T">Type of the scoped <see cref="IHttpClient{TScope}"/>.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <returns><see cref="HttpClientBuilder{T}"/> to build the http client.</returns>
    public static HttpClientBuilder<T> RegisterHttpClient<T>(this IServiceCollection services)
    {
        services.ThrowIfNull(nameof(services));

        return new HttpClientBuilder<T>(services);
    }

    /// <summary>
    /// Register a scoped <see cref="IClientWebSocket{TScope}"/> to be used by the DI engine.
    /// </summary>
    /// <typeparam name="T">Type of the scoped <see cref="IClientWebSocket{TScope}"/>.</typeparam>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <returns><see cref="ClientWebSocketBuilder{T}"/> to build the websocket client.</returns>
    public static ClientWebSocketBuilder<T> RegisterClientWebSocket<T>(this IServiceCollection services)
    {
        services.ThrowIfNull(nameof(services));

        return new ClientWebSocketBuilder<T>(services);
    }
}
