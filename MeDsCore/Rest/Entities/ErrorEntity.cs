using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities;

internal class ErrorEntity
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}