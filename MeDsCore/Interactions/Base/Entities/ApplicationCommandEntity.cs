using System.Text.Json.Serialization;
using MeDsCore.Base;

namespace MeDsCore.Interactions.Base.Entities;

public class ApplicationCommandEntity
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("application_id")]
    public string ApplicationId { get; set; }
    [JsonPropertyName("guild_id")]
    public string GuildId { get; set; }
    [JsonPropertyName("type")]
    public ApplicationCommandType Type { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("options")]
    public ApplicationCommandOptionEntity[] Options { get; set; }
    [JsonPropertyName("default_member_permissions")]
    public string DefaultMemberPermissions { get; set; }
    [JsonPropertyName("dm_permission")]
    public bool DmPermission { get; set; }
    [JsonPropertyName("default_permission")]
    public bool DefaultPermission { get; set; }
    [JsonPropertyName("version")]
    public string Version { get; set; }
}

public class ApplicationCommandOptionEntity
{
    [JsonPropertyName("type")]
    public ApplicationCommandOptionType Type { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("required")]
    public bool Required { get; set; }
    [JsonPropertyName("choices")]
    public ApplicationCommandOptionChoiceEntity[] Choices { get; set; }
    [JsonPropertyName("options")]
    public ApplicationCommandOptionEntity[] Options { get; set; }
    [JsonPropertyName("channel_types")]
    public ChannelType[] ChannelTypes { get; set; }
    [JsonPropertyName("min_value")]
    public object MinValue { get; set; }
    [JsonPropertyName("max_value")]
    public object MaxValue { get; set; }
    [JsonPropertyName("autocomplete")]
    public bool Autocomplete { get; set; }
}

public class ApplicationCommandOptionChoiceEntity
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("value")]
    public object Value { get; set; }
}

public enum ApplicationCommandOptionType
{
    SubCommand = 1,
    SubCommandGroup = 2,
    String = 3,
    Integer = 4,
    Boolean = 5,
    User = 6,
    Channel = 7,
    Role = 8,
    Mentionable = 9,
    Number = 10,
    Attachment = 11
}

public enum ApplicationCommandType
{
    ChatInput = 1,
    User = 2,
    Message = 3
}