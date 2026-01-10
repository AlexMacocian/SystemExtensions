using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

// TODO: Cannot properly test because ClientWebSocket is sealed
public sealed class ClientWebSocket<TScope> : IClientWebSocket<TScope>, IDisposable
{
    private readonly ILogger<TScope>? logger;
    private ClientWebSocket internalWebSocket = new();

    public ClientWebSocketOptions Options => this.internalWebSocket.Options;

    public WebSocketCloseStatus? CloseStatus => this.internalWebSocket.CloseStatus;

    public string CloseStatusDescription => this.internalWebSocket.CloseStatusDescription;

    public string SubProtocol => this.internalWebSocket.SubProtocol;

    public WebSocketState State => this.internalWebSocket.State;

    public ClientWebSocket()
    {
    }

    public ClientWebSocket(ILogger<TScope> logger)
    {
        this.logger = logger.ThrowIfNull(nameof(logger));
    }

    public ClientWebSocket(ClientWebSocket clientWebSocket, ILogger<TScope> logger)
    {
        this.internalWebSocket = clientWebSocket.ThrowIfNull(nameof(clientWebSocket));
        this.logger = logger.ThrowIfNull(nameof(logger));
    }

    public void Dispose()
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(Dispose), string.Empty);
        scopedLogger?.LogDebug($"Disposing websocket");
        this.internalWebSocket?.Dispose();
    }

    public void Abort()
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(Abort), string.Empty);
        scopedLogger?.LogDebug($"Aborting websocket");
        this.internalWebSocket.Abort();
    }

    public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(CloseAsync), string.Empty);
        scopedLogger?.LogDebug($"Closing websocket. Status [{closeStatus}]. Status Description [{statusDescription}]");
        return this.internalWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }

    public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(CloseOutputAsync), string.Empty);
        scopedLogger?.LogDebug($"Closing output. Status [{closeStatus}]. Status Description [{statusDescription}]");
        return this.internalWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
    }

    public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(ConnectAsync), string.Empty);
        scopedLogger?.LogDebug($"Connecting to {uri}");
        return this.internalWebSocket.ConnectAsync(uri, cancellationToken);
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(ConnectAsync), string.Empty);
        scopedLogger?.LogDebug($"Attempting to receive bytes");
        var result = await this.internalWebSocket.ReceiveAsync(buffer, cancellationToken);
        scopedLogger?.LogDebug($"Received message [{result.MessageType}]");
        scopedLogger?.LogDebug($"Type: {result.MessageType}\nCount: {result.Count}\nEndOfMessage: {result.EndOfMessage}\nCloseStatus: {result.CloseStatus}\nCloseStatusDescription: {result.CloseStatusDescription}");

        return result;
    }

    public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger?.CreateScopedLogger(nameof(SendAsync), string.Empty);
        scopedLogger?.LogDebug($"Sending bytes");
        scopedLogger?.LogDebug($"Type: {messageType}\nCount: {buffer.Count}\nEndOfMessage: {endOfMessage}");
        return this.internalWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

    public void RefreshSocket(ClientWebSocket? clientWebSocket = null)
    {
        this.internalWebSocket?.Dispose();
        this.internalWebSocket = clientWebSocket ?? new ClientWebSocket();
    }
}
