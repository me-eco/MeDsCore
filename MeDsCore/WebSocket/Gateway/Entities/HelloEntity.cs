using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class HelloEntity
{
    [JsonPropertyName("heartbeat_interval")]
    public int HeartbeatInterval { get; set; }
}