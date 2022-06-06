namespace MeDsCore.WebSocket.Gateway;

/// <summary>
/// Предоставляет абстракции для обработки с WS сообщения
/// </summary>
public interface IGatewayMessageHandler
{
    /// <summary>
    /// Обрабатывает сообщение с WS
    /// </summary>
    /// <param name="message">Сообщение</param>
    Task ProcessMessageAsync(DiscordGatewayMessage message);
}