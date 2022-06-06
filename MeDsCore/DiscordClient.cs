using MeDsCore.Abstractions;
using MeDsCore.Rest;
using MeDsCore.WebSocket;
using Microsoft.Extensions.Logging;

namespace MeDsCore;

public class DiscordClient
{
    public DiscordRestClient RestClient { get; }
    public DiscordWebSocketClient WebSocketClient { get; }
    public ILogger<DiscordClient> Logger { get; }
    public ulong? ApplicationId => WebSocketClient.ApplicationId;
    
    public DiscordClient(ILogger<DiscordClient> logger, DiscordClientConfiguration configuration)
    {
        Logger = logger;

        RestClient = new DiscordRestClient(
            configuration.Domain + $"/api/v{configuration.RestApiVersion}",
            configuration.AuthorizationProvider, Logger);
        WebSocketClient = new DiscordWebSocketClient(RestClient, configuration.AuthorizationProvider, Logger, configuration.Intents.Intents);
    }

    public async Task StartAsync()
    {
        Logger.LogInformation("me.ds module");
        Logger.LogInformation("Copyright 2022 Me Organisation. All rights reserved");
        await WebSocketClient.StartAsync();
    }
    
    public Task<IDiscordGuild?> GetGuildAsync(ulong guildId) => RestClient.GetGuildAsync(guildId);
}