namespace MeDsCore.WebSocket.Gateway;

public interface IGatewayMessageHandler
{
    /// <summary>
    /// Handles message from Gateway
    /// </summary>
    /// <param name="message">Message</param>
    Task ProcessMessageAsync(DiscordGatewayMessage message);
}