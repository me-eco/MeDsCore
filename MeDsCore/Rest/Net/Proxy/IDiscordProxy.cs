namespace MeDsCore.Rest.Net.Proxy;

public interface IDiscordProxy
{
    /// <summary>
    /// Pings Discord server
    /// </summary>
    /// <returns></returns>
    Task<long> PingAsync();
    /// <summary>
    /// Sends a HTTP request to the Discord API server
    /// </summary>
    /// <param name="message">HTTP request message</param>
    /// <returns>HTTP response</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
}