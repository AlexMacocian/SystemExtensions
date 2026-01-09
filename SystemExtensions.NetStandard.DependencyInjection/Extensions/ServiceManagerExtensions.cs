using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slim;
using System.Net.Http;
using System.Net.WebSockets;

namespace System.Extensions;

public static class ServiceManagerExtensions
{
    /// <summary>
    /// Registers a <see cref="ILoggerFactory"/>.
    /// </summary>
    /// <param name="serviceProducer"><see cref="IServiceProducer"/>.</param>
    /// <param name="loggerFactory">Factory that produces a <see cref="ILoggerFactory"/>.</param>
    /// <returns></returns>
    public static IServiceProducer RegisterLoggerFactory(this IServiceProducer serviceProducer, Func<Slim.IServiceProvider, ILoggerFactory> loggerFactory)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterSingleton<ILoggerFactory, ILoggerFactory>(loggerFactory);
        return serviceProducer;
    }

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

    [Obsolete($"{nameof(RegisterHttpFactory)} is obsolete. Please use {nameof(RegisterHttpClient)} for each service that requires an instance of {nameof(IHttpClient<object>)}.")]
    public static IServiceProducer RegisterHttpFactory(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterResolver(new HttpClientResolver());
        return serviceProducer;
    }
    [Obsolete($"{nameof(RegisterHttpFactory)} is obsolete. Please use {nameof(RegisterHttpClient)} for each service that requires an instance of {nameof(IHttpClient<object>)}.")]
    public static IServiceProducer RegisterHttpFactory(this IServiceProducer serviceProducer, Func<Slim.IServiceProvider, Type, HttpMessageHandler> handlerFactory)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterResolver(
            new HttpClientResolver()
            .WithHttpMessageHandlerFactory(handlerFactory));
        return serviceProducer;
    }
}
