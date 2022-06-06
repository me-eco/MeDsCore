using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities;

public class GetGatewayEntity
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}