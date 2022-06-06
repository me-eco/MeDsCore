using System.Text.Json.Serialization;

namespace MeDsCore.Base;

public class GuildMemberEntity
{
    [JsonPropertyName("user")]
    public UserEntity? User { get; set; }
    [JsonPropertyName("nick")]
    public string? Nick { get; set; }
}