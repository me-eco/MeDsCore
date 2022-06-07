using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities;

internal class GetGatewayEntity
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}