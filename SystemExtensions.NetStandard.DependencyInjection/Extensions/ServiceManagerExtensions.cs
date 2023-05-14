using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slim;
using System.Configuration;
using System.Logging;
using System.Net.Http;

namespace System.Extensions;

public static class ServiceManagerExtensions
{
    /// <summary>
    /// Registers a <see cref="IOptionsManager"/> with the default <see cref="DefaultOptionsManager"/>.
    /// This call also registers the resolver that resolves <see cref="IUpdateableOptions{T}"/> and <see cref="IOptions{T}"/>.
    /// </summary>
    /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
    /// <returns>Provided <see cref="IServiceManager"/>.</returns>
    public static IServiceProducer RegisterOptionsManager(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterSingleton<IOptionsManager, DefaultOptionsManager>();
        serviceProducer.RegisterOptionsResolver();

        return serviceProducer;
    }

    /// <summary>
    /// Registers a <see cref="IOptionsManager"/>.
    /// This call also registers the resolver that resolves <see cref="IUpdateableOptions{T}"/> and <see cref="IOptions{T}"/>.
    /// </summary>
    /// <typeparam name="T">Implementation of <see cref="IOptionsManager"/>.</typeparam>
    /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
    /// <returns>Provided <see cref="IServiceManager"/>.</returns>
    public static IServiceProducer RegisterOptionsManager<T>(this IServiceProducer serviceProducer)
        where T : IOptionsManager
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterSingleton<IOptionsManager, T>();
        serviceProducer.RegisterOptionsResolver();

        return serviceProducer;
    }

    /// <summary>
    /// Registers resolvers for <see cref="IOptions{TOptions}"/> and <see cref="IUpdateableOptions{T}"/>.
    /// Depends on a <see cref="IOptionsManager"/> in order to properly resolve options.
    /// </summary>
    /// <param name="serviceManager"><see cref="IServiceManager"/>.</param>
    /// <returns><see cref="IServiceManager"/>.</returns>
    public static IServiceProducer RegisterOptionsResolver(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterResolver(new OptionsResolver());
        serviceProducer.RegisterResolver(new UpdateableOptionsResolver());
        serviceProducer.RegisterResolver(new LiveOptionsResolver());
        serviceProducer.RegisterResolver(new LiveUpdateableOptionsResolver());

        return serviceProducer;
    }

    /// <summary>
    /// Registers a <see cref="ILogsWriter"/> with the default <see cref="CVLoggerProvider"/>.
    /// </summary>
    /// <typeparam name="TLogsWriter">Implementation of <see cref="ILogsWriter"/>.</typeparam>
    /// <param name="serviceManager"><see cref="IServiceProducer"/>.</param>
    /// <returns>Provided <see cref="IServiceProducer"/>.</returns>
    public static IServiceProducer RegisterLogWriter<TLogsWriter>(this IServiceProducer serviceManager)
        where TLogsWriter : ILogsWriter
    {
        serviceManager.ThrowIfNull(nameof(serviceManager));

        serviceManager.RegisterSingleton<ILogsWriter, TLogsWriter>();
        serviceManager.RegisterScoped<ILoggerFactory, LoggerFactory>(sp =>
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new CVLoggerProvider(sp.GetService<ILogsWriter>()));
            return factory;
        });

        return serviceManager;
    }

    /// <summary>
    /// Registers a <see cref="ILogsWriter"/> with the default <see cref="CVLoggerProvider"/>.
    /// </summary>
    /// <typeparam name="TILogsWriter">Interface of <see cref="ILogsWriter"/>.</typeparam>
    /// <typeparam name="TLogsWriter">Implementation of <see cref="ILogsWriter"/>.</typeparam>
    /// <param name="serviceManager"><see cref="IServiceProducer"/>.</param>
    /// <returns>Provided <see cref="IServiceProducer"/>.</returns>
    public static IServiceProducer RegisterLogWriter<TILogsWriter, TLogsWriter>(this IServiceProducer serviceManager)
        where TLogsWriter : TILogsWriter
        where TILogsWriter : class, ILogsWriter
    {
        serviceManager.ThrowIfNull(nameof(serviceManager));

        serviceManager.RegisterSingleton<TILogsWriter, TLogsWriter>();
        serviceManager.RegisterScoped<ILoggerFactory, LoggerFactory>(sp =>
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new CVLoggerProvider(sp.GetService<TILogsWriter>()));
            return factory;
        });

        return serviceManager;
    }

    /// <summary>
    /// Register a <see cref="ILoggerFactory"/> with a <see cref="CVLoggerProvider"/>.
    /// </summary>
    /// <param name="serviceProducer"></param>
    /// <returns></returns>
    public static IServiceProducer RegisterCVLoggerFactory(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterScoped<ILoggerFactory, LoggerFactory>(sp =>
        {
            LoggerFactory loggerFactory = new();
            loggerFactory.AddProvider(new CVLoggerProvider(sp.GetService<ILogsWriter>()));
            return loggerFactory;
        });

        return serviceProducer;
    }

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
    /// Registers a <see cref="ILoggerFactory"/> with a <see cref="ILogsWriter"/> that writes to <see cref="Diagnostics.Debug"/>.
    /// </summary>
    /// <param name="serviceProducer"><see cref="IServiceProducer"/>.</param>
    /// <returns></returns>
    public static IServiceProducer RegisterDebugLoggerFactory(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        serviceProducer.RegisterLogWriter<ILogsWriter, DebugLogsWriter>();
        return serviceProducer;
    }

    /// <summary>
    /// Register a scoped <see cref="IHttpClient{TScope}"/> to be used by the DI engine.
    /// </summary>
    /// <typeparam name="T">Type of the scoped <see cref="IHttpClient{TScope}"/>.</typeparam>
    /// <param name="serviceProducer"><see cref="IServiceProducer"/>.</param>
    /// <returns><see cref="HttpClientBuilder{T}"/> to build the http client.</returns>
    public static HttpClientBuilder<T> RegisterHttpClient<T>(this IServiceProducer serviceProducer)
    {
        serviceProducer.ThrowIfNull(nameof(serviceProducer));

        return new HttpClientBuilder<T>(serviceProducer);
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
