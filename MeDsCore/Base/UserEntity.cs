using System.Text.Json.Serialization;

namespace MeDsCore.Base;

internal class UserEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("bot")]
    public bool Bot { get; set; }
}