using System.Text.Json;
using System.Text.Json.Serialization;
using MeDsCore.Base;

namespace MeDsCore.Interactions.Base.Entities;

public class InteractionApplicationCommandResponseEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("type")]
    public ApplicationCommandType Type { get; set; }
    [JsonPropertyName("guild_id")]
    public string? GuildId { get; set; }
    [JsonPropertyName("options")]
    public InteractionResponseDataOptionEntity[]? Options { get; set; } 
    [JsonPropertyName("resolved")]
    public ResolvedDataEntity? ResolvedData { get; set; }
}

public class ResolvedDataEntity
{
    [JsonPropertyName("channels")]
    public Dictionary<string, ChannelEntity>? Channels { get; set; }
    [JsonPropertyName("users")]
    public Dictionary<string, UserEntity>? Users { get; set; }
    [JsonPropertyName("messages")]
    public Dictionary<string, MessageEntity>? Messages { get; set; }
    [JsonPropertyName("roles")]
    public Dictionary<string, RoleEntity>? Roles { get; set; }
}

public class InteractionResponseDataOptionEntity
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("type")] public ApplicationCommandOptionType Type { get; set; }
    [JsonPropertyName("value")] public JsonElement Value { get; set; }
}