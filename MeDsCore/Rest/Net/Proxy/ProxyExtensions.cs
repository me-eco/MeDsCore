namespace MeDsCore.Rest.Net.Proxy;

public static class ProxyExtensions
{
    public static async Task<bool> IsAvailableAsync(this IDiscordProxy proxy) => await proxy.PingAsync() > 0;
}