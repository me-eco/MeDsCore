using MeDsCore.Interactions.ApplicationCommands;
using Microsoft.Extensions.DependencyInjection;

namespace MeDsCore.Interactions;

public static class ClientExtensions
{
    public static ApplicationCommandsClient UseApplicationCommands(this DiscordClient client, IServiceProvider services, bool isDebug = false,  ulong debugGuildId = 0)
    {
        var appCommandsClient =
            new ApplicationCommandsClient(client.RestClient, client.WebSocketClient, client.Logger, services, isDebug, debugGuildId);
        
        client.WebSocketClient.AttachToSocket(appCommandsClient);

        return appCommandsClient;
    }

    public static ApplicationCommandsClient UseApplicationCommands(this DiscordClient client, bool isDebug = false,  ulong debugGuildId = 0) =>
        UseApplicationCommands(client, new ServiceCollection().BuildServiceProvider(), isDebug, debugGuildId);
}