using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class DiscordWsMessageScheme<T>
{
    [JsonPropertyName("op")]
    public int Op { get; set; }
    [JsonPropertyName("d")]
    public T D { get; set; }
}