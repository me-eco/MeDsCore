using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities;

public class ErrorEntity
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}