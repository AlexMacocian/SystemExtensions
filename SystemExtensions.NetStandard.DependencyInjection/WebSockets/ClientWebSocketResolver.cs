using Microsoft.Extensions.Logging;

namespace System.Net.WebSockets;
public sealed class ClientWebSocketResolver
{
    private static readonly Type ClientType = typeof(ClientWebSocket<>);
    private static readonly Type LoggerType = typeof(ILogger<>);

    public bool CanResolve(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IClientWebSocket<>))
        {
            return true;
        }

        return false;
    }

    public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
    {
        var typedClientType = ClientType.MakeGenericType(type.GetGenericArguments());
        var typedLoggerType = LoggerType.MakeGenericType(type.GetGenericArguments());
        var logger = serviceProvider.GetService(typedLoggerType);
        var client = Activator.CreateInstance(typedClientType, new object[] { logger });
        return client;
    }
}
