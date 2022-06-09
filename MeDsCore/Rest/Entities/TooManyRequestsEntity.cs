using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities;

public class TooManyRequestsEntity
{
    [JsonPropertyName("retry_after")]
    public double RetryAfter { get; set; }
}