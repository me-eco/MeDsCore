using System.Reflection;
using System.Text.Json.Serialization;
using MeDsCore.Interactions.Base.Entities;

namespace MeDsCore.Interactions.ApplicationCommands.Modules.Core.CommandInfos;

public abstract class PrivateApplicationCommandInfo : ApplicationCommandInfo
{
    [JsonPropertyName("guild_id")]
    public ulong GuildId { get; }

    public PrivateApplicationCommandInfo(string name, ApplicationCommandType type, MethodInfo reflectionMethodInfo, ulong guildId, string? defaultMemberPermissions) :
        base(name, type, reflectionMethodInfo, defaultMemberPermissions)
    {
        GuildId = guildId;
    }
}