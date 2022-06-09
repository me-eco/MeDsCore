using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MeDsCore.Abstractions;
using MeDsCore.WebSocket.Base;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket.Gateway;

/// <summary>
/// Executes Gateway methods
/// </summary>
internal class DiscordGatewayMethodExecutor : IDiscordGatewayMethodExecutor
{
    private readonly IWebSocketClient _connector;
    private readonly ILogger _logger;

    public DiscordGatewayMethodExecutor(IWebSocketClient connector, ILogger logger)
    {
        _connector = connector;
        _logger = logger;
    }

    public async Task ExecuteAsync(DiscordGatewayMethodInfo info)
    {
        var jsonEntityInstance = new DiscordMethodEntity()
        {
            Op = (int) info.OpCode,
            D = info.JsonPayload
        };

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(jsonEntityInstance);

        await _connector.SendBytesAsync(jsonBytes, WebSocketMessageType.Text);
        
        _logger.LogTrace("Gateway method executed. Opcode: {OpCode}({IntOpCode}); Length: {Length}", 
            (int)info.OpCode, info.OpCode, jsonBytes.Length);
    }

    public async Task ExecuteAsync(string request)
    {
        var bytes = Encoding.UTF8.GetBytes(request);

        await _connector.SendBytesAsync(bytes, WebSocketMessageType.Text);
        _logger.LogTrace("Gateway anonymous method executed. Content: {Request}", request);
    }

    private class DiscordMethodEntity
    {
        [JsonPropertyName("op")]
        public int Op { get; set; }
        [JsonPropertyName("d")]
        public object D { get; set; }
    }
}