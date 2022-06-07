using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities.Args;

internal class SendMessageArg
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}