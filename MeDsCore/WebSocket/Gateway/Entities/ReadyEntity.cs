using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class ReadyEntity
{
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }
    [JsonPropertyName("application")]
    public ApplicationEntity Application { get; set; }
}

public class ApplicationEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}