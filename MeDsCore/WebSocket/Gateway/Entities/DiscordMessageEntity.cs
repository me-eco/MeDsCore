using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class DiscordMessageEntity
{
    [JsonPropertyName("op")]
    public GatewayServerOpcode Op { get; set; }
    [JsonPropertyName("d")]
    public dynamic? D { get; set; }
    [JsonPropertyName("s")]
    public int? S { get; set; }
    [JsonPropertyName("t")]
    public string? T { get; set; }
}