using System.ComponentModel.DataAnnotations;
using MeDsCore.WebSocket.Gateway;

namespace MeDsCore;

public class DiscordClientConfiguration
{
    [Required]
    public AuthorizationProvider AuthorizationProvider { get; set; }
    [Required]
    public GatewayIntents Intents { get; set; } = GatewayIntents.None;
    [Required]
    public string Domain { get; set; } = "https://discord.com";
    [Required]
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