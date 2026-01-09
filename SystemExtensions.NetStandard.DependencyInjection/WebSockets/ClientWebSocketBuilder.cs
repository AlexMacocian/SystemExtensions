using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Net.WebSockets;

namespace System.Net.Http;

public sealed class ClientWebSocketBuilder<T>
{
    private readonly IServiceCollection services;

    private ClientWebSocket? innerWebSocket;
    private Func<IServiceProvider, ClientWebSocket>? innerWebSocketFactory;

    internal ClientWebSocketBuilder(IServiceCollection services)
    {
        services.ThrowIfNull(nameof(services));
        this.services = services;
    }

    public ClientWebSocketBuilder<T> WithInnerWebSocket(ClientWebSocket innerWebSocket)
    {
        this.innerWebSocket = innerWebSocket.ThrowIfNull(nameof(innerWebSocket));
        return this;
    }

    public ClientWebSocketBuilder<T> WithInnerWebSocketFactory(Func<IServiceProvider, ClientWebSocket> innerWebSocketFactory)
    {
        this.innerWebSocketFactory = innerWebSocketFactory.ThrowIfNull(nameof(innerWebSocketFactory));
        return this;
    }

    public IServiceCollection Build()
    {
        this.services.AddScoped<IClientWebSocket<T>>(sp =>
        {
            var logger = sp.GetService<ILogger<T>>();
            if (logger is null)
            {
                return new ClientWebSocket<T>();
            }

            if (this.innerWebSocketFactory is not null)
            {
                return new ClientWebSocket<T>(this.innerWebSocketFactory(sp), logger);
            }

            if (this.innerWebSocket is not null)
            {
                return new ClientWebSocket<T>(this.innerWebSocket, logger);
            }

            return new ClientWebSocket<T>(logger);
        });

        return this.services;
    }
}
