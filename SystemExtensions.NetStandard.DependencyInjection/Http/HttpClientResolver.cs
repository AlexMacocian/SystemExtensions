using Slim.Resolvers;
using System.Linq;
using System.Net.Http;

namespace System.Http;

[Obsolete($"Please use {nameof(Extensions.ServiceManagerExtensions.RegisterHttpClient)} for each use case of {nameof(IHttpClient<object>)}")]
public sealed class HttpClientResolver : IDependencyResolver
{
    private static readonly Type clientType = typeof(HttpClient<>);

    /// <summary>
    /// Factory method. <see cref="Type"/> parameter of the factory is the scope of <see cref="IHttpClient{TScope}"/>.
    /// </summary>
    public Func<Slim.IServiceProvider, Type, HttpMessageHandler> HttpMessageHandlerFactory { get; set; }

    /// <summary>
    /// Sets the <see cref="HttpMessageHandlerFactory"/>.
    /// </summary>
    /// <param name="factory">Factory method. <see cref="Type"/> parameter of the factory is the scope of <see cref="IHttpClient{TScope}"/>.</param>
    /// <returns></returns>
    public HttpClientResolver WithHttpMessageHandlerFactory(Func<Slim.IServiceProvider, Type, HttpMessageHandler> factory)
    {
        this.HttpMessageHandlerFactory = factory;
        return this;
    }

    public bool CanResolve(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IHttpClient<>))
        {
            return true;
        }

        return false;
    }

    public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
    {
        var typedClientType = clientType.MakeGenericType(type.GetGenericArguments());
        var handler = this.HttpMessageHandlerFactory?.Invoke(serviceProvider, type.GetGenericArguments().First());
        var httpClient = handler is null ?
            Activator.CreateInstance(typedClientType) :
            Activator.CreateInstance(typedClientType, new object[] { handler });
        return httpClient;
    }
}