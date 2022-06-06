namespace MeDsCore.WebSocket.Gateway;

public interface IDiscordEventsListener
{
    public event Func<DiscordGatewayMessage, Task>? OnMessageReceived;
}