using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.WebSocket.Base;
using MeDsCore.WebSocket.Gateway.Entities;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket.Gateway;

/// <summary>
/// Listens messages from the Discord Gateway and converts them to <see cref="DiscordGatewayMessage"/>
/// </summary>
internal class DiscordGatewayConnectorEventsListener
{
    private readonly ILogger _logger;
    private readonly ICollection<IGatewayMessageHandler> _messageHandlers;

    public DiscordGatewayConnectorEventsListener(IWebSocketClient connector, ILogger logger)
    {
        _messageHandlers = new List<IGatewayMessageHandler>();
        _logger = logger;
        connector.RegisterCallback(ConnectorOnMessageReceived);
    }

    public void AddGatewayHandler(IGatewayMessageHandler handler)
    {
        _messageHandlers.Add(handler);
    }

    private async Task ConnectorOnMessageReceived(byte[] message)
    {
        var apiEntity = JsonSerializer.Deserialize<DiscordMessageEntity>(message);
        var collectionSnapshot = _messageHandlers.ToArray();
        if (apiEntity == null) throw new DiscordException("Failed to deserialize bytes as Discord WS message");
        
        _logger.LogTrace("Discord OP message received; Op: {ApiEntity} ({IntApiEntity}); Length: {message.Length}; Event: {Event}", 
            (int)apiEntity.Op, apiEntity.Op, message.Length, apiEntity.T ?? "NONE");
        
        for (var i = collectionSnapshot.Length - 1; i >= 0; i--)
        {
            var connectorEventsListener = collectionSnapshot[i];
            await connectorEventsListener.ProcessMessageAsync(new DiscordGatewayMessage(
                apiEntity.Op, apiEntity.D, apiEntity.S, apiEntity.T));
        }
    }
}