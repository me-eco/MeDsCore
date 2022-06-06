using MeDsCore.Abstractions;
using MeDsCore.WebSocket.Base;
using MeDsCore.WebSocket.Gateway.Entities;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket.Gateway.WebSocketLifetimes;

internal class ConnectionStateHandler : IDisposable, IGatewayMessageHandler
{
    private CancellationTokenSource _handlingConnectionCancellationTokenSource = null!;
    private readonly IWebSocketClient _client;
    private readonly ILogger _logger;
    private readonly DiscordGatewayConnector _connector;
    private readonly IDiscordGatewayMethodExecutor _methodExecutor;
    private readonly WebSocketHeartbeatService _heartbeatService;
    private readonly string _token;
    private string? _sessionId;
    private int? _seqNumber;
    
    public ConnectionStateHandler(IWebSocketClient client, ILogger logger, DiscordGatewayConnector connector,
        DiscordGatewayConnectorEventsListener listener, string token, IDiscordGatewayMethodExecutor methodExecutor,
        WebSocketHeartbeatService heartbeatService)
    {
        _client = client;
        _logger = logger;
        _connector = connector;
        _token = token;
        _methodExecutor = methodExecutor;
        _heartbeatService = heartbeatService;
        listener.AddGatewayHandler(this);
    }

    public void RunHandlingConnection(Uri gatewayUri, int maxBufferSize)
    {
        _handlingConnectionCancellationTokenSource = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!_handlingConnectionCancellationTokenSource.IsCancellationRequested)
            {
                while (_client.IsConnected)
                {
                    //Wait while client has connection with Discord Gateway       
                    await Task.Delay(100);
                }

                _logger.LogDebug("Client disconnected. Reconnecting process started");
                await ReconnectAsync(gatewayUri, maxBufferSize);
            }
        });
        _logger.LogDebug("Handling WebSocket connection started");
    }   

    private async Task ReconnectAsync(Uri gatewayUri, int maxBufferSize)
    {
        _heartbeatService.StopService();
        
        if (_sessionId == null)
        {
            _handlingConnectionCancellationTokenSource.Cancel();
            _logger.LogError("Reconnecting process failed: session's id is null");
            return;
        }

        await _connector.ReconnectAsync(gatewayUri);
        _connector.StartReceiving(maxBufferSize, _handlingConnectionCancellationTokenSource.Token);
        _logger.LogDebug("Client reconnected");

        if (_seqNumber == null)
        {
            _logger.LogError("Invalid sequence number");
            return;
        }
        
        var resume = DiscordGatewayMethods.CreateResumeMethodInfo(_token, _sessionId, _seqNumber.Value);
        await _methodExecutor.ExecuteAsync(resume);
        _logger.LogDebug("Session resumed");
        _heartbeatService.RestartService(_methodExecutor);
    }
    
    public void Dispose()
    {
        _handlingConnectionCancellationTokenSource.Cancel();
        _handlingConnectionCancellationTokenSource.Dispose();
    }

    Task IGatewayMessageHandler.ProcessMessageAsync(DiscordGatewayMessage message)
    {
        if (message.Opcode != GatewayServerOpcode.Dispatch) return Task.CompletedTask;
        
        _seqNumber = message.Sequence;
        
        if(message.EventName != "READY") return Task.CompletedTask;
        
        var hello = message.ConvertJson<ReadyEntity>();
        _sessionId = hello.SessionId;
        return Task.CompletedTask;
    }
}