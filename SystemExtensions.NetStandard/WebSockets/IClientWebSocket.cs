using System.Threading.Tasks;
using System.Threading;

namespace System.Net.WebSockets;

public interface IClientWebSocket<TScope>
{
    WebSocketCloseStatus? CloseStatus { get; }
    
    string CloseStatusDescription { get; }
    
    ClientWebSocketOptions Options { get; }
    
    WebSocketState State { get; }
    
    string SubProtocol { get; }

    void Abort();

    Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

    Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);

    Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

    void Dispose();

    Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

    Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);

    void RefreshSocket(ClientWebSocket? clientWebSocket = default);
}
