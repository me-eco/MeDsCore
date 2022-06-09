using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.Base;
using MeDsCore.Rest;
using MeDsCore.Rest.Entities;
using MeDsCore.Rest.Extensions;
using MeDsCore.Rest.Net;
using MeDsCore.Rest.Net.Content;
using MeDsCore.Rest.Net.Methods;
using MeDsCore.WebSocket.Gateway;
using MeDsCore.WebSocket.Gateway.Entities;
using MeDsCore.WebSocket.Gateway.WebSocketEntities;
using MeDsCore.WebSocket.Gateway.WebSocketLifetimes;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket;

public class DiscordWebSocketClient : IAsyncDisposable, IGatewayMessageHandler
{
    private readonly IDiscordGatewayMethodExecutor _gatewayMethodExecutor;
    private readonly DiscordRestClient _client;
    private readonly AuthorizationProvider _authProvider;
    private readonly ILogger _logger;
    private readonly DiscordGatewayConnector _gatewayConnector;
    private readonly DiscordGatewayConnectorEventsListener _eventsListener;
    private readonly WebSocketHeartbeatService _heartbeatService;
    private readonly ConnectionStateHandler _connectionStateHandler;
    private readonly int _intents;

    private const int DefaultBufferSize = 1 << 15; //32 Kb
    
    public ulong? ApplicationId { get; private set; }
    public bool IsReady { get; private set; } = false;
    
    public DiscordWebSocketClient(DiscordRestClient client, AuthorizationProvider authProvider,
        ILogger logger, int intents)
    {
        _client = client;
        _authProvider = authProvider;
        _logger = logger;
        _gatewayConnector = new DiscordGatewayConnector(_logger);
        _gatewayMethodExecutor = new DiscordGatewayMethodExecutor(_gatewayConnector, logger);
        _eventsListener = new DiscordGatewayConnectorEventsListener(_gatewayConnector, _logger);
        _heartbeatService = new WebSocketHeartbeatService(_gatewayConnector, _logger, _eventsListener);
        _connectionStateHandler = new ConnectionStateHandler(_gatewayConnector, logger, _gatewayConnector,
            _eventsListener, authProvider.GetToken(), _gatewayMethodExecutor, _heartbeatService);
        _eventsListener.AddGatewayHandler(this);
        _intents = intents;
    }

    public void AttachToSocket(IGatewayMessageHandler gatewayMessageHandler)
    {
        _eventsListener.AddGatewayHandler(gatewayMessageHandler);
    }
    
    /// <summary>
    /// Creates WebSocket connection with Discord API Gateway
    /// </summary>
    /// <returns>Abstraction to execute Gateway methods</returns>
    public async Task StartAsync()
    {
        var gatewayUri = await GetGatewayAsync();
        await _gatewayConnector.ConnectAsync(gatewayUri);
        
        _gatewayConnector.StartReceiving(DefaultBufferSize, CancellationToken.None);
        _connectionStateHandler.RunHandlingConnection(gatewayUri, DefaultBufferSize);
        _logger.LogInformation("Receiving started");
    }
    
    private async Task CheckEventsAsync(DiscordGatewayMessage msg)
    {
        if (msg.Opcode != GatewayServerOpcode.Dispatch) return;
        if (msg.EventName == null) throw new DiscordException("Invalid WS message signature", _logger);

        Task? HandleReady()
        {
            IsReady = true;
            var readyEnt = msg.ConvertJson<ReadyEntity>();
            var readyObj = new Ready(readyEnt);

            ApplicationId = readyObj.Application.Id;

            return OnReady?.Invoke(readyObj);
        }

        var @event = msg.EventName switch
        {
            GatewayEvents.GuildCreate => OnGuildCreated?.Invoke(new WebSocketGuild(_client.MethodExecutor,
                msg.ConvertJson<GuildEntity>())),
            GatewayEvents.GuildDelete => OnGuildDeleted?.Invoke(new UnavailableGuild(msg.ConvertJson<GuildEntity>())),
            GatewayEvents.MessageCreated => OnMessageReceived?.Invoke(await WebSocketMessage.InitializeAsync(_client.MethodExecutor,
                msg.ConvertJson<MessageEntity>())),
            GatewayEvents.Ready => HandleReady(),
            _ => null
        };

        if(@event != null) await @event;
    }
    
    private async Task IdentifyAsync(string token)
    {
        var methodInfo = DiscordGatewayMethods.CreateIdentifyMethodInfo(token, _intents, "me-ds", "me-ds", "me-ds");

        await _gatewayMethodExecutor.ExecuteAsync(methodInfo);
    }

    /// <summary>
    /// Fires when Bot joins to a new Discord guild
    /// </summary>
    public event Func<WebSocketGuild, Task>? OnGuildCreated;

    public event Func<UnavailableGuild, Task>? OnGuildDeleted; 

    /// <summary>
    /// Fires when Bot receives user's message
    /// </summary>
    public event Func<WebSocketMessage, Task>? OnMessageReceived;

    public event Func<Ready, Task>? OnReady;
    
    /// <summary>
    /// Contains available Discord Gateway's events codes
    /// </summary>
    private static class GatewayEvents
    {
        public const string GuildCreate = "GUILD_CREATE";
        public const string GuildDelete = "GUILD_DELETE";
        public const string MessageCreated = "MESSAGE_CREATE";
        public const string Ready = "READY";
    }
    
    private async Task<Uri> GetGatewayAsync() //Gets Discord's gateway
    {
        var methodExecutor = _client.MethodExecutor;
        var getGatewayMethodInfo = GatewayMethods.ConfigureGetGatewayMethodInfo();
        var gatewayEntity = await methodExecutor.ExecuteMethodAsync<GetGatewayEntity>(getGatewayMethodInfo, IContentBuilder.EmptyContent);

        if (gatewayEntity == null) 
            throw new DiscordException("Failed to deserialize gateway entity response", _logger);
        
        _logger.LogTrace("Discord Gateway: {Url}", gatewayEntity.Url);
        
        return new Uri(gatewayEntity.Url + "?v=9&encoding=json");
    }

    public async ValueTask DisposeAsync()
    {
        await _gatewayConnector.DisposeAsync();
    }
    
    /// <inheritdoc cref="IGatewayMessageHandler"/>
    async Task IGatewayMessageHandler.ProcessMessageAsync(DiscordGatewayMessage message)
    {
        if (message.Opcode == GatewayServerOpcode.Hello)
        {
            var helloObject = message.ConvertJson<HelloEntity>();
            await IdentifyAsync(_authProvider.GetToken());
            _heartbeatService.StartService(helloObject.HeartbeatInterval, _gatewayMethodExecutor!);
            return;
        }

        await CheckEventsAsync(message);
    }
}
