using System.Text.Json.Serialization;

namespace MeDsCore.Base;

internal class GuildMemberEntity
{
    [JsonPropertyName("user")]
    public UserEntity? User { get; set; }
    [JsonPropertyName("nick")]
    public string? Nick { get; set; }
}