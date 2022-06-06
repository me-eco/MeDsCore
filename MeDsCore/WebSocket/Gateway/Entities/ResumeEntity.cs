using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class ResumeEntity
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }
    [JsonPropertyName("seq")]
    public int Seq { get; set; }
}