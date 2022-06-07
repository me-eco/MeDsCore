using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities.Args;

internal class CreateDmMessageArg
{
    [JsonPropertyName("recipient_id")]
    public string RecipientId { get; set; }
}