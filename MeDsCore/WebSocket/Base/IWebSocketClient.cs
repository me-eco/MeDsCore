using System.Net.WebSockets;

namespace MeDsCore.WebSocket.Base;

internal interface IWebSocketClient
{
    /// <summary>
    /// True if WS opened
    /// </summary>
    bool IsConnected { get; }
    /// <summary>
    /// Регистрирует асинхронного подписчика на получение данных с сокета 
    /// </summary>
    /// <param name="callback"></param>
    void RegisterCallback(Func<byte[], Task> callback);
    /// <summary>
    /// Sends bytes to the Server 
    /// </summary>
    /// <param name="memory">Bytes to send</param>
    Task SendBytesAsync(ReadOnlyMemory<byte> memory, WebSocketMessageType messageType);
    /// <summary>
    /// Reads bytes and writes them to the buffer
    /// </summary>
    Task<int> ReadAsync(byte[] buffer);
}