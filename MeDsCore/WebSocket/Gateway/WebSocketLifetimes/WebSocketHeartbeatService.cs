using System.Text.Json;
using MeDsCore.Abstractions;
using MeDsCore.WebSocket.Base;
using Microsoft.Extensions.Logging;

namespace MeDsCore.WebSocket.Gateway.WebSocketLifetimes;

internal class WebSocketHeartbeatService : IDisposable, IGatewayMessageHandler
{
    private readonly IWebSocketClient _client;
    private readonly ILogger _logger;
    private CancellationTokenSource _stopHeartbeatingTokenSource;
    public int? SequenceNumber { get; private set; }

    private int _interval;

    public WebSocketHeartbeatService(IWebSocketClient client, ILogger logger, DiscordGatewayConnectorEventsListener eventsListener)
    {
        _client = client;
        _logger = logger;
        eventsListener.AddGatewayHandler(this);
    }

    public void StartService(int interval, IDiscordGatewayMethodExecutor gatewayMethodExecutor)
    {
        _stopHeartbeatingTokenSource = new CancellationTokenSource(); //Reset token
        _interval = interval;
        var random = new Random();
        var delay = TimeSpan.FromMilliseconds(_interval * random.NextDouble());
        const string heartBeatTemplate = "{\"op\":1,\"d\":SEQ_NUM}";
        
        Task.Run(async () =>
        {
            _logger.LogDebug("Heartbeating started with a {Delay} milliseconds interval", delay.TotalMilliseconds);
        
            while (!_stopHeartbeatingTokenSource.IsCancellationRequested && _client.IsConnected)
            {
                var heartBeatInfo = heartBeatTemplate.Replace("SEQ_NUM", SequenceNumber?.ToString() ?? "null");

                await gatewayMethodExecutor.ExecuteAsync(heartBeatInfo);
                await Task.Delay(delay, _stopHeartbeatingTokenSource.Token);
            }
        
            _logger.LogWarning("Heartbeating stopped");
        });
    }
    
    public void RestartService(IDiscordGatewayMethodExecutor gatewayMethodExecutor)
    {
        _logger.LogDebug("Restarting heartbeating service");
        StopService();
        StartService(_interval!, gatewayMethodExecutor);
    }
    
    public void StopService()
    {
        _stopHeartbeatingTokenSource.Cancel();
    }
    
    private async Task OnNewMessageReceived(byte[] arg)
    {
        await using var memStream = new MemoryStream(arg);
        var proto = await JsonSerializer.DeserializeAsync<DiscordObjectProto>(memStream);

        if (proto is null) throw new DiscordException("Failed to deserialize a proto of WS message", _logger);

        if (proto.Op == 0) SequenceNumber = proto.S!.Value; //New seq num received
    }

    private class DiscordObjectProto
    {
        public int Op { get; set; }
        public int? S { get; set; }
    }

    public void Dispose()
    {
        SequenceNumber = null;
        _stopHeartbeatingTokenSource.Dispose();
    }

    Task IGatewayMessageHandler.ProcessMessageAsync(DiscordGatewayMessage message)
    {
        if (message.Opcode != GatewayServerOpcode.Dispatch) return Task.CompletedTask;

        SequenceNumber = message.Sequence;
        return Task.CompletedTask;
    }
}