using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities.Args;

public class SendMessageArg
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}