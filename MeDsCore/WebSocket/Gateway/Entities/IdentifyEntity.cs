using System.Text.Json.Serialization;

namespace MeDsCore.WebSocket.Gateway.Entities;

public class IdentifyEntity
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("intents")]
    public int Intents { get; set; }
    [JsonPropertyName("properties")]
    public PropertiesEntity Properties { get; set; } 
    
    public class PropertiesEntity
    {
        [JsonPropertyName("$os")]
        public string Os { get; set; }
        [JsonPropertyName("$browser")]
        public string Browser { get; set; }
        [JsonPropertyName("$device")]
        public string Device { get; set; } 
    }
}