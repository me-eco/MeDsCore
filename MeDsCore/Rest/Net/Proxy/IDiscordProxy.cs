namespace MeDsCore.Rest.Net.Proxy;

/// <summary>
/// Предоставляет интферейс для использования методов Discord API
/// </summary>
public interface IDiscordProxy
{
    /// <summary>
    /// Ping-ует Discord API Сервер
    /// </summary>
    /// <returns></returns>
    Task<long> PingAsync();
    /// <summary>
    /// Отправляет HTTP запрос на API сервер Discord
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
}