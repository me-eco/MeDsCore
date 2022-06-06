namespace MeDsCore.WebSocket.Gateway;

public interface IDiscordGatewayMethodExecutor
{
    Task ExecuteAsync(DiscordGatewayMethodInfo info);
    Task ExecuteAsync(string request);
}