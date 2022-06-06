using System.Net.WebSockets;
using MeDsCore.Abstractions;
using MeDsCore.WebSocket.Base;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket.Gateway;

/// <summary>
/// Allows to create and manage a WS connection
/// </summary>
public class DiscordGatewayConnector : IAsyncDisposable, IWebSocketClient
{
    private readonly ILogger _logger;
    private readonly ClientWebSocket _clientWebSocket;
    private readonly List<Func<byte[], Task>> _callbackList;
    
    public bool IsConnected => _clientWebSocket.State == WebSocketState.Open;
    
    public DiscordGatewayConnector(ILogger logger)
    {
        _logger = logger;
        _clientWebSocket = new ClientWebSocket();
        _callbackList = new List<Func<byte[], Task>>();
    }

    /// <summary>
    /// Connects to Discord gateway
    /// </summary>
    public async Task ConnectAsync(Uri gatewayUri)
    {
        await _clientWebSocket.ConnectAsync(gatewayUri, CancellationToken.None);

        if (_clientWebSocket.State != WebSocketState.Open)
        {
            _logger.LogError("Unexpected WebSocket connection state. Expected WebSocketState.Open");
        }
        
        _logger.LogDebug("Connected to gateway");
    }

    public async Task ReconnectAsync(Uri gatewayUri)
    {
        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable,
            "CONNECTION_TERMINATION",
            CancellationToken.None);
        await ConnectAsync(gatewayUri);
    }
    
    /// <inheritdoc cref="IAsyncDisposable"/>
    public async ValueTask DisposeAsync()
    {
        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.Empty, null, CancellationToken.None);
        _clientWebSocket.Dispose();
        _logger.LogDebug("Gateway connector disposed");
    }

    public void RegisterCallback(Func<byte[], Task> callback)
    {
        _callbackList.Add(callback);
    }
    
    public async Task SendBytesAsync(ReadOnlyMemory<byte> memoryBytes, WebSocketMessageType messageType)
    {
        if (_clientWebSocket.State != WebSocketState.Open)
            throw new DiscordException("Can't send byte message because gateway closed", _logger);
        
        await _clientWebSocket.SendAsync(memoryBytes, messageType, true, CancellationToken.None);
    }

    public async Task<int> ReadAsync(byte[] buffer)
    { 
        return (await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None)).Count;
    }

    /// <summary>
    /// Starts reading WebSocket byte stream 
    /// </summary>
    /// <param name="bufferSize">Maximum buffer size</param>
    /// <param name="token">Cancellation token</param>
    public void StartReceiving(int bufferSize, CancellationToken token)
    {
        Task.Run(async () =>
        {
            byte[] buffer = new byte[bufferSize];
            while (!token.IsCancellationRequested && IsConnected)
            {
                var countReceived = await ReadAsync(buffer);

                if (countReceived > 0)
                {
                    _logger.LogTrace($"{countReceived} byte(-s) received from Discord Gateway");

                    var subBuffer = new byte[countReceived];
                    
                    for (var i = subBuffer.Length - 1; i >= 0; i--)
                    {
                        subBuffer[i] = buffer[i];
                    }
                    
                    foreach (var callback in _callbackList)
                    {
                        await callback(subBuffer);
                    }
                }

                await Task.Delay(1, token);
            }

            _logger.LogDebug("Receiving stopped");
        }, token);
    }
}