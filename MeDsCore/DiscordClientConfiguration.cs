using MeDsCore.WebSocket.Gateway;

namespace MeDsCore;

public class DiscordClientConfiguration
{
    public AuthorizationProvider AuthorizationProvider { get; set; }
    public GatewayIntents Intents { get; set; } = GatewayIntents.None;
    public string Domain { get; set; } = "https://discord.com";
    public int RestApiVersion { get; set; } = 10;

    public DiscordClientConfiguration(string token)
    {
        AuthorizationProvider = AuthorizationProvider.FromStaticToken(token);
    }

    public DiscordClientConfiguration(AuthorizationProvider authorizationProvider)
    {
        AuthorizationProvider = authorizationProvider;
    }
}