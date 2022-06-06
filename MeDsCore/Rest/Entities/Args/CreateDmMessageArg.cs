using System.Text.Json.Serialization;

namespace MeDsCore.Rest.Entities.Args;

public class CreateDmMessageArg
{
    [JsonPropertyName("recipient_id")]
    public string RecipientId { get; set; }
}