using System.Text.Json;
using System.Text.Json.Serialization;
using MeDsCore.Base;

namespace MeDsCore.Interactions.Base.Entities;

public class InteractionBaseEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("application_id")]
    public string ApplicationId { get; set; }
    [JsonPropertyName("type")]
    public InteractionType Type { get; set; }
    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }
    [JsonPropertyName("member")]
    public GuildMemberEntity Member { get; set; }
    [JsonPropertyName("user")]
    public UserEntity User { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("message")]
    public MessageEntity Message { get; set; }
    [JsonPropertyName("locale")]
    public string Locale { get; set; }
    [JsonPropertyName("guild_locale")]
    public string GuildLocale { get; set; }
}