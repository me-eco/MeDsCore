using System.Text.Json.Serialization;

namespace MeDsCore.Base;

public class MessageEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }
    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }
    [JsonPropertyName("author")]
    public UserEntity? Author { get; set; }
    [JsonPropertyName("member")]
    public GuildMemberEntity? Member { get; set; }
    [JsonPropertyName("content")]
    public string Content { get; set; }
}