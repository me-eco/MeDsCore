using System.Text.Json.Serialization;

namespace MeDsCore.Base;

public class GuildEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("channels")]
    public ChannelEntity[]? Channels { get; set; }
    [JsonPropertyName("members")]
    public GuildMemberEntity[]? Members { get; set; }
}